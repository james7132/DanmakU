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
using System.Collections.Generic;

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	/// <summary>
	/// A script for defining boundaries for detecting collision with Projectiles
	/// </summary>
	[RequireComponent(typeof(Collider2D))]
	public class DanmakuBoundary : MonoBehaviour, IDanmakuCollider {

		/// <summary>
		/// A filter for a set of tags, delimited by "|" for selecting which bullets to affect
		/// Leaving this blank will affect all bullets
		/// </summary>
		[SerializeField]
		private string tagFilter;

		private HashSet<string> validTags;

		/// <summary>
		/// Called on Component instantiation
		/// </summary>
		void Awake() {
			if (tagFilter == null || tagFilter == "")
				validTags = new HashSet<string> ();
			else
				validTags = new HashSet<string>(tagFilter.Split ('|'));
		}

		/// <summary>
		/// Called on collision with any Danmaku
		/// </summary>
		/// <param name="proj">Proj.</param>
		public void OnDanmakuCollision(Danmaku danmaku) {
			if(validTags.Count <= 0 || validTags.Contains(danmaku.Tag)) {
				ProcessDanmaku(danmaku);
			}
		}

		/// <summary>
		/// Processes a danmaku.
		/// By default, this deactivates all Projectiles that come in contact with the ProjectileBoundary
		/// Override this in subclasses for alternative behavior.
		/// </summary>
		/// <param name="proj">the projectile to process</param>
		protected virtual void ProcessDanmaku(Danmaku danmaku) {
			danmaku.Deactivate();
		}
	}
}