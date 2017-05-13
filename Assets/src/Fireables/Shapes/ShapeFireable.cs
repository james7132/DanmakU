using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    public abstract class ShapeFireable : Fireable {

        protected abstract IEnumerable<DanmakuInitialState> GetSubemissions(DanmakuInitialState state);
        public override void Fire(DanmakuInitialState state) {
            var subemissions = GetSubemissions(state);
            if (subemissions == null || Subemitter == null)
                return;
            foreach (DanmakuInitialState sub in subemissions)
                Subemitter.Fire(sub);
        }

    }

}

