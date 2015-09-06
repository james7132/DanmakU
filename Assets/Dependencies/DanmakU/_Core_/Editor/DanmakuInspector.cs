using UnityEngine;
using UnityEditor;
using Hourai.Editor;

namespace Hourai.DanmakU.Editor
{
    [InitializeOnLoad]
    public class DanmakuInspector : EditorWindow
    {
        const string selectorDisplayKey = "showDanmakuSelectors";

        static bool showSelectors;
        static Danmaku selectedDanmaku;

        static DanmakuInspector()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;

            if(EditorPrefs.HasKey(selectorDisplayKey))
                showSelectors = EditorPrefs.GetBool(selectorDisplayKey);
            else
                EditorPrefs.SetBool(selectorDisplayKey, showSelectors = true);
        }

        [MenuItem("Hourai/DanmakU/Danmaku Inspector")]
        public static void CreateWindow()
        {
            EditorWindow.GetWindow<DanmakuInspector>("Danmaku");
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            if(!EditorApplication.isPlaying || !showSelectors)
                return;
            Vector3 forward = Vector3.forward;
            using (HandleUtil.With(Color.green)) {                
                foreach (Danmaku danmaku in Danmaku.All) {
                    if(danmaku == null || !danmaku.IsActive)
                        continue;
                    selectedDanmaku = danmaku;
                    Handles.matrix = Matrix4x4.TRS(danmaku.Position, Quaternion.Euler(0f, 0f, danmaku.Rotation), danmaku.Size * Vector3.one);
                    float colliderSize = danmaku.Prefab.ColliderSize.Max();
                    Handles.DrawWireDisc(danmaku.Prefab.ColliderOffset, forward, colliderSize);
                    Handles.DrawLine(Vector3.zero, colliderSize * Vector3.right);
                }
            }
        }

        void OnEnable()
        {
            EditorApplication.update += Repaint;
        }

        void OnDisable()
        {
            EditorApplication.update -= Repaint;
        }

        void OnGUI()
        {
            if (!EditorApplication.isPlaying)
                NotPlaying();
            DanmakuDisplay();
        }

        void DanmakuDisplay()
        {
            if (selectedDanmaku == null)
                return;

            EditorGUILayout.LabelField("Danmaku Properties");
            EditorGUI.indentLevel++;
            selectedDanmaku.Position = EditorGUILayout.Vector2Field("Position", selectedDanmaku.Position);
            float rotation = (selectedDanmaku.Rotation % 360f + 360f) % 360f - 180f;
            selectedDanmaku.Rotation = EditorGUILayout.Slider("Rotation", rotation, -180f, 180f);
            selectedDanmaku.Size = EditorGUILayout.FloatField("Size", selectedDanmaku.Size);
            selectedDanmaku.Color = EditorGUILayout.ColorField("Color", selectedDanmaku.Color);
            selectedDanmaku.Speed = EditorGUILayout.FloatField("Speed", selectedDanmaku.Speed);
            selectedDanmaku.AngularSpeed = EditorGUILayout.FloatField("Angular Speed", selectedDanmaku.AngularSpeed);
            
            EditorGUILayout.LabelField("Frames", selectedDanmaku.Frames.ToString());
            EditorGUILayout.LabelField("Time", selectedDanmaku.Time.ToString());
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Prefab Properties");
            EditorGUI.indentLevel++;
            DanmakuPrefab prefab = selectedDanmaku.Prefab;
            DanmakuType type = selectedDanmaku.Type;
            EditorGUILayout.ObjectField("Prefab", prefab, typeof(DanmakuPrefab));
            type.Size = EditorGUILayout.FloatField("Size", type.Size);
            type.Color = EditorGUILayout.ColorField("Color", type.Color);
            EditorGUILayout.LabelField("Tag", prefab.tag);
            EditorGUILayout.LabelField("Layer", LayerMask.LayerToName(selectedDanmaku.Prefab.gameObject.layer));
        }

        void NotPlaying()
        {
        }

        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}
