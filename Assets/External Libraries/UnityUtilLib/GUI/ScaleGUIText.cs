using UnityEngine;
using System.Collections;

namespace UnityUtilLib.GUI {
	[RequireComponent(typeof(GUIText))]
	public class ScaleGUIText : MonoBehaviour {

		[SerializeField]
		private float originalScreenWidth = 1024f;

		private int originalFontSize;
		private GUIText cachedGUIText;

		void Awake()
		{
			cachedGUIText = guiText;
			originalFontSize = cachedGUIText.fontSize;
		}
		
		// Update is called once per frame
		void Update () 
		{
			cachedGUIText.fontSize = (int)((float)originalFontSize * ((float)Screen.width / originalScreenWidth));
		}
	}
}