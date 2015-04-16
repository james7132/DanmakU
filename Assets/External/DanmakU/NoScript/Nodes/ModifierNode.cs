using UnityEngine;
using System.Collections;

namespace DanmakU {
	
	public class ModifierNode : DanmakuNode {

		[SerializeField]
		private DanmakuModifier modifier;

		#region implemented abstract members of DanmakuNode
		protected override void Process () {
			Target.Modifier.Append (modifier);
		}
		#endregion




	}

}
