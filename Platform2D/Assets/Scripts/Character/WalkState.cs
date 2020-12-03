using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Debug.Log("Walking");
        Walk();
        enemy.Move();
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    private void Walk()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) > 10)
        {
            enemy.ChangeState(new IdleState());
        }

        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= 2)
        {
            enemy.ChangeState(new AttackState());
        }
    }
}
