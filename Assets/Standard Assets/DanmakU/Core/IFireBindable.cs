using UnityEngine;
using System.Collections;

namespace Hourai.DanmakU {

    public interface IFireBindable {

        void Bind(FireData fireData);
        void Unbind(FireData fireData);

    }

}

