using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Hurt,
        Death,
        Shoot,
        JumpShoot,
        Attack
    }
    
    private PlayerState currentState;
    
    private Dictionary<PlayerState, string> animatorStates = new Dictionary<PlayerState, string>();
    
    private Animator animator;
    
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        animatorStates = new Dictionary<PlayerState, string>
        {
            { PlayerState.Attack, "Attack" },
            { PlayerState.Death, "Death" },
            { PlayerState.Fall, "Fall" },
            { PlayerState.Hurt, "Hurt" },
            { PlayerState.Idle, "Idle" },
            { PlayerState.Jump, "Jump" },
            { PlayerState.Run, "Run" },
            { PlayerState.Shoot, "Shoot" },
            { PlayerState.JumpShoot, "JumpShoot"}
        };
        
        ChangeState(PlayerState.Idle);
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;
        animator.Play(animatorStates[currentState]);
    }
    
    private void Update()
    {
        PlayerState newState = GetNexState();
        
        if(newState != currentState) ChangeState(newState);
    }

    private PlayerState GetNexState()
    {
        PlayerState highPriorityState = CheckHighPriorityState();
        if (highPriorityState != PlayerState.Idle) return highPriorityState;
        return PlayerState.Idle;
    }

    private PlayerState CheckHighPriorityState()
    {
        if (playerHealth.isDead) return PlayerState.Death;
        if (playerHealth.isHurt) return PlayerState.Hurt;
        return playerMovement.isGrounded ? GetGroundedState() : GetAirborneState();
    }

    private PlayerState GetGroundedState()
    {
        if (playerMovement.isAttacking) return PlayerState.Attack;
        if (playerMovement.isShooting) return PlayerState.Shoot;
        if (playerMovement.isMoving) return PlayerState.Run;
        return PlayerState.Idle;
    }

    private PlayerState GetAirborneState()
    {
        if (playerMovement.isShooting) return PlayerState.JumpShoot;
        return playerMovement.isFalling ? PlayerState.Fall : PlayerState.Jump;
    }
    
    public void EndAttackState()
    {
        if(playerMovement.isAttacking || playerMovement.isShooting)
        {
            playerMovement.isAttacking = false;
            playerMovement.isShooting = false;
        }
    }
}
