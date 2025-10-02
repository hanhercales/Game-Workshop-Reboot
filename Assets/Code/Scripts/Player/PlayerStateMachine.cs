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
    public Dictionary<PlayerState, string> animatorStates;
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

    public PlayerState GetNextState()
    {
        PlayerState highPriorityState = CheckHighPriorityState();
        if (highPriorityState != PlayerState.Idle) return highPriorityState;

        if (!playerMovement.isGrounded) return GetAirborneState();
        else return GetGroundedState();
    }
    
    private PlayerState CheckHighPriorityState()
    {
        if (playerMovement.isDead) return PlayerState.Death;
        if (playerMovement.isHurt) return PlayerState.Hurt;
        return PlayerState.Idle;
    }
    
    private PlayerState GetAirborneState()
    {
        if (playerMovement.isShooting) return PlayerState.JumpShoot;
        else
        {
            if(playerMovement.isFalling) return PlayerState.Fall;
            return PlayerState.Jump;
        }
    }
    
    private PlayerState GetGroundedState()
    {
        if (playerMovement.isMoving) return PlayerState.Run;
        if (playerMovement.isShooting) return PlayerState.Shoot;
        if (playerMovement.isAttacking) return PlayerState.Attack;
        return PlayerState.Idle;
    }
    
    public void ChangeState(PlayerState newState)
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
    
}
