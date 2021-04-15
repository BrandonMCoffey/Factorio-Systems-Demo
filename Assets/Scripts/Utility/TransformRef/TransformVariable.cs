using UnityEngine;

namespace Assets.Scripts.Utility.TransformRef {
    [CreateAssetMenu]
    public class TransformVariable : ScriptableObject {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}