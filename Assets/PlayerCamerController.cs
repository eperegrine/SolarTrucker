using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamerController : MonoBehaviour
{
    public Rigidbody2D Target;
    public Camera cam;

    public float MinSize = 7f;
    public float MaxSize = 40f;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Target.velocity.magnitude);
        var newSize = Target.velocity.magnitude;
        newSize = Mathf.Max(MinSize, newSize);
        newSize = Mathf.Min(MaxSize, newSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, .1f);
    }
}
