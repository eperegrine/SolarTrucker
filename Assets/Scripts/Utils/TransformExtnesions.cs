using UnityEngine;

namespace Utils
{
    public static class TransformExtnesions
    {
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform t in transform)
            {
                Object.Destroy(t.gameObject);
            }
        }
    }
}