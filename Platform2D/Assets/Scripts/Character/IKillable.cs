using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable<T>
{
    void TakeDamage(T damage);
    void Die();
}
