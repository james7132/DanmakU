// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections;
using UnityUtilLib;

namespace DanmakU.Phantasmagoria {
	public class PhantasmagoriaEnemy : BasicEnemy {

		[SerializeField]
		public GameObject onDeathSpawn;

		[SerializeField]
		private float specialBoost = 0.05f;

		protected override void OnDeath () {
			Field.SpawnGameObject (onDeathSpawn, transform.position, DanmakuField.CoordinateSystem.World);
			(Field.player as PhantasmagoriaPlayableCharacter).CurrentChargeCapacity += specialBoost;
		}
	}
}