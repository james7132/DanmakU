// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;
using Vexe.Runtime.Types;

namespace DanmakU.Modifiers {

	public class AddControllersModifier : DanmakuModifier {

		[Serialize, Show]
		private IDanmakuController[] controllers;

		private DanmakuController controllerAggregate;

		public AddControllersModifier() {
			controllers = null;
		}

		public AddControllersModifier(IDanmakuController controller) {
			controllers = null;
			controllerAggregate = controller.Update;
		}

		public AddControllersModifier(IEnumerable<IDanmakuController>  controllers) {
			this.controllers = null;

			if(controllers == null)
				return;

			var colList = controllers as IList<IDanmakuController>;
			if(colList != null) {
				for(int i = 0; i <colList.Count; i++) {
					IDanmakuController controller = colList[i];
					if(controller != null)
						controllerAggregate += controller.Update;
				}
			} else {
				foreach(var controller in controllers) {
					if(controller != null)
						controllerAggregate += controller.Update;
				}
			}
		}
		
		public AddControllersModifier (DanmakuController controller) {
			controllerAggregate = controller;
		}

		public AddControllersModifier(IEnumerable<DanmakuController>  controllers) {
			this.controllers = null;
			
			if(controllers == null)
				return;
			
			var colList = controllers as IList<DanmakuController>;
			if(colList != null) {
				for(int i = 0; i <colList.Count; i++)
					controllerAggregate += colList[i];
			} else {
				foreach(var controller in controllers)
					controllerAggregate += controller;
			}
		}

		public void AddController(IDanmakuController controller) {
			controllerAggregate += controller.Update;
		}

		public void RemoveController(IDanmakuController controller) {
			controllerAggregate -= controller.Update;
		}

		public void AddController(DanmakuController controller) {
			controllerAggregate += controller;
		}

		public void RemoveController(DanmakuController controller) {
			controllerAggregate -= controller;
		}

		public void ClearControllers() {
			controllerAggregate = null;
			controllers = new IDanmakuController[0];
		}

		#region implemented abstract members of DanmakuModifier

		public override void OnFire (Vector2 position, DynamicFloat rotation) {

			DanmakuController temp = controllerAggregate;

			if(controllers != null) {
				for(int i = 0; i < controllers.Length; i++) {
					temp += controllers[i].Update;
				}
			}

			Controller += temp;
			FireSingle (position, rotation);
			Controller -= temp;
		}

		#endregion



		
	}

}
