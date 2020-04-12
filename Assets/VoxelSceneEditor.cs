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

        if(targets != null && targets.Length == 1)
        {
            var voxelScene = target as VoxelScene;
            if (voxelScene.GetTopScene() == null && GUILayout.Button("Add Scene to Top"))
            {

            }

            if (voxelScene.GetLeftScene() == null && GUILayout.Button("Add Scene to Left"))
            {

            }

            if (voxelScene.GetBottomScene() == null && GUILayout.Button("Add Scene to Bottom"))
            {

            }

            if (voxelScene.GetRightScene() == null && GUILayout.Button("Add Scene to Right"))
            {

            }
        }
    }
}
