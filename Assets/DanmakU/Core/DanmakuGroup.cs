// Copyright (c) 2015 James Liu
//	
// See the LISCENSE file for copying permission.

using System.Collections;
using System.Collections.Generic;

namespace DanmakU {

	public abstract class DanmakuGroup : ICollection<Danmaku> {

		protected readonly ICollection<Danmaku> Group;

		public event DanmakuEvent OnAdd;

		public event DanmakuEvent OnRemove;

		protected virtual void OnDanmakuDeactivate(Danmaku danmaku) {
			Remove (danmaku);
		}

		protected void RaiseAddEvent (Danmaku target) {
			if (target != null)
				target.groups.Add (this);
			if (OnAdd != null)
				OnAdd (target);
		}

		protected void RaiseRemoveEvent(Danmaku target) {
			if (target != null)
				target.groups.Remove (this);
			if (OnRemove != null)
				OnRemove (target);
		}

		public void AddRange (IEnumerable<Danmaku> collection) {
			IList<Danmaku> colList = collection as IList<Danmaku>;
			if (colList != null) {
				for(int i = 0; i < colList.Count; i++) {
					Add (colList[i]);
				}
			} else {
				foreach (var danmaku in collection) {
					Add (danmaku);
				}
			}
		}

		public void RemoveRange (IEnumerable<Danmaku> collection) {
			IList<Danmaku> colList = collection as IList<Danmaku>;
			if (colList != null) {
				for(int i = 0; i < colList.Count; i++) {
					Remove (colList[i]);
				}
			} else {
				foreach (var danmaku in collection) {
					Remove (danmaku);
				}
			}
		}

		public Danmaku[] ToArray() {
			Danmaku[] array = new Danmaku[Count];
			CopyTo (array, 0);
			return array;
		}

		#region ICollection implementation

		public void Add (Danmaku item) {
			Group.Add (item);
			item.OnDeactivate += OnDanmakuDeactivate;
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
	}

	public class DanmakuGroup<T> : DanmakuGroup where T : ICollection<Danmaku>, new() {

		public DanmakuGroup() : base() {
			Group = new T();
		}

		public DanmakuGroup(IEnumerable<Danmaku> danmakus) : base (danmakus) {
			Group = new T();
			AddRange (danmakus);
		}

		#region implemented abstract members of DanmakuGroup

		protected override ICollection<Danmaku> CreateGroup () {
			return new T ();
		}

		#endregion

	}
	
	public class DanmakuList : DanmakuGroup<List<Danmaku>>, IList<Danmaku> {
		
		//TODO Add additional list functions here
		//TODO Document
		
		private List<Danmaku> danmakuList;

		public DanmakuList() : base() {
			danmakuList = Group as List<Danmaku>;
		}
		
		public DanmakuList(IEnumerable<Danmaku> danmakus) : base (danmakus) {
			danmakuList = Group as List<Danmaku>;
		}
		protected override ICollection<Danmaku> CreateGroup () {
			danmakuList = base.CreateGroup () as List<Danmaku>;
			return danmakuList;
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

		//TODO Add additional HashSet functions here
		//TODO Document

		public DanmakuSet() : base() {
		}
		
		public DanmakuSet(IEnumerable<Danmaku> danmakus) : base (danmakus) {
		}

	}
	
}

