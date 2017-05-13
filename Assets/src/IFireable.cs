using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

	public interface IFireable {
		void Fire(DanmakuInitialState state);
	}

    public abstract class Fireable : IFireable {

        IFireable _subemitter;
        public IFireable Subemitter { get; set; }

        public abstract void Fire(DanmakuInitialState state);

        protected void Subfire(DanmakuInitialState state) {
            if (Subemitter == null)
                return;
            Subemitter.Fire(state);
        }

    }

}
