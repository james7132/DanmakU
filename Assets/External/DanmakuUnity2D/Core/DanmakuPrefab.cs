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
using System.Collections.Generic;

namespace Danmaku2D {

	/// <summary>
	/// A container behavior used on prefabs to define how a bullet looks or behaves
	/// </summary>
	[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(SpriteRenderer)), AddComponentMenu("Danmaku 2D/Danmaku Prefab")]
	public sealed class DanmakuPrefab : DanmakuObjectPrefab {

		private DanmakuPrefab runtime;

		[SerializeField]
		private IDanmakuController[] extraControllers;
		internal IDanmakuController[] ExtraControllers {
			get {
				return extraControllers;
			}
		}

		internal DanmakuPrefab GetRuntime() {
			if(runtime == null)
				runtime = CreateRuntimeInstance(this);
			return runtime;
		}

		private static DanmakuPrefab CreateRuntimeInstance(DanmakuPrefab prefab) {
			DanmakuPrefab runtime = (DanmakuPrefab)Instantiate (prefab);
			runtime.gameObject.hideFlags = HideFlags.HideInHierarchy;
			runtime.gameObject.SetActive (false);
			return runtime;
		}
	}
}