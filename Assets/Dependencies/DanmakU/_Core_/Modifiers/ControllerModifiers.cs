using UnityEngine;
using System;
using System.Collections;

namespace Hourai.DanmakU
{

    public static class ControllerModifierExtensions
    {

        public static IEnumerable WithController(this IEnumerable data,
                                                 Func<FireData, Action<Danmaku>> controller,
                                                 Func<FireData, bool> filter = null)
        {
            if (controller == null)
                return data;
            return data.ForEachFireData(fireData => fireData.Controller += controller(fireData), filter);
        }

        public static IEnumerable WithController(this IEnumerable data,
                                                 Action<Danmaku> controller,
                                                 Func<FireData, bool> filter = null)
        {
            if (controller == null)
                return data;
            return data.ForEachFireData(fireData => fireData.Controller += controller, filter);
        }

        public static IEnumerable WithoutControllers(this IEnumerable data, Func<FireData, bool> filter = null)
        {
            return data.ForEachFireData(fireData => fireData.Controller = null, filter);
        }
    }
}

