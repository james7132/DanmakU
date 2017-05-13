using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    public static class FireableExtensions {

        static Fireable GetLowestChild(Fireable fireable) {
            var last = fireable;
            while (fireable != null) {
                last = fireable;
                fireable = fireable.Subemitter as Fireable;
            }
            return last;
        }

        public static Fireable Of(this Fireable fireable, IFireable subemitter) {
            if (fireable == null)
                throw new ArgumentNullException("fireable");
            var lowest = GetLowestChild(fireable);
            lowest.Subemitter = subemitter;
            return fireable;
        }

        public static Fireable Of(this Fireable fireable, IEnumerable<IFireable> subemitters) {
            return fireable.Of(new RandomSubemitterFireable(subemitters));
        }

        public static Fireable Of(this Fireable fireable, params IFireable[] subemitters) {
            return fireable.Of(new RandomSubemitterFireable(subemitters));
        }

    }

}
