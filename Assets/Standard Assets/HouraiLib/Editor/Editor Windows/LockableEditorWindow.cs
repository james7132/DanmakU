using System;
using UnityEditor;
using UnityEngine;

namespace Hourai.Editor {

    /// <summary>
    /// Creates a EditorWindow that has the small padlock button at the top
    /// like the Inspector.
    /// 
    /// The state of the padlock can be accessed through the Locked property.
    /// </summary>
    public abstract class LockableEditorWindow : EditorWindow, IHasCustomMenu {

        [NonSerialized]
        private GUIStyle lockButtonStyle;

        [NonSerialized]
        private bool locked;

        public bool IsLocked {
            get { return locked; }
            set { locked = value; }
        }

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu) {
            menu.AddItem(new GUIContent("Lock"), locked, () => { locked = !locked; });
        }

        private void ShowButton(Rect position) {
            if (lockButtonStyle == null)
                lockButtonStyle = "IN LockButton";
            locked = GUI.Toggle(position, locked, GUIContent.none, lockButtonStyle);
        }

    }

}