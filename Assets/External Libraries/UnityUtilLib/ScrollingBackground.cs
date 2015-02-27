using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	[RequireComponent(typeof(MeshRenderer))]
	public class ScrollingBackground : CachedObject {

		private Renderer rend;
		public Vector2 differential;
		private Vector2 offset;

		void Start() {
			rend = renderer;
			rend.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
		}

		// Update is called once per frame
		void Update () {
			offset += differential * Time.deltaTime;
			rend.material.SetTextureOffset("_MainTex", offset);
		}
	}
}