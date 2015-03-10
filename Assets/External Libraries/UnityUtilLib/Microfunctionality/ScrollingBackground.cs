using UnityEngine;
using System.Collections;

namespace UnityUtilLib {

	/// <summary>
	/// Creates a scrolling background by offsetting the renderer's Main Texture
	/// </summary>
	[RequireComponent(typeof(MeshRenderer))]
	public class ScrollingBackground : CachedObject {

		private Renderer rend;
		[SerializeField]
		private Vector2 differential;
		private Vector2 offset;

		/// <summary>
		/// Called on MonoBehavior start
		/// </summary>
		void Start() {
			rend = GetComponent<MeshRenderer>();
			rend.material.mainTexture.wrapMode = TextureWrapMode.Repeat;
		}

		/// <summary>
		/// Called once per frame
		/// </summary>
		void Update () {
			offset += differential * Time.deltaTime;
			rend.material.SetTextureOffset("_MainTex", offset);
		}
	}
}