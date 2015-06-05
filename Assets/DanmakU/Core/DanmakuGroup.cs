// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DanmakU {

	public abstract class DanmakuGroup : ICollection<Danmaku> {

		protected ICollection<Danmaku> Group;

		public event DanmakuEvent OnAdd;

		public event DanmakuEvent OnRemove;

		protected virtual void OnDanmakuDeactivate(Danmaku danmaku) {
			Remove (danmaku);
		}

		protected void RaiseAddEvent (Danmaku target) {
			if (target != null) {
				target.groups.Add (this);
				target.OnDeactivate += OnDanmakuDeactivate;

				if (OnAdd != null)
					OnAdd (target);
			}
		}

		protected void RaiseAddEvent (IEnumerable<Danmaku> targets) {
			if(targets == null)
				throw new ArgumentNullException();

			var colList = targets as IList<Danmaku>;
			if(colList != null) {
				int count = colList.Count;
				for(int i = 0; i < count; i++) {
					Danmaku target = colList[i];
					if (target != null) {
						target.groups.Add (this);
						if (OnAdd != null)
							OnAdd (target);
					}
				}
			} else {
				foreach(var target in targets) {
					if (target != null) {
						target.groups.Add (this);
						if (OnAdd != null)
							OnAdd (target);
					}
				}
			}
		}

		protected void RaiseRemoveEvent(Danmaku target) {
			if (target != null) {
				target.groups.Remove (this);
				if (OnRemove != null)
					OnRemove (target);
			}
		}

		protected void RaiseRemoveEvent (IEnumerable<Danmaku> targets) {
			if(targets == null)
				throw new ArgumentNullException();
			
			var colList = targets as IList<Danmaku>;
			if(colList != null) {
				int count = colList.Count;
				for(int i = 0; i < count; i++) {
					Danmaku target = colList[i];
					if (target != null) {
						target.groups.Remove (this);
						if (OnRemove != null)
							OnRemove (target);
					}
				}
			} else {
				foreach(var target in targets) {
					if (target != null) {
						target.groups.Remove (this);
						if (OnRemove != null)
							OnRemove (target);
					}
				}
			}
		}

		public bool TrueForAll(Predicate<Danmaku> match) {
			if(match == null)
				throw new ArgumentNullException();

			var colList = Group as IList<Danmaku>;
			if (colList != null) {
				for(int i = 0; i < colList.Count; i++) {
					if(!match(colList[i]))
						return false;
				}
			} else {
				foreach (var danmaku in Group) {
					if(!match(danmaku))
						return false;
				}
			}
			return true;
		}

		public void AddRange (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new ArgumentNullException();

			if(Group is List<Danmaku>) {
				(Group as List<Danmaku>).AddRange(collection);
			} else if(Group is HashSet<Danmaku>) {
				(Group as HashSet<Danmaku>).UnionWith(collection);
			} else if (collection is IList<Danmaku>) {
				var colList = (IList<Danmaku>)collection;
				for(int i = 0; i < colList.Count; i++)
					Group.Add(colList[i]);
			} else {
				foreach (var danmaku in collection)
					Group.Add(danmaku);
			}
			RaiseAddEvent(collection);
		}

		public int RemoveRange (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new ArgumentNullException();

			int oldCount = Group.Count;
			if(Group is HashSet<Danmaku>) {
				(Group as HashSet<Danmaku>).ExceptWith(collection);
			} else if (collection is IList<Danmaku>) {
				var colList = (IList<Danmaku>)collection;
				for(int i = 0; i < colList.Count; i++)
					Group.Remove(colList[i]);
			} else {
				foreach (var danmaku in collection)
					Group.Remove(danmaku);
			}
			RaiseRemoveEvent(collection);
			return oldCount - Group.Count;
		}

		public bool ContainsAll (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new ArgumentNullException();

			var colList = collection as IList<Danmaku>;
			if (colList != null) {
				for(int i = 0; i < colList.Count; i++) {
					if(!Group.Contains(colList[i]))
						return false;
				}
			} else {
				foreach (var danmaku in collection) {
					if(!Group.Contains(danmaku))
						return false;
				}
			}
			return true;
		}

		public bool ContainsAny (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new ArgumentNullException();

			var colList = collection as IList<Danmaku>;
			if (colList != null) {
				for(int i = 0; i < colList.Count; i++) {
					if(Contains(colList[i]))
						return true;
				}
			} else {
				foreach (var danmaku in collection) {
					if(Contains(danmaku))
						return true;
				}
			}
			return true;
		}

		public bool Exists (Predicate<Danmaku> match) {
			return (Find (match) != null);
		}

		public Danmaku Find (Predicate<Danmaku> match) {
			var colList = Group as IList<Danmaku>;
			if (colList != null) {
				int count = colList.Count;
				for(int i = 0; i < count; i++) {
					if(match(colList[i]))
						return colList[i];
				}
			} else {
				foreach (var danmaku in Group) {
					if(match(danmaku))
						return danmaku;
				}
			}
			return null;
		}

		public List<Danmaku> FindAll(Predicate<Danmaku> match) {
			var matches = new List<Danmaku>();
			var colList = Group as IList<Danmaku>;
			if (colList != null) {
				int count = colList.Count;
				for(int i = 0; i < count; i++) {
					if(match(colList[i]))
						matches.Add (colList[i]);
				}
			} else {
				foreach (var danmaku in Group) {
					if(match(danmaku))
						matches.Add (danmaku);
				}
			}
			return matches;
		}

		public void RemoveAll (Predicate<Danmaku> match) {
			RemoveRange(FindAll(match));
		}

		public Danmaku[] ToArray() {
			var array = new Danmaku[Count];
			CopyTo (array, 0);
			return array;
		}

		#region ICollection implementation

		public void Add (Danmaku item) {
			Group.Add (item);
			RaiseAddEvent (item);
		}

		public void Clear () {
			var groupList = Group as IList<Danmaku>;
			Group.Clear ();
			if (groupList != null) {
				for (int i = 0; i < groupList.Count; i++) {
					RaiseRemoveEvent (groupList[i]);
				}
			} else {
				foreach(var danmaku in Group) {
					RaiseRemoveEvent (danmaku);
				}
			}
		}

		public bool Contains (Danmaku item) {
			return Group.Contains (item);
		}

		public void CopyTo (Danmaku[] array, int arrayIndex) {
			Group.CopyTo (array, arrayIndex);
		}

		public bool Remove (Danmaku item) {
			bool success = Group.Remove (item);
			if (success) {
				RaiseRemoveEvent(item);
			}
			return success;
		}

		public int Count {
			get {
				return Group.Count;
			}
		}

		public bool IsReadOnly {
			get {
				return Group.IsReadOnly;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<Danmaku> GetEnumerator () {
			return Group.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator () {
			return Group.GetEnumerator ();
		}

		#endregion
	
		public override int GetHashCode () {
			return Group.GetHashCode();
		}

		public override bool Equals (object obj) {
			return Group.Equals(obj);
		}
	}

	public class DanmakuGroup<T> : DanmakuGroup where T : ICollection<Danmaku>, new() {
		
		public DanmakuGroup() : base() {
			Group = new T();
		}
		
		public DanmakuGroup(IEnumerable<Danmaku> danmakus) : base () {
			Group = new T();
			AddRange (danmakus);
		}

	}
	
	public class DanmakuList : DanmakuGroup<List<Danmaku>>, IList<Danmaku> {

		//TODO Document
		
		private List<Danmaku> danmakuList;

		public int Capacity {
			get {
				return danmakuList.Capacity;
			}
		}

		public DanmakuList() : base() {
			danmakuList = Group as List<Danmaku>;
		}
		
		public DanmakuList(IEnumerable<Danmaku> danmakus) : base (danmakus) {
			danmakuList = Group as List<Danmaku>;
		}

		public int FindIndex (Predicate<Danmaku> match) {
			return danmakuList.FindIndex(match);
		}

		public int FindIndex (int startIndex, Predicate<Danmaku> match) {
			return danmakuList.FindIndex(startIndex, match);
		}

		public int FindIndex (int startIndex, int count, Predicate<Danmaku> match) {
			return danmakuList.FindIndex(startIndex, count, match);
		}

		public int IndexOf (Danmaku item, int start) {
			return danmakuList.IndexOf(item, start);
		}

		public int IndexOf (Danmaku item, int start, int end) {
			return danmakuList.IndexOf(item, start, end);
		}

		public int LastIndexOf (Danmaku item) {
			return danmakuList.LastIndexOf(item);
		}

		public int LastIndexOf (Danmaku item, int start) {
			return danmakuList.LastIndexOf(item, start);
		}
		
		public int LastIndexOf (Danmaku item, int start, int end) {
			return danmakuList.LastIndexOf(item, start, end);
		}
		
		public Danmaku FindLast (Predicate<Danmaku> match) {
			return danmakuList.FindLast(match);
		}

		public int FindLastIndex (Predicate<Danmaku> match) {
			return FindLastIndex (0, Count, match);
		}

		public int FindLastIndex (int startIndex, Predicate<Danmaku> match) {
			return danmakuList.FindLastIndex(startIndex, match);
		}

		public int FindLastIndex (int startIndex, int count, Predicate<Danmaku> match) {
			return danmakuList.FindLastIndex(startIndex, count, match);
		}

		public void InsertRange(int index, IEnumerable<Danmaku> collection) {
			danmakuList.InsertRange(index, collection);
			var colList = collection as IList<Danmaku>;
			if(colList != null) {
				int count =colList.Count;
				for(int i = 0; i < count; i++) {
					RaiseAddEvent(colList[i]);
				}
			} else {
				foreach(var danmaku in collection) {
					RaiseAddEvent(danmaku);
				}
			}
		}

		public void Reverse() {
			danmakuList.Reverse();
		}

		public void Reverse (int start, int end) {
			danmakuList.Reverse (start, end);
		}

		public void TrimExcess() {
			danmakuList.TrimExcess();
		}

		#region IList implementation

		public int IndexOf (Danmaku item) {
			return danmakuList.IndexOf (item);
		}

		public void Insert (int index, Danmaku item) {
			danmakuList.Insert (index, item);
			RaiseAddEvent (item);
		}

		public void RemoveAt (int index) {
			Danmaku target = danmakuList [index];
			danmakuList.RemoveAt (index);
			RaiseRemoveEvent (target);
		}

		public Danmaku this [int index] {
			get {
				return danmakuList[index];
			}
			set {
				Danmaku current = danmakuList[index];
				if(current == value)
					return;
				RaiseRemoveEvent(current);
				RaiseAddEvent(value);
				danmakuList[index] = value;
			}
		}

		#endregion
	}

	public class DanmakuSet : DanmakuGroup<HashSet<Danmaku>> {

		//TODO Document

		private HashSet<Danmaku> danmakuSet;

		public DanmakuSet() : base() {
			danmakuSet = Group as HashSet<Danmaku>;
		}
		
		public DanmakuSet(IEnumerable<Danmaku> danmakus) : base (danmakus) {
			danmakuSet = Group as HashSet<Danmaku>;
		}

		public void UnionWith (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new System.ArgumentNullException();
			
			var otherAsCollection = collection as ICollection<Danmaku>;
			if(otherAsCollection != null && otherAsCollection.Count <= 0)
				return;

			AddRange(collection);
		}

		public void ExceptWith (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new System.ArgumentNullException();
			
			if(danmakuSet.Count == 0)
				return;
			
			var otherAsCollection = collection as ICollection<Danmaku>;
			if(otherAsCollection != null && otherAsCollection.Count <= 0)
				return;

			RemoveRange(collection);
		}

		public void IntersectWith (IEnumerable<Danmaku> collection) {
			if(collection == null)
				throw new System.ArgumentNullException();

			if(danmakuSet.Count <= 0)
				return;

			var otherAsCollection = collection as ICollection<Danmaku>;
			if(otherAsCollection != null && otherAsCollection.Count <= 0) {
				Clear();
				return;
			}

			foreach(var danmaku in danmakuSet) {
				if(!collection.Contains(danmaku))
					RaiseRemoveEvent(danmaku); 
			}
			danmakuSet.IntersectWith(collection);
		}
	}
	
}

