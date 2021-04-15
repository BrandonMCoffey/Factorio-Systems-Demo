using System;
using UnityEngine;

namespace Assets.Scripts.Utility.TransformRef {
    [Serializable]
    public class TransformReference {
        public bool UseConstant;
        public Transform ConstantValue;
        public TransformVariable Variable;

        public TransformReference()
        {
            UseConstant = true;
            ConstantValue = null;
        }

        public Vector3 Position => UseConstant ? ConstantValue.position : Variable.Position;
        public Quaternion Rotation => UseConstant ? ConstantValue.rotation : Variable.Rotation;
        public Vector3 Forward => UseConstant ? ConstantValue.forward : Variable.Rotation * Vector3.forward;
    }
}