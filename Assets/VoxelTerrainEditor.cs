using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoxelTerrain))]
public class VoxelTerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Destroy"))
        {
            var voxelTerrain = target as VoxelTerrain;
            voxelTerrain.Destroy();
            return;
        }

        if (GUILayout.Button("Rebuild"))
        {
            var voxelTerrain = target as VoxelTerrain;
            voxelTerrain.Rebuild();
            return;
        }
    }


}
