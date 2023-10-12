
using System;
using SpaceSystem;
using UnityEngine;

public class PlayerHealth : Health
{
    public float CollisonDamageRatio = 1f;
    public float MinDamageAmt = 2f;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var damageAmt = other.relativeVelocity.magnitude * CollisonDamageRatio;
        if (damageAmt <= MinDamageAmt) damageAmt = 0;
        Debug.Log($"Player Damage {damageAmt}");
        TakeDamage(damageAmt);
    }

    public override void RunDeath()
    {
        Debug.Log("Player Dead");
        SpaceSystemManager.Instance.PlayerDied();
    }
}