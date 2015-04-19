using UnityEngine;
using UnityUtilLib;

namespace DanmakU {
	
	public class EmitterNode : DanmakuNode {

		[SerializeField]
		private FireBuilder fireData;

		#region implemented abstract members of DanmakuNode
		public override void Process (FireBuilder target) {
			target.Copy(fireData);
		}
		#endregion
		
	}

}
