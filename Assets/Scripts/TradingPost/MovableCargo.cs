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

        private void Awake()
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
}