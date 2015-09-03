using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{
    public static class ColorModifierExtensions
    {
        public static IEnumerable WithColor(this IEnumerable data,
                                            Func<FireData, Color?> color,
                                            Func<FireData, bool> filter = null)
        {
            if (color == null)
                throw new ArgumentNullException("color");
            return data.ForEachFireData(fd => fd.Color = color(fd), filter);
        }

        public static IEnumerable WithColor(this IEnumerable data,
                                            Color? color,
                                            Func<FireData, bool> filter = null)
        {
            return data.ForEachFireData(fd => fd.Color = color, filter);
        }

        public static IEnumerable WithColor(this IEnumerable data,
                                            Gradient gradient,
                                            Func<FireData, bool> filter = null)
        {
            return data.ForEachFireData(fd => fd.Color = gradient.Random(), filter);
        }
    }
}
