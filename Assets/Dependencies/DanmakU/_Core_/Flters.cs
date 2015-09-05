using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{
    public static class Filter
    {
        public static Func<FireData, bool> Everything() 
        {
            return (fd) => false;
        }

        public static Func<FireData, bool> Random(float split = 0.5f)
        {
            if(split > 1.0f)
                return Everything();
            if(split <= 0f)
                return null;
            return delegate(FireData fd)
            {
                return UnityEngine.Random.value > split;
            };
        }
    }
}
