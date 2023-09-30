using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{
    [HideInInspector]
    public SpriteRenderer Renderer;
    [HideInInspector]
    public Rigidbody2D Rigidbody;
    
    public Color SelectedColor = Color.red;
    public Color DeselectedColor = Color.grey;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void Selected()
    {
        Renderer.color = SelectedColor;
    }

    public void Deselected()
    {
        Renderer.color = DeselectedColor;
    }
}