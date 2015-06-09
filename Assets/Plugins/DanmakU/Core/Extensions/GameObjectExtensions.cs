// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Collections.Generic;

namespace DanmakU
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns a List<> of the object's children.
        /// </summary>
        /// <param name="go">GameObject</param>
        /// <returns>The object's children.</returns>
        public static List<GameObject> GetChildrenList(this GameObject go)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform tran in go.transform)
            {
                children.Add(tran.gameObject);
            }
            return children;
        }

        public static GameObject[] GetChildren(this GameObject go)
        {
            return go.GetChildrenList().ToArray();
        }

        public static IEnumerable<GameObject> Children(this GameObject go)
        {
            foreach (Transform tran in go.transform)
                yield return tran.gameObject;
        }

        public static GameObject FindChild(this GameObject go, string childName)
        {
            GameObject[] children = go.GetChildren();
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i].name == childName)
                {
                    return children[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Safe get component method. Is a defensive alternative to the GetComponent method.
        /// This will tell you when it does not find the component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <param name="obj">Game Object</param>
        /// <returns>Returns the component or null.</returns>
        public static T SafeGetComponent<T>(this GameObject obj) where T : MonoBehaviour
        {
            T component = obj.GetComponent<T>();
            if (component == null)
                Debug.LogError("Expected to find component of type " + typeof (T) + " but found none!", obj);
            return component;
        }
    }
}