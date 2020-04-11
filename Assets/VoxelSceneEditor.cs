using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoxelScene)), CanEditMultipleObjects]
public class VoxelSceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Flatten"))
        {
            foreach (var curTarget in targets)
            {
                var voxelScene = curTarget as VoxelScene;
                voxelScene.Flatten();
            }
        }

        if (GUILayout.Button("Save"))
        {
            var voxelScenne = target as VoxelScene;
            voxelScenne.Save();
            return;
        }
    }
}
