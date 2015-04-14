using UnityEngine;
using System;
using System.Collections.Generic;
using Vexe.Runtime.Types;

namespace Danmaku2D {

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class DanmakuNoScriptContainer : BetterBehaviour {

		[SerializeField]
		private DanmakuTrigger[] triggers;
		
		[SerializeField]
		private Dictionary<Guid, Source> sourceMap = new Dictionary<Guid, Source>();

		[System.Serializable]
		private class Source : ISerializationCallbackReceiver {
			public string name;
			public Transform transform;
			[SerializeField]
			private string identificationString;
			public Guid identifier;

			public Source(Transform transform) { 
				this.transform = transform;
				name = transform.name;
				RegenerateID();
			}

			public void RegenerateID() {
				identifier = Guid.NewGuid();
			}

			#region ISerializationCallbackReceiver implementation

			public void OnBeforeSerialize () {
				identificationString = identifier.ToString ();
			}

			public void OnAfterDeserialize () {
				if (identificationString == null)
					RegenerateID ();
				else 
					identifier = new Guid (identificationString);
			}

			#endregion
		}

		#if UNITY_EDITOR
		void Update() {
			if (Application.isEditor) {
				triggers = GetComponentsInChildren<DanmakuTrigger>();
				foreach(Source source in sourceMap.Values) {
					if(source.identifier == Guid.Empty)
						source.RegenerateID();
				}
			}
			DanmakuNoScriptContainer[] containers = GetComponentsInChildren<DanmakuNoScriptContainer>();
			foreach(var container in containers) {
				if(container != this)
					DestroyImmediate(container);
			}
		}
		#endif
	}

}
