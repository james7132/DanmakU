using UnityEngine;
using System.Collections;

/// <summary>
/// A set of small utility GUI scripts that can be easily ported from one game to another
/// </summary>
namespace UnityUtilLib.GUI {

	//TODO: Make GUITextShadow work in the editor

	/// <summary>
	/// A way to create a shadow behind a <a href="http://docs.unity3d.com/ScriptReference/GUIText.html">GUIText</a> that mirrors any displayed text at a given color and offset
	/// </summary>
	[RequireComponent(typeof(GUIText))]
	public class GUITextShadow : MonoBehaviour  {

		private GameObject shadow;
		private GUIText shadowText;
		private GUIText text;

		[SerializeField]
		private Vector2 offset;

		[SerializeField]
		private Color shadowColor;

		private void Start() {
			shadow = new GameObject();
			shadow.transform.parent = transform;
			shadow.name = "Shadow";
			shadowText = shadow.AddComponent<GUIText> ();
			text = GetComponent<GUIText> ();
		}

		private void Update () {
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
