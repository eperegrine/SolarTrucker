using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject Target;
    public Camera ActiveCamera;

    public float padding = 20f;
    
    private void Update()
    {
        var sp = ActiveCamera.WorldToScreenPoint(Target.transform.position);
        float xpos = Mathf.Clamp(sp.x, padding, Screen.width-padding);
        float ypos = Mathf.Clamp(sp.y, padding, Screen.height-padding);
        transform.position = new Vector3(xpos, ypos);

    }
}
