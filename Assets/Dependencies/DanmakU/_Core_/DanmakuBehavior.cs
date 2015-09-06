// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using UnityEngine;
using Vexe.Runtime.Types;

namespace Hourai.DanmakU {

    public abstract class DanmakuBehaviour : HouraiBehaviour {

        /// <summary>
        /// Shorthand for <c>TimeUtil.DeltaTime</c>.
        /// </summary>
        protected static float dt {
            get { return TimeUtil.DeltaTime; }
        }
    }

}