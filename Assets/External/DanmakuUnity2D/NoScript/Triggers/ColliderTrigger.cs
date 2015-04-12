using UnityEngine;
using System.Collections.Generic;

namespace Danmaku2D {

	[RequireComponent(typeof(Collider2D)), AddComponentMenu("Danmaku 2D/Triggers/Collider Trigger")]
	public class ColliderTrigger : DanmakuTrigger {
		
		[SerializeField]
		private string[] tagFilter;

		void OnCollisionEnter2D(Collision2D collision) {
			TriggerCheck (collision.gameObject);
		}

		void OnTriggerEnter2D(Collider2D other) {
			TriggerCheck (other.gameObject);
		}

		void TriggerCheck(GameObject gameObject) {
			for(int i = 0; i < tagFilter.Length; i++) {
				if(gameObject.CompareTag(tagFilter[i])) {
					Trigger();
					break;
				}
			}
		}

	}

}
