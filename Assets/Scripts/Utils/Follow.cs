using System;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public bool Lerp;
    public float LerpTime = 0.1f;
    public bool X, Y, Z;
    public float XOffset, YOffset, ZOffset;

    public bool LookAt;
    public bool LerpLookAt;
    public float LerpLookatAtime;

    private void Start()
    {
        transform.position = new Vector3(
            (X ? target.position.x + XOffset : transform.position.x),
            (Y ? target.position.y + YOffset : transform.position.y),
            (Z ? target.position.z + ZOffset : transform.position.z));
    }

    void FixedUpdate()
    {
        if (Lerp)
        {
            Vector3 targetpos = Vector3.Lerp(transform.position, target.position, LerpTime);
            transform.position = new Vector3(
                (X ? targetpos.x + XOffset : transform.position.x),
                (Y ? targetpos.y + YOffset : transform.position.y),
                (Z ? targetpos.z + ZOffset : transform.position.z));
        }
        else
        {
            transform.position = new Vector3(
                (X ? target.position.x + XOffset : transform.position.x),
                (Y ? target.position.y + YOffset : transform.position.y),
                (Z ? target.position.z + ZOffset : transform.position.z));
        }

        if (LookAt)
        {
            if (LerpLookAt)
            {
                transform.rotation = Quaternion.LerpUnclamped(
                    transform.rotation,
                    Quaternion.LookRotation(transform.position - target.position, transform.up),
                    LerpLookatAtime);
            }
            else
            {
                transform.LookAt(target);
            }
        }
    }
}