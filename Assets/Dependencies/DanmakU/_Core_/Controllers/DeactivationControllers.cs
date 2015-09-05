using System;

namespace Hourai.DanmakU
{

    public static partial class Controller
    {
        public static Action<Danmaku> Deactivation(Func<Danmaku, bool> deactivation)
        {
            if (deactivation == null)
                throw new ArgumentNullException("deactivation");
            return delegate(Danmaku danmaku)
            {
                if (deactivation(danmaku))
                    danmaku.Destroy();
            };
        }
    }

}

