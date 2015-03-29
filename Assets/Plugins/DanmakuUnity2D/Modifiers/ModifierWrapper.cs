using UnityEngine;
using System.Collections;

namespace Danmaku2D {

	public abstract class ModifierWrapper : MonoBehaviour {

		public abstract FireModifier Modifier {
			get;
		}

	}

	public abstract class ModifierWrapper<T> : ModifierWrapper where T : FireModifier {

		[SerializeField]
		private T modifier;

		public override FireModifier Modifier {
			get {
				return modifier;
			}
		}

		[SerializeField]
		private ModifierWrapper subModifier;

		public ModifierWrapper SubModifier {
			get {
				return subModifier;
			}
			set {
				subModifier = value;
				if(subModifier != null)
					modifier.SubModifier = subModifier.Modifier;
			}
		}

		public void Awake() {
			if(subModifier != null)
				modifier.SubModifier = subModifier.Modifier;
		}

	}
}
