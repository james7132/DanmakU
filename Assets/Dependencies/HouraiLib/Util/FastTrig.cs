using UnityEngine;
using System.Collections;

namespace Hourai
{

    public static class FastTrig
    {
        /// <summary>
        /// A set of 2D vectors corresponding to unit circle coordinates
        /// Precalculated since Cosine and Sine calculations are expensive when called thousands of times per frame.
        /// An array access on an array of structs is much cheaper than calling both Mathf.Cos, and Mathf.Sin.
        /// </summary>
        internal static Vector2[] unitCircle;

        private const float DefaultAngleResolution = 0.01f;

        private static float _angRes = DefaultAngleResolution;
        private static float invAngRes;
        private static int unitCircleMax;

        public static float AngleResolution
        {
            get { return _angRes; }
            set
            {
                value = Mathf.Abs(value);
                bool changed = _angRes != value;
                _angRes = value;
                if (changed)
                    Calculate();
            }
        }

        static FastTrig()
        {
            Calculate();
        }

        static void Calculate()
        {
            invAngRes = 1f / _angRes;
            unitCircleMax = Mathf.CeilToInt(360f / _angRes);
            float angle = 90f;
            unitCircle = new Vector2[unitCircleMax];
            for (int i = 0; i < unitCircleMax; i++)
            {
                angle += _angRes;
                unitCircle[i] = Util.OnUnitCircle(angle);
            }
        }


        public static Vector2 UnitCircle(float angle)
        {
            //Clamp the angle to the range [0,360]
            angle = (angle % 360f + 360f) % 360f;
            return unitCircle[(int)(angle * invAngRes)];
        }

        public static float Cos(float angle)
        {
            return UnitCircle(angle).x;
        }

        public static float Sin(float angle)
        {
            return UnitCircle(angle).y;
        }

        public static float Tan(float angle)
        {
            Vector2 uc = UnitCircle(angle);
            if (uc.x == 0f)
                return float.NaN;
            return uc.y / uc.x;
        }
    }
}
