using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoxelStack)), CanEditMultipleObjects]
public class VoxelStacklEditor : Editor
{
    private int _selectedVoxelPrefabIndex;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Raise"))
        {
            foreach(var curTarget in targets)
            {
                var voxelStack = curTarget as VoxelStack;
                var voxelPrefab = GetVoxelPrefab(_selectedVoxelPrefabIndex);
                voxelStack.Raise(voxelPrefab.Key);
            }
        }

        if (GUILayout.Button("Lower"))
        {
            foreach (var curTarget in targets)
            {
                var voxelStack = curTarget as VoxelStack;
                voxelStack.Lower();
            }
        }

        var firstVoxelStack = targets.First() as VoxelStack;
        if(firstVoxelStack != null && firstVoxelStack.transform.parent != null)
        {
            var terrain = firstVoxelStack.transform.parent.transform.parent.GetComponent<VoxelTerrain>(); 
            _selectedVoxelPrefabIndex = EditorGUILayout.Popup(
                "Voxel Prefab",
                _selectedVoxelPrefabIndex,
                terrain.VoxelPrefabs.Select(x => x.Key).ToArray());
        }
    }

    private VoxelPrefab GetVoxelPrefab(int index)
    {
        var firstVoxelStack = targets.First() as VoxelStack;
        var terrain = firstVoxelStack.transform.parent.transform.parent.GetComponent<VoxelTerrain>();
        return terrain.VoxelPrefabs[index];
    }
}
