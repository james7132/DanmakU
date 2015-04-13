using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku2D {

	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class DanmakuNoScriptContainer : MonoBehaviour {

		[SerializeField]
		private DanmakuTrigger[] triggers;

		[SerializeField]
		private List<Source> sources;

		private Dictionary<Guid, Source> sourceMap;

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

		void OnEnable() {
			sourceMap = new Dictionary<Guid, Source> ();
			for (int i = 0; i < sources.Count; i++) {
				sourceMap[sources[i].identifier] = sources[i];
			}
		}

		#if UNITY_EDITOR
		void Update() {
			if (Application.isEditor) {
				triggers = GetComponentsInChildren<DanmakuTrigger>();
				for(int i = 0; i < sources.Count; i++) {
					if(sources[i].identifier == Guid.Empty)
						sources[i].RegenerateID();
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
