// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using Vexe.Runtime.Types;

/// <summary>
/// A set of pre-created Danmaku Colliders that can be used
/// </summary>
namespace DanmakU.Collider {
	
	public class ColorChangeCollider : DanmakuCollider {

		//TODO Make a proper custom editor for this class
		//TODO Document

		public enum ColorType { Constant, Random, Gradient }

		[SerializeField, Show]
		private ColorType type;
		public ColorType Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}

		[SerializeField, Show]
		private Color color;

		public Color Color {
			get {
				return color;
			}
			set {
				color = value;
			}
		}

		[SerializeField, Show]
		private Color[] colors;
		public Color[] Colors {
			get {
				return colors;
			}
			set {
				colors = value;
			}
		}

		[SerializeField, Show]
		private Gradient gradient;
		public Gradient Gradient {
			get {
				return gradient;
			}
			set {
				gradient = value;
			}
		}

		private DanmakuGroup affected;

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
		public override void Awake () {
			base.Awake ();
			affected = new DanmakuSet ();
		}

		#region implemented abstract members of DanmakuCollider

		/// <summary>
		/// Handles a Danmaku collision. Only ever called with Danmaku that pass the filter.
		/// </summary>
		/// <param name="danmaku">the danmaku that hit the collider.</param>
		/// <param name="info">additional information about the collision</param>
		protected override void DanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if (affected.Contains (danmaku))
				return;

			switch (type) {
				case ColorType.Constant:
					if(colors.Length > 0)
						danmaku.Color = colors[0];
					break;
				case ColorType.Random:
					if(colors.Length > 0)
						danmaku.Color = colors.Random();
					break;
				case ColorType.Gradient:
					if(gradient != null)
						danmaku.Color = gradient.Evaluate(Random.value);
					break;
			}
			
			affected.Add (danmaku);
		}

		#endregion
	}

}
