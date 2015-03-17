using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace Danmaku2D.Phantasmagoria {
	public class PhantasmagoriaPlayableCharacter : DanmakuPlayer {

		[SerializeField]
		private AttackPattern[] attackPatterns;

		[SerializeField]
		private Vector2 shotOffset;

		[SerializeField]
		private float shotVelocity;

		[SerializeField]
		private float chargeRate = 1.0f;

		[SerializeField]
		private float chargeCapacityRegen;

		[SerializeField]
		private float currentChargeCapacity;

		[SerializeField]
		private ProjectilePrefab shotType;

		[SerializeField]
		private int shotDamage = 5;

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
				return attackPatterns.Length + 1; 
			}
		}

		public float CurrentChargeCapacity {
			get {
				return currentChargeCapacity;
			}
		}

		[SerializeField]
		private BulletCancelArea cancelPrefab;

		[SerializeField]
		private float deathCancelDuration;

		[SerializeField]
		private float deathCancelRadius;

		[SerializeField]
		private FrameCounter deathInvincibiiltyPeriod;

		[SerializeField]
		private FrameCounter invincibiltyFlash;

		private bool invincible;

		public override void Hit(Projectile proj) {
			if(!invincible) {
				base.Hit (proj);
				BulletCancelArea cancelArea = (BulletCancelArea)Instantiate (cancelPrefab, transform.position, Quaternion.identity);
				cancelArea.Run(deathCancelDuration, deathCancelRadius);
				invincible = true;
				StartCoroutine(DeathInvincibiilty());
			}
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
					Debug.Log("Null AttackPattern triggered. Make Sure all AttackPatterns are fully implemented");
				}
			}
			chargeLevel -= level;
			currentChargeCapacity -= level;
		}

		public override void Initialize (PlayerAgent agent) {
			base.Initialize (agent);
			for(int i = 0; i < attackPatterns.Length; i++)
				if(attackPatterns[i] != null)
					attackPatterns[i].TargetField = Field.TargetField;
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

		public override void Fire () {
			Vector2 location;
			location = transform.position;
			Projectile proj1 = Field.FireLinearProjectile (shotType, location + shotOffset, 0f, shotVelocity, DanmakuField.CoordinateSystem.World).Projectile;
			Projectile proj2 = Field.FireLinearProjectile (shotType, location - shotOffset, 0f, shotVelocity, DanmakuField.CoordinateSystem.World).Projectile;
			proj1.Damage = proj2.Damage = shotDamage;
		}
	}
}