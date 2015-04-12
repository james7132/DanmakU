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

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace Danmaku2D.DanmakuControllers.Wrapper {

	/// <summary>
	/// An abstract generic superclass to mirror the functionality of any implementor of IDanmakuController in a DanmakuControlBehavior.
	/// It can be used with other DanmakuControlBehavior implementations; however it is best used with IDanmakuController implementations that don't derive from DanmakuControlBehavior.
	/// </summary>
	public abstract class ControllerWrapperBehavior<T> : DanmakuControlBehavior where T : IDanmakuController, new() {

		[SerializeField]
		private T danmakuController;

		/// <summary>
		/// Gets the underlying DanmakuController.
		/// </summary>
		/// <value>The underlying controller.</value>
		public sealed override DanmakuController Controller {
			get {
				return danmakuController.UpdateDanmaku;
			}
		}

		public override void Awake () {
			if (danmakuController == null) {
				danmakuController = new T();
			}
		}

		#region implemented abstract members of DanmakuControlBehavior
		public sealed override void UpdateDanmaku (Danmaku danmaku, float dt) {
			danmakuController.UpdateDanmaku(danmaku, dt);
		}
		#endregion

		public override string NodeName {
			get {
				return typeof(T).Name;
			}
		}

	}
}