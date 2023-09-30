using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CargoManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipLoadingController : MonoBehaviour
{
    //Input
    public InputActionAsset ActionAsset;

    private InputAction SwitchObject;
    private InputAction Move;
    private InputAction Rotate;
    private InputAction Confirm;
    
    //Component
    public float MoveSpeed = 5f;
    public float RotationSpeed = 180f;
    public CargoRegistry Registry;
    public List<MovableCargo> _MovableObjects;
    private MovableCargo _target;

    private int _index = 0;
    
    void Start()
    {
        var ActionMap = ActionAsset.FindActionMap("LoadShip");
        ActionMap.Enable();
        SwitchObject = ActionMap.FindAction("Switch Object");
        Move = ActionMap.FindAction("Movement");
        Rotate = ActionMap.FindAction("Rotate");
        Confirm = ActionMap.FindAction("Confirm");
        SelectMovable(_MovableObjects.First());
    }

    public void SelectMovable(MovableCargo movableCargo)
    {
        if (_target != null) _target.Deselected();
        _target = movableCargo;
        _target.Selected();
    }

    private string saved = "";
    
    private void Update()
    {
        if (SwitchObject.triggered)
        {
            if (_index >= _MovableObjects.Count-1)
            {
                _index = -1;
            }
            _index++;
            SelectMovable(_MovableObjects[_index]);
        }

        if (Confirm.triggered)
        {
            var load = new CargoLoad();
            //TODO: limit to cargo in ship
            //TODO: add storage relative point
            load.LoadFromMovableCargo(transform.position, _MovableObjects);
            saved = load.SaveToString();
            Debug.Log(saved);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _MovableObjects.ForEach(mo => Destroy(mo.gameObject));
            _MovableObjects = new List<MovableCargo>();
            _target = null;
            Debug.Log("Cleared");
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            var load = CargoLoad.LoadFromString(saved);
            _MovableObjects = load.Instantiate(transform.position, Registry);
            _index = 0;
            SelectMovable(_MovableObjects[_index]);
        }
        
    }

    void FixedUpdate()
    {
        if (_target)
        {
            var moveVector = Move.ReadValue<Vector2>();
            var rotation = Rotate.ReadValue<float>();

            var delta = moveVector * (MoveSpeed * Time.fixedDeltaTime);
            Debug.Log(delta);
            _target.Rigidbody.MovePosition((Vector2)_target.transform.position + delta);
            var rotDelta = -rotation * RotationSpeed * Time.fixedDeltaTime;
            _target.Rigidbody.MoveRotation(_target.transform.rotation.eulerAngles.z + rotDelta);
        }
    }
}
