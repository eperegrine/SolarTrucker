using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShipController : MonoBehaviour
{
    //Input
    public InputActionAsset ActionAsset;

    private InputAction Acceleration;
    private InputAction Rotation;

    //Class
    public float AccelSpeed = 5f;
    public float RotSpeed = 2f;
    
    private Rigidbody2D _rb;
    
    void Start()
    {
        var ActionMap = ActionAsset.FindActionMap("FlyShip");
        ActionMap.Enable();
        Acceleration = ActionMap.FindAction("Acceleration");
        Rotation = ActionMap.FindAction("Rotation");

        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var accel = -Acceleration.ReadValue<float>();
        var rot = -Rotation.ReadValue<float>();
        
        _rb.AddForce(transform.up * accel * AccelSpeed);
        _rb.AddTorque(rot * RotSpeed);
    }
}
