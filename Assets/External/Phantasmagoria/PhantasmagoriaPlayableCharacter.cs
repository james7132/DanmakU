using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaPlayableCharacter : DanmakuPlayer {

		public Sprite characterPortrait;

		[SerializeField]
		private AttackPattern[] attackPatterns;

		[SerializeField]
		private float chargeRate = 1.0f;

		[SerializeField]
		private float chargeCapacityRegen;

		[SerializeField]
		private float currentChargeCapacity;

		[SerializeField]
		private float cancelDurationPerChargeLevel = 0.5f;

		[SerializeField]
		private float cancelRadiusPerChargeLevel = 5;

		private bool charging;
		public bool IsCharging {
			get { 
				return charging; 
			}
			set {
				if(charging && !value) {
					SpecialAttack(Mathf.FloorToInt(CurrentChargeLevel));
				}
				charging = value;
			}
		}

		public override bool IsFiring {
			get {
				return base.IsFiring && !IsCharging;
			}
		}
		
		private float chargeLevel = 0f;
		public float CurrentChargeLevel {
			get { 
				return chargeLevel; 
			}
		}

		public int MaxChargeLevel {
			get { 
				return attackPatterns.Length; 
			}
		}

		public float CurrentChargeCapacity {
			get {
				return currentChargeCapacity;
			}
			set {
				currentChargeCapacity = value;
			}
		}

		[SerializeField]
		private BulletCancelArea cancelPrefab;

		[SerializeField]
		private float deathCancelDuration = 0.5f;

		[SerializeField]
		private float deathCancelRadius = 40f;

		[SerializeField]
		private FrameCounter deathInvincibiiltyPeriod;

		[SerializeField]
		private FrameCounter invincibiltyFlash;

		private bool invincible;

		public override DanmakuField Field {
			get {
				return base.Field;
			}
			set {
				DanmakuField field = base.Field = value;
				for(int i = 0; i < attackPatterns.Length; i++)
					if(attackPatterns[i] != null)
						attackPatterns[i].TargetField = field.TargetField;
			}
		}

		public override void Hit(Danmaku proj) {
			if(!invincible) {
				base.Hit (proj);
				SpawnCancel(deathCancelDuration, deathCancelRadius);
				invincible = true;
				StartCoroutine(DeathInvincibiilty());
			}
		}

		public void SpawnCancel(float duration, float radius) {
			BulletCancelArea cancel = (BulletCancelArea)Instantiate (cancelPrefab, transform.position, Quaternion.identity);
			cancel.Run(duration, radius);
		}

		private IEnumerator DeathInvincibiilty() {
			deathInvincibiiltyPeriod.Reset ();
			invincibiltyFlash.Reset ();
			WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
			SpriteRenderer render = GetComponent<SpriteRenderer> ();
			bool flash = false;
			Color normalColor = render.color;
			Color flashColor = normalColor;
			flashColor.a = 0;
			while(!deathInvincibiiltyPeriod.Tick()) {
				if(invincibiltyFlash.Tick()) {
					flash = !flash;
					render.color = (flash) ? flashColor : normalColor;
				}
				yield return wfeof;
			}
			invincible = false;
			render.color = normalColor;
		}

		public virtual void SpecialAttack(int level) {
			int index = level - 1;
			if (index >= 0 && index < attackPatterns.Length) {
				if(attackPatterns[index] != null) {
					attackPatterns[index].Fire();
				} else {
					print("Null AttackPattern triggered. Make Sure all AttackPatterns are fully implemented");
				}
			}
			SpawnCancel (level * cancelDurationPerChargeLevel, level * cancelRadiusPerChargeLevel);
			chargeLevel = 0f;
			currentChargeCapacity -= (int)level;
		}

		public override void NormalUpdate () {
			base.NormalUpdate ();
			float dt = Util.TargetDeltaTime;
			currentChargeCapacity += chargeCapacityRegen * dt;
			if(currentChargeCapacity > MaxChargeLevel) {
				currentChargeCapacity = MaxChargeLevel;
			}
			if(charging) {
				chargeLevel += chargeRate * dt;
				if(chargeLevel > currentChargeCapacity)
					chargeLevel = currentChargeCapacity;
			} else {
				FireCheck(dt);
			}
		}
	}
}