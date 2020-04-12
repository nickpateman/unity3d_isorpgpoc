using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoxelTerrain))]
public class VoxelTerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var voxelTerrain = target as VoxelTerrain;
        if (GUILayout.Button("Destroy"))
        {
            voxelTerrain.Destroy();
            return;
        }

        if (GUILayout.Button("Rebuild"))
        {
            voxelTerrain.Rebuild();
            return;
        }

        if (GUILayout.Button("Reload"))
        {
            voxelTerrain.Reload();
            return;
        }

        if (voxelTerrain.DataFile != null && GUILayout.Button("Save"))
        {
            voxelTerrain.Save();
            return;
        }

        if (GUILayout.Button("Save As..."))
        {
            voxelTerrain.SaveAs();
            return;
        }
    }


}
