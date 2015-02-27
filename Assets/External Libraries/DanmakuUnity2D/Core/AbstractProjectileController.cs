using UnityEngine;
using System;
using System.Collections.Generic;

namespace Danmaku2D {
	public abstract class AbstractProjectileController  {
		private string unique_id;
		public string ID {
			get {
				if(unique_id == null) {
					unique_id = this.GetType().ToString() + "_" + Guid.NewGuid().ToString();
					Debug.Log(unique_id);
				}
				return unique_id;
			}
		}

		public abstract bool IsFinite { get; }

		public virtual void UpdateBullet (Projectile bullet, float dt) {
		}

		public virtual void OnCollision(Projectile bullet, Collider2D other) {
		}

		public virtual void OnControllerAdd(Projectile bullet) {
		}

		public virtual void OnControllerRemove(Projectile bullet) {
		}

		public virtual bool CheckDone(Projectile bullet) {
			return false;
		}

		protected void AddUniversalKey<T>(Projectile bullet, string key, T value, bool overrideOld) {
			if(overrideOld || !bullet.HasProperty<T>(key)) {
				bullet.SetProperty<T>(key, value);
			}
		}

		protected string UniqueKey(string key) {
			return "*&#" + ID + "_" + key;
		}
	}
}