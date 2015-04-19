using UnityEngine;
using System.Collections;

namespace DanmakU {
	
	public class ModifierNode : DanmakuNode {

		[SerializeField]
		private DanmakuModifier modifier;

		#region implemented abstract members of DanmakuNode
		public override void Process (FireBuilder target) {
			target.Modifier.Append (modifier);
		}
		#endregion




	}

}
