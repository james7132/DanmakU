using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	public abstract class Modifier : CachedObject {

		public abstract FireModifier WrappedModifier {
			get;
		}

	}

	public abstract class ModifierWrapper<T> : Modifier where T : FireModifier {

		[SerializeField]
		private T modifier;

		public override FireModifier WrappedModifier {
			get {
				return modifier;
			}
		}

		[SerializeField]
		private Modifier subModifier;

		public Modifier SubModifier {
			get {
				return subModifier;
			}
			set {
				subModifier = value;
				if(subModifier != null)
					modifier.SubModifier = subModifier.WrappedModifier;
			}
		}

		public override void Awake() {
			base.Awake ();
			if(subModifier != null)
				modifier.SubModifier = subModifier.WrappedModifier;
		}

	}
}
