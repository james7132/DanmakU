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

//using UnityEngine;
//using UnityUtilLib;
//using System.Collections;
//
//namespace DanmakU.Phantasmagoria {
//
//	[DisallowMultipleComponent]
//	[RequireComponent(typeof(DanmakuTransferBoundary))]
//	[RequireComponent(typeof(Collider2D))]
//	public class BulletTransferArea : PausableGameObject {
//
//		public void Run(float duration, float maxScale, DanmakuField origin) {
//			DanmakuTransferBoundary ptb = GetComponent<DanmakuTransferBoundary> ();
//			ptb.Field = origin;
//			StartCoroutine (Execute (duration, maxScale));
//		}
//
//		private IEnumerator Execute(float duration, float maxScale) {
//			SpriteRenderer rend = GetComponent<SpriteRenderer> ();
//			Vector3 maxScaleV = Vector3.one * maxScale;
//			Vector3 startScale = transform.localScale;
//			Color spriteColor = rend.color;
//			Color targetColor = spriteColor;
//			targetColor.a = 0;
//			float dt = Util.TargetDeltaTime;
//			float t = 0;
//			while (t < 1f) {
//				transform.localScale = Vector3.Lerp(startScale, maxScaleV, t);
//				rend.color = Color.Lerp(spriteColor, targetColor, t);
//				yield return UtilCoroutines.WaitForUnpause(this);
//				t += dt / duration;
//			}
//			Destroy (gameObject);
//		}
//	}
//}