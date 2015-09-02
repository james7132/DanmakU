using System.Collections;
using UnityEngine;

namespace Hourai {

    public static class DebugUtil {

        public static void Log(object obj) {
            var enumerable = obj as IEnumerable;
            var iterator = obj as IEnumerator;
            if (enumerable != null) {
                foreach (object contained in enumerable)
                    Debug.Log(contained);
            } else if (iterator != null) {
                while (iterator.MoveNext())
                    Debug.Log(iterator.Current);
            }
        }

    }

}