using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU.Fireables {

    public abstract class ShapeFireable : Fireable {

        protected abstract IEnumerable<DanmakuState> GetSubemissions(DanmakuState state);

        public override void Fire(DanmakuState state) {
            var subemissions = GetSubemissions(state);
            if (subemissions == null || Child == null)
                return;
            foreach (DanmakuState sub in subemissions)
                Child.Fire(sub);
        }

    }

}

