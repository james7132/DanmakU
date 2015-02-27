using UnityEngine;
using System.Collections;

namespace UnityUtilLib.GUI {

	[RequireComponent(typeof(GUIText))]
	public class GUITextShadow : MonoBehaviour  {

		private GameObject shadow;
		private GUIText shadowText;
		private GUIText text;

		[SerializeField]
		private Vector2 offset;

		[SerializeField]
		private Color shadowColor;

		void Start() {
			shadow = new GameObject();
			shadow.transform.parent = transform;
			shadow.name = "Shadow";
			shadowText = shadow.AddComponent<GUIText> ();
			text = GetComponent<GUIText> ();
		}

		void Update () {
			shadowText.alignment = text.alignment;
			shadowText.anchor = text.anchor;
			shadowText.color = shadowColor;
			shadowText.font = text.font;
			shadowText.fontSize = text.fontSize;
			shadowText.fontStyle = text.fontStyle;
			shadowText.lineSpacing = text.lineSpacing;
			//shadowText.material = text.material;
			shadowText.pixelOffset = text.pixelOffset;
			shadowText.richText = text.richText;
			shadowText.tabSize = text.tabSize;
			shadowText.text = text.text;
			shadow.transform.position = transform.position + new Vector3(offset.x, offset.y, -1f);
		}
	}
}
