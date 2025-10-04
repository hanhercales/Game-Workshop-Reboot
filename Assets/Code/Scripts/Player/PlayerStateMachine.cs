using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Attack,
        Death,
        Fall,
        Hurt,
        Idle,
        Jump,
        Run,
        Shoot,
        JumpShoot
    }
    public PlayerState currentState;
    public Animator animator;
    private Dictionary<PlayerState, string> animatorStates;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
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

    private void Update()
    {
        PlayerState newState = GetNextState();

        if (currentState != newState)
        {
            ChangeState(newState);
        }
    }

    private PlayerState GetNextState()
    {
        PlayerState highPriorityState = CheckHighPriorityState();
        if (highPriorityState != PlayerState.Idle) return highPriorityState;
        return PlayerState.Idle;
    }
    
    private PlayerState CheckHighPriorityState()
    {
        if (playerMovement.isDead) return PlayerState.Death;
        if (playerMovement.isHurt) return PlayerState.Hurt;
        return playerMovement.isGrounded ? GetGroundedState() : GetAirborneState();
    }
    
    private PlayerState GetAirborneState()
    {
        if (playerMovement.isShooting) return PlayerState.JumpShoot;
        return playerMovement.isFalling ? PlayerState.Fall : PlayerState.Jump;
    }
    
    private PlayerState GetGroundedState()
    {
        if (playerMovement.isAttacking) return PlayerState.Attack;
        if (playerMovement.isShooting) return PlayerState.Shoot;
        if (playerMovement.isMoving) return PlayerState.Run;
        return PlayerState.Idle;
    }
    
    private void ChangeState(PlayerState newState)
    {
        if(currentState == newState) return;

        currentState = newState;
        if (animatorStates.ContainsKey(newState))
        {
            animator.Play(animatorStates[newState]);
        }
        else
        {
            Debug.LogError("State animation not found");
        }
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
