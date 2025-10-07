using System.Collections.Generic;
using UnityEngine;

public class SlimeStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Hurt,
        Death,
        Move
    }
    
    public EnemyState currentState;

    public Animator animator;

    public Dictionary<EnemyState, string> stateAnimations;

    private Enemy enemy;
    private Slime slime;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        slime = GetComponent<Slime>(); // Get the Slime component
        animator = GetComponent<Animator>(); // Make sure animator is also set here
    }

    private void Start()
    {
        stateAnimations = new Dictionary<EnemyState, string>
        {
            { EnemyState.Idle, "Idle" },
            { EnemyState.Move, "Move"},
            { EnemyState.Death, "Death"},
            { EnemyState.Hurt, "Hurt"}
        };

        ChangeState(EnemyState.Idle);
    }
    
    private void Update()
    {
        EnemyState newState = GetNextState();

        if (currentState != newState)
        {
            ChangeState(newState);
        }
    }
    
    public EnemyState GetNextState()
    {
        EnemyState highPriorityState = CheckHighPriorityState();
        if(highPriorityState != EnemyState.Idle) return highPriorityState;
        
        if (slime.isMoving) return EnemyState.Move;
        
        return EnemyState.Idle;
    }
    
    private EnemyState CheckHighPriorityState()
    {
        if (enemy.isDead) return EnemyState.Death;
        if (enemy.isHurt) return EnemyState.Hurt;
        return EnemyState.Idle;
    }

    public void ChangeState(EnemyState newState)
    {
        if(currentState == newState) return;

        currentState = newState;
        if (stateAnimations.ContainsKey(newState))
        {
            animator.Play(stateAnimations[newState]);
        }
        else
        {
            Debug.LogError("State animation not found for: " + newState);
        }
    }
}