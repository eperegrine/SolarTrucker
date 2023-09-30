using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject Target;
    public Camera ActiveCamera;

    private void Update()
    {
        var sp = ActiveCamera.WorldToScreenPoint(Target.transform.position);
        float xpos = Mathf.Clamp(sp.x, 0, Screen.width);
        float ypos = Mathf.Clamp(sp.y, 0, Screen.height);
        transform.position = new Vector3(xpos, ypos);

    }
}
