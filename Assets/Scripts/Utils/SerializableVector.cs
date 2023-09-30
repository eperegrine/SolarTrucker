using Newtonsoft.Json;

namespace Utils
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SerializableVector
    {
        public float x;
        public float y;
        public float z;

        [JsonIgnore]
        public Vector3 Vector
        {
            get => new Vector3(x, y, z);
            set { x = value.x; y = value.y; z = value.z; }
        }

        public SerializableVector()
        {
            Vector = Vector3.zero;
        }

        public SerializableVector(Vector3 vec)
        {
            Vector = vec;
        }

        public static implicit operator SerializableVector(Vector3 value) => new SerializableVector(value);
        public static implicit operator Vector3(SerializableVector sv) => sv.Vector;
    }
}