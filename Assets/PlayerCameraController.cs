using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Rigidbody2D Target;
    public Camera cam;

    public float Multiplier = 2f;
    public float MinSize = 7f;
    public float MaxSize = 40f;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var newSize = Multiplier * Target.velocity.magnitude;
        newSize = Mathf.Max(MinSize, newSize);
        newSize = Mathf.Min(MaxSize, newSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newSize, 3f);
    }
}
