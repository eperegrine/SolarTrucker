﻿using System;
using System.Linq;
using UnityEngine;

namespace TradingPost
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovableCargo : MonoBehaviour
    {
        [HideInInspector]
        public SpriteRenderer Renderer;
        [HideInInspector]
        public Rigidbody2D Rigidbody;

        public CargoObject CargoInfo;
    
        public Color SelectedColor = Color.red;
        public Color DeselectedColor = Color.grey;

        private Collider2D thisCollider;
        private bool inLoadingArea;
        private bool selected;
        public bool playerOwned = true;
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<SpriteRenderer>();
            thisCollider = GetComponent<Collider2D>();
        }

        public void Selected()
        {
            selected = true; //Renderer.color = SelectedColor;
            UpdateColor();
        }

        public void Deselected()
        {
            selected = false; //Renderer.color = DeselectedColor;
            UpdateColor();
        }

        public void SetInLoadingArea(bool inArea)
        {
            inLoadingArea = inArea;
            UpdateColor();
        }

        public void UpdateColor()
        {
            var color = Color.white;
            if (selected)
            {
                if (inLoadingArea)
                {
                    color = Color.green;
                }
                else
                {
                    color = SelectedColor;
                }
            }
            else
            {
                color = DeselectedColor;
            }

            Renderer.color = color;
        }

        public bool InArea(Collider2D targetCollider2D)
        {

            if (thisCollider is PolygonCollider2D thisPoly)
            {
                return thisPoly.points.All(p =>
                {
                    var worldPos = transform.position + transform.right * p.x + transform.up * p.y;

                    return targetCollider2D.OverlapPoint(worldPos);
                });
            }
            
            if (thisCollider is BoxCollider2D thisBox)
            {
                return InAreaBox(targetCollider2D, thisBox);
            }

            Debug.LogError($"Cannot check movable cargo with Collider type {thisCollider.GetType()}", thisCollider);
            return false;

        }

        private void OnDrawGizmos()
        {
            if (thisCollider is PolygonCollider2D thisPoly)
            {
                foreach (var p in thisPoly.points)
                {
                    var world = transform.position + transform.right * p.x + transform.up * p.y;
                    Gizmos.DrawSphere(world, .1f);

                    // Gizmos.DrawSphere((Vector2)transform.position+p, .1f);
                }
            }
        }

        private bool InAreaBox(Collider2D targetCollider2D, BoxCollider2D thisBox)
        {
            var scaleSize = thisBox.size * transform.localScale;
            var topRight = transform.position + (scaleSize.x * 0.5f * transform.right + scaleSize.y * 0.5f * transform.up);
            var bottomRight = transform.position + (scaleSize.x * 0.5f * transform.right + scaleSize.y * 0.5f * -transform.up);
            var topLeft = transform.position + (scaleSize.x * 0.5f * -transform.right + scaleSize.y * 0.5f * transform.up);
            var bottomLeft = transform.position + (scaleSize.x * 0.5f * -transform.right + scaleSize.y * 0.5f * -transform.up);

            var inArea = targetCollider2D.OverlapPoint(topRight) &&
                         targetCollider2D.OverlapPoint(bottomRight) &&
                         targetCollider2D.OverlapPoint(bottomLeft) &&
                         targetCollider2D.OverlapPoint(topLeft);

            var col = inArea ? Color.red : Color.blue;
            Debug.DrawLine(topRight, bottomRight, col);
            Debug.DrawLine(bottomRight, bottomLeft, col);
            Debug.DrawLine(bottomLeft, topLeft, col);
            Debug.DrawLine(topLeft, topRight, col);
            return inArea;
        }
    }
}