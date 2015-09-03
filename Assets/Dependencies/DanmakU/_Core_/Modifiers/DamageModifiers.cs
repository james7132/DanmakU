using System;
using System.Collections;

namespace Hourai.DanmakU
{
    public static class DamageModifierExtensions
    {
        public static IEnumerable WithDamage(this IEnumerable coroutine,
                                            Func<FireData, float> damage,
                                            Func<FireData, bool> filter = null)
        {
            if (damage == null)
                throw new ArgumentNullException("damage");
            return coroutine.ForEachFireData(d => d.Damage = damage(d), filter);
        }

        public static IEnumerable WithDamage(this IEnumerable coroutine,
                                            float damage,
                                            Func<FireData, bool> filter = null)
        {
            return coroutine.ForEachFireData(d => d.Damage = damage, filter);
        }
    }
}

