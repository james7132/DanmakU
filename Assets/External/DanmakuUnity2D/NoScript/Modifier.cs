// Copyright (C) 2015  James Liu
//	
//	This program is free software: you can redistribute it and/or modify
//	it under the terms of the GNU General Public License as published by
//	the Free Software Foundation, either version 3 of the License, or
//	(at your option) any later version.
//		
//	This program is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//	GNU General Public License for more details.
//			
//	You should have received a copy of the GNU General Public License
//	along with this program.  If not, see <http://www.gnu.org/licenses/>

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
