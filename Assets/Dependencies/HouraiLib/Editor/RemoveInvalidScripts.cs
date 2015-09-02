using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using Vexe.Runtime.Extensions;
using UnityObject = UnityEngine.Object;

namespace Hourai.Editor {

    [InitializeOnLoad]
    internal static class RemoveInvalidScripts {

        private static readonly IEnumerable<Type> AllTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                                               from assemblyType in assembly.GetTypes()
                                                               select assemblyType;

        private static readonly IEnumerable<UnityObject> EditorOnlyComponents = from type in AllTypes
                                                                                where !type.IsAbstract &&
                                                                                      type.IsA<Component>() &&
                                                                                      type.IsDefined<EditorOnly>(true)
                                                                                from obj in Resources.FindObjectsOfTypeAll(type)
                                                                                select obj;

        private static readonly IEnumerable<UnityObject> BuildOnlyComponents = from type in AllTypes
                                                                                where !type.IsAbstract &&
                                                                                      type.IsA<Component>() &&
                                                                                      type.IsDefined<BuildOnly>(true)
                                                                                from obj in Resources.FindObjectsOfTypeAll(type)
                                                                                select obj;

        [PostProcessScene]
        private static void RemoveScripts() {
            if (BuildPipeline.isBuildingPlayer)
                foreach (UnityObject obj in EditorOnlyComponents)
                    UnityObject.DestroyImmediate(obj);
            else
                foreach (UnityObject obj in BuildOnlyComponents)
                    UnityObject.DestroyImmediate(obj);
        }

    }

}
