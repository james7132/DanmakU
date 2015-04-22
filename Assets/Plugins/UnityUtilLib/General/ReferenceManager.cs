// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReferenceManager : MonoBehaviour {

	void Awake() {
		Destroy (gameObject);
	}

	#if UNITY_EDITOR
	[System.Serializable]
	public class Node {
		public Object objectRef;
		public Rect editorRect;
		public bool expanded;
		public SerializedObject serializedObject;

		[System.NonSerialized]
		private Edge[] children;

		[SerializeField]
		private string displayName;
		public string DisplayName {
			get {
				return displayName;
			}
		}

		public Node(Object source) {
			objectRef = source;
			RecheckName();
			if(objectRef != null)
				serializedObject = new SerializedObject(objectRef);
		}

		public void RecheckName() {
			displayName = objectRef.name + " (" + Regex.Replace(objectRef.GetType().Name + ")", @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0").Trim();
		}

		public static bool operator ==(Node n1, Node n2) {
			if ((object)n1 == null || (object)n2 == null)
				return false;
			return n1.objectRef == n2.objectRef;
		}

		public static bool operator != (Node n1, Node n2) {
			return !(n1 == n2);
		}

		public override bool Equals (object obj) {
			if (obj is Node)
				return this == (obj as Node);
			if (obj is Object)
				return objectRef == obj;
			return false;
		}

		public override int GetHashCode () {
			return objectRef.GetHashCode ();
		}
	}

	[System.Serializable]
	public class Edge {
		public Object origin;
		public Object reference;
		public string serializedPropertyPath;

		public Edge(Object origin, SerializedProperty property) {
			this.origin = origin;
			this.reference = property.objectReferenceValue;
			this.serializedPropertyPath = property.propertyPath; 
		}

		public static bool operator ==(Edge e1, Edge e2) {
			return e1.origin == e2.origin && e1.reference == e2.reference;
		}

		public static bool operator !=(Edge e1, Edge e2) {
			return !(e1 == e2);
		}

		public override bool Equals (object obj) {
			if (obj is Edge)
				return this == (obj as Edge);
			return false;
		}

		public override int GetHashCode () {
			return 23 * origin.GetHashCode () + 17 * reference.GetHashCode ();
		}
	}

	[SerializeField]
	public List<Node> nodes;

	[SerializeField]
	public List<Edge> edges;

	private void Setup() {
		if (nodes == null)
			nodes = new List<Node> ();
		if (edges == null)
			edges = new List<Edge> ();
	}

	public void Initialize(Object source) {
		Setup ();
		InitializeHelper (source);
		Prune ();
	}

	public void Initialize(Object[] sources) {
		Setup ();
		for (int i = 0; i < sources.Length; i++) {
			InitializeHelper(sources[i]);
		}
		Prune ();
	}

	private Node InitializeHelper(Object source) {
		if (source == this || source == transform)
			return null;
		Setup ();
		Node sourceNode = new Node (source);
		if (!nodes.Contains (sourceNode))
			nodes.Add (sourceNode);
		else
			nodes [nodes.IndexOf (sourceNode)].RecheckName ();
		if (sourceNode.serializedObject != null) {
			SerializedProperty iterator = sourceNode.serializedObject.GetIterator ();
			while(iterator.NextVisible(true)) {
				if(iterator.propertyType == SerializedPropertyType.ObjectReference && iterator.objectReferenceValue != null) {
					if(iterator.objectReferenceValue is MonoScript)
						continue;
					Debug.Log(sourceNode.DisplayName + iterator.objectReferenceValue.name);
					Node newNode = InitializeHelper(iterator.objectReferenceValue);
					if(newNode != null) {
						Edge testEdge = new Edge(source, iterator);
						if(!edges.Contains(testEdge))
							edges.Add(testEdge);
					}
				}
			}
		}
		return sourceNode;
	}

	public List<Node> GetRoots() {
		var temp = new List<Node> (nodes);
		int index = 0;
		while (index < temp.Count) {
			Object target = temp[index].objectRef;
			bool used = false;
			foreach(Edge edge in edges) {
				if(edge.origin == target) {
					used = true;
					break;
				}
			}
			if(used) {
				temp.RemoveAt(index);
			} else {
				index++;
			}
		}
		return temp;
	}

	public void Prune() {
		Setup ();
		nodes.RemoveAll(n => n.objectRef == null);
		edges.RemoveAll (e => e.origin == null || e.reference == null);
	}
	#endif
}
