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
            var parentTerrain = voxelScene.GetParentTerrain();
            if (voxelScene.GetTopScene() == null && GUILayout.Button("Add Scene to Top"))
            {
                parentTerrain.AddScene(voxelScene.WorldLocation + new Vector2(0, 1));
            }

            if (voxelScene.GetLeftScene() == null && GUILayout.Button("Add Scene to Left"))
            {
                parentTerrain.AddScene(voxelScene.WorldLocation + new Vector2(-1, 0));
            }

            if (voxelScene.GetBottomScene() == null && GUILayout.Button("Add Scene to Bottom"))
            {
                parentTerrain.AddScene(voxelScene.WorldLocation + new Vector2(0, -1));
            }

            if (voxelScene.GetRightScene() == null && GUILayout.Button("Add Scene to Right"))
            {
                parentTerrain.AddScene(voxelScene.WorldLocation + new Vector2(1, 0));
            }
        }
    }
}
