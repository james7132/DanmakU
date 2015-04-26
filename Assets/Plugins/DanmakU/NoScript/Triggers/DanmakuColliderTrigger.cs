// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Text.RegularExpressions;

namespace DanmakU  {

	[RequireComponent(typeof(Collider2D))]
	[AddComponentMenu("DanmakU/Triggers/Projectile Collider Trigger")]
	public class DanmakuColliderTrigger : DanmakuTrigger, IDanmakuCollider {

		[SerializeField]
		private string tagFilter;
		
		private Regex validTags;
		
		public override void Awake() {
			base.Awake ();
			if (string.IsNullOrEmpty (tagFilter))
				validTags = null;
			else
				validTags = new Regex (tagFilter);
		}

		#region IDanmakuCollider implementation
		public void OnDanmakuCollision (Danmaku danmaku, RaycastHit2D info) {
			if(validTags == null || validTags.IsMatch(danmaku.Tag)) {
				Trigger();
			}
		}
		#endregion




	}
}
