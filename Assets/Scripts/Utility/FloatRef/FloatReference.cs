using System;

namespace Assets.Scripts.Utility.FloatRef {
    [Serializable]
    public class FloatReference {
        public bool UseConstant;
        public float ConstantValue;
        public FloatVariable Variable;

        public FloatReference()
        {
            UseConstant = true;
            ConstantValue = 0;
        }

        public FloatReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value => UseConstant ? ConstantValue : Variable.Value;

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}