using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Attack();
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Attack()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= 2 && !enemy.isAttacking)
        {
            enemy.isAttacking = true;
            enemy.StartCoroutine(enemy.DoAttack(enemy.activationDelay, enemy.enemyAttackDelay));
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
