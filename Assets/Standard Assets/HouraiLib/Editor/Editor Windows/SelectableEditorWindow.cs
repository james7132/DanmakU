using System;
using System.Collections.Generic;
using System.Linq;
using Hourai.Editor;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Hourai {

    /// <summary>
    /// An EditorWindow that is based on the current selection.
    /// 
    /// The selection can be locked via the padlock in the top right of the window.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SelectableEditorWindow<T> : LockableEditorWindow where T : Object {

        private T[] _selection;

        /// <summary>
        /// A list of selected objects.
        /// </summary>
        protected IEnumerable<T> Selection {
            get {
                if (_selection == null)
                    return Enumerable.Empty<T>();
                return _selection.Where(selected => Filter == null || Filter(selected));
            }
        }

        /// <summary>
        /// Provides a filter on the selection's objects.
        /// </summary>
        protected SelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Provides a filter for what kinds of selected objects are used.
        /// 
        /// If null, the selection is unfiltered.
        /// </summary>
        protected Predicate<T> Filter { get; set; }

        /// <summary>
        /// Updates the Selection as needed.
        /// </summary>
        protected virtual void OnSelectionChange() {
            if (IsLocked)
                return;

            _selection =
                UnityEditor.Selection.GetFiltered(typeof (T), SelectionMode).Select(selected => selected as T).ToArray();
        }

    }

}