using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float Damage = 5f;
    public float DestroyAfterSeconds = 10f;

    float timeCreated;

    private void Start()
    {
        timeCreated = Time.time; 
    }

    private void Update()
    {
        if (Time.time - timeCreated > DestroyAfterSeconds * 1000)
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health h = collision.gameObject.GetComponent<Health>();
        if (h) h.TakeDamage(Damage);
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
