using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthDrawer : MonoBehaviour
{
    public float Width;
    public float Height;
    public Vector2 Offset;
    public bool DrawWhenFull = false;

    public Color emptyColour = Color.red;
    public Color fullColour = Color.blue;

    ProgressDrawer _drawer;
    Health health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        _drawer = new ProgressDrawer(Width, Height, transform, Offset, health.MaxHealth, health.CurrentHealth);
    }

    #if UNITY_EDITOR
    private void Update()
    {
        _drawer.Width = Width;
        _drawer.Height = Height;
        _drawer.Offset = Offset;
    }
    #endif

    // Update is called once per frame
    void OnRenderObject()
    {
        if (!DrawWhenFull && health.CurrentHealth >= health.MaxHealth) return;
        
        _drawer.fullColour = this.fullColour;
        _drawer.emptyColour = this.emptyColour;
        _drawer.Value = health.CurrentHealth;
        _drawer.MaxValue = health.MaxHealth;
        _drawer.RenderObject();
    }
}
