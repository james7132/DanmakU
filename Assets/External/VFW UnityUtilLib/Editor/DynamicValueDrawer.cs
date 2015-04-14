using UnityEngine;
using System.Collections;
using Vexe.Editor.Drawers;

namespace UnityUtilLib.Editor.VFW {
	
	public class DynamicIntDrawer : BasicDrawer<DynamicInt> {
		protected override DynamicInt DoField (string text, DynamicInt value) {
			int center = 0;
//			int range = 0;
			using (gui.Horizontal ()) 
			{
				center = gui.Int (text, value);
			}
			return center;
		}
	}

	public class DynamicFloatDrawer : BasicDrawer<DynamicFloat> {
		protected override DynamicFloat DoField (string text, DynamicFloat value) {
			float center = 0;
//			float range = 0;
			using (gui.Horizontal ()) 
			{
				center = gui.Float(text, value);
			}
			return center;
		}
	}

}
