using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth = 10f;
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amt)
    {
        CurrentHealth -= amt;
        if (CurrentHealth <= 0)
        {
            RunDeath();
        }
    }

    public virtual void RunDeath()
    {
        Destroy(this.gameObject);
    }
}
