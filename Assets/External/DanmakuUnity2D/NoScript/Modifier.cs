using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D {

	public abstract class Modifier : CachedObject, IDanmakuNode {
		
		[SerializeField]
		private Modifier subModifier;

		public Modifier SubModifier {
			get {
				return subModifier;
			}
			set {
				subModifier = value;
				if(subModifier != null)
					WrappedModifier.SubModifier = subModifier.WrappedModifier;
			}
		}

		public abstract FireModifier WrappedModifier {
			get;
		}
		
		public override void Awake() {
			base.Awake ();
			if(subModifier != null)
				WrappedModifier.SubModifier = subModifier.WrappedModifier;
		}
		
		#region IDanmakuNode implementation
		public virtual bool Connect (IDanmakuNode node) {
			if (node is Modifier) {
				SubModifier = node as Modifier;
				return true;
			}
			return false;
		}

		public virtual string NodeName {
			get {
				return GetType().Name;
			}
		}

		public Color NodeColor {
			get {
				return Color.green;
			}
		}
		#endregion
	}

	public abstract class Modifier<T> : Modifier where T : FireModifier {

		[SerializeField]
		private T modifier;
		
		public override FireModifier WrappedModifier {
			get {
				return modifier;
			}
		}

		public override string NodeName {
			get {
				return typeof(T).ToString();
			}
		}
	}
}
