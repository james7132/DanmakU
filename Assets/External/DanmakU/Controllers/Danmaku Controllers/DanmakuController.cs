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

/// <summary>
/// A development kit for quick development of 2D Danmaku games
/// </summary>
namespace DanmakU {

	public delegate void DanmakuController(Danmaku proj, float dt);

	/// <summary>
	/// An interface for defining any controller of Danmaku behavior.
	/// </summary>
	public interface IDanmakuController {

		/// <summary>
		/// Updates the Danmaku controlled by the controller instance.
		/// </summary>
		/// <returns>the displacement from the Danmaku's original position after udpating</returns>
		/// <param name="dt">the change in time since the last update</param>
		void UpdateDanmaku (Danmaku danmaku, float dt);

	}
}