using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityUtilLib;
using UnityUtilLib.Pooling;

namespace Danmaku2D {

	[DisallowMultipleComponent]
	public sealed class ProjectileManager : Singleton<ProjectileManager>, IPausable {

		private static ProjectilePool projectilePool;

		private class ProjectilePool : IPool<Projectile> {

			internal Queue<int> pool;
			internal Projectile[] all;
			internal int totalCount;
			internal int inactiveCount;
			internal int spawnCount;

			public ProjectilePool(int initial, int spawn) {
				this.spawnCount = spawn;
				pool = new Queue<int>();
				totalCount = 0;
				inactiveCount = 0;
				Spawn (initial);
			}
			
			protected void Spawn(int count) {
				if(all == null) {
					all = new Projectile[1024];
				}
				int endCount = totalCount + spawnCount;
				if(all.Length <= endCount) {
					int arraySize = all.Length;
					while (arraySize <= endCount) {
						arraySize = Mathf.NextPowerOfTwo(arraySize + 1);
					}
					Projectile[] temp = new Projectile[arraySize];
					System.Array.Copy(all, temp, all.Length);
					all = temp;
				}
				for(int i = totalCount; i < endCount; i++) {
					all[i] = new Projectile();
					all[i].index = i;
					all[i].Pool = this;
					pool.Enqueue(i);
				}
				totalCount = endCount;
				inactiveCount += spawnCount;
			}

			#region IPool implementation

			public Projectile Get () {
				if(inactiveCount <= 0) {
					Spawn (spawnCount);
				}
				inactiveCount--;
				return all [pool.Dequeue ()];
			}

			public void Return (Projectile obj) {
				pool.Enqueue (obj.index);
				inactiveCount++;
			}

			#endregion

			#region IPool implementation

			object IPool.Get () {
				return Get ();
			}

			public void Return (object obj) {
				Return (obj as Projectile);
			}

			#endregion
		}

		[SerializeField]
		private int initialCount = 1000;

		[SerializeField]
		private int spawnOnEmpty = 100;
	
		[SerializeField]
		private LayerMask defaultCollisionMask = ((LayerMask)~0);

		public override void Awake () {
			base.Awake ();
			Projectile.SetupCollisions ();
		}

		public void Start () {
			Debug.Log (((int)defaultCollisionMask).ToString ("X8"));
			if(projectilePool == null) {
				projectilePool = new ProjectilePool (initialCount, spawnOnEmpty);
			}
		}

		public int TotalCount {
			get {
				return (projectilePool != null) ? projectilePool.totalCount : 0;
			}
		}

		public int ActiveCount {
			get {
				return (projectilePool != null) ? projectilePool.totalCount : 0;
			}
		}

		public void Update() {
			if (!Paused)
				NormalUpdate ();
		}

		public void NormalUpdate () {
			float dt = Util.TargetDeltaTime;
			int totalCount = projectilePool.totalCount;
			DanmakuField[] fields = DanmakuField.fields.ToArray ();
			int fieldCount = fields.Length;
			for (int i = 0; i < totalCount; i++) {
				Projectile proj = projectilePool.all[i];
				if(proj.is_active) {
					proj.Update(dt);
					bool check = true;
					for(int j = 0; j < fieldCount; j++) {
						if(fields[j].Bounds.Contains(proj.Position)) {
							check = false;
							break;
						}
					}
					if(check) {
						proj.DeactivateImmediate();
					}
				}
			}
		}

		public static void DeactivateAll() {
			Projectile[] all = projectilePool.all;
			int totalCount = projectilePool.totalCount;
			for (int i = 0; i < totalCount; i++) {
				if(all[i].is_active)
					all[i].DeactivateImmediate();
			}
		}

		internal static Projectile Get (ProjectilePrefab projectileType) {
			Projectile proj = projectilePool.Get ();
			proj.MatchPrefab (projectileType);
			return proj;
		}

		#region IPausable implementation

		public bool Paused {
			get;
			set;
		}

		#endregion
	}
}