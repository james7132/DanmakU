// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DanmakU {

    public static class GameObjectExtensions {

		/// <summary>
		/// Enumerates the children of a GameObject.
		/// </summary>
		/// <remarks>
		/// The enumerated children can be filtered using <paramref name="filter"/>. A child is only included if
		/// <paramref name="filter"/> returns true. If <paramref name="filter"/> is null, all children are returned.
		/// </remarks>
		/// <exception cref="NullReferenceException">thrown if <paramref name="gameObject"/> is null</exception>
		/// <param name="go">the GameObject to enumerate the children of</param>
		/// <param name="filter">a filter for which children to return. Defaults to <c>null</c></param>
        public static IEnumerable<GameObject> Children(this GameObject gameObject, Predicate<GameObject> filter = null) {
			if(gameObject == null)
				throw new NullReferenceException();
			if(filter != null) {
				foreach(Transform tran in gameObject.transform) {
					var child = tran.gameObject;
					if(filter(child))
						yield return child;
				}
			} else {
	            foreach (Transform tran in gameObject.transform)
    	            yield return tran.gameObject;
			}
        }

    }

}