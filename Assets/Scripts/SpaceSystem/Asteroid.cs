using System;
using SpaceSystem;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
    private Rigidbody2D _rb;

    [FormerlySerializedAs("InAccuracy")] 
    public float AngleAdjustmentRange = 1f;

    public float FlingForce = 10f;
    
    public Transform Target;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        Target = FindObjectOfType<PlayerShipController>().transform;
        Fling();
    }

    #if UNITY_EDITOR
    private void Update()
    {
        DrawRanges(GetDirectionToTarget());
    }
    #endif

    public void Fling()
    {
        var dir = GetDirectionToTarget();
        dir = AdjustDirectionVector(dir, Random.Range(-AngleAdjustmentRange, AngleAdjustmentRange));
        _rb.AddForce(dir * FlingForce,ForceMode2D.Impulse);
    }

    private Vector2 GetDirectionToTarget()
    {
        var dir = (Vector2)(transform.position - Target.position);
        dir = -dir.normalized;
        return dir;
    }

    public Vector3 AdjustDirectionVector(Vector3 direction, float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward) * direction;
    }
    
    private void DrawRanges(Vector2 dir)
    {
        var upperBound = Quaternion.AngleAxis(AngleAdjustmentRange, Vector3.forward) * dir;
        var lowerBound = Quaternion.AngleAxis(-AngleAdjustmentRange, Vector3.forward) * dir;
        Debug.DrawLine(transform.position, transform.position + upperBound, Color.red);
        Debug.DrawLine(transform.position, (transform.position + (Vector3)dir), Color.magenta);
        Debug.DrawLine(transform.position, transform.position + lowerBound, Color.green);
    }
}