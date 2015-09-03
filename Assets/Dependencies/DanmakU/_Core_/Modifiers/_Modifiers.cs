using System;

namespace Hourai.DanmakU {

    public static partial class Modifier {

        public static Func<FireData, T> Wrap<T>(this Func<T> func)
        {
            return (fd) => func();
        }

        public static Func<FireData, T> Wrap<T>(this T val)
        {
            return (fd) => val;
        }

    }

}
