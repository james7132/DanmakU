using UnityEngine;

namespace DanmakU {
	
	public class EmitterNode : DanmakuNode {

		[SerializeField]
		private FireBuilder fireData;

		#region implemented abstract members of DanmakuNode
		protected override void Process () {
			Target = fireData.Clone ();
		}
		#endregion
		
	}

}
