using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Idle();
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Idle()
    {
        enemy.Animator.SetBool("IsIdling", true);

        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= 10 && !enemy.isAttacking)
        {
            enemy.ChangeState(new WalkState());
        }

        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= 2 && !enemy.isAttacking)
        {
            enemy.ChangeState(new AttackState());
        }
    }
}
