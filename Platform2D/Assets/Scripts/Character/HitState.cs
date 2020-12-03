using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : IEnemyState
{
    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        Hit();
    }

    public void Exit()
    {
        
    }

    public void OnTriggerEnter(Collider2D other)
    {
        enemy.TakeDamage(other.gameObject.GetComponentInParent<Player>().swordDamage);
    }

    private void Hit()
    {
        enemy.Animator.SetTrigger("GetsHit");
        SoundManager.instance.PlayEnemyHit();
        enemy.ChangeState(new IdleState());
    }
}
