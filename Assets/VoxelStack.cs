using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelStack : MonoBehaviour
{
    private Stack<string> _prefabKeys = new Stack<string>();
    private Stack<GameObject> _voxels = new Stack<GameObject>();
    private int _curHeight;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (_curHeight > 0)
        {
            Gizmos.DrawWireCube(
                transform.position + new Vector3(0, 0.5f * (_curHeight - 1), 0),
                new Vector3(1, _curHeight, 1));
        }

        var sphereLocation = transform.position;
        sphereLocation += new Vector3(0, _curHeight, 0);
        Gizmos.DrawSphere(
            sphereLocation,
            0.1f);
    }

    private GameObject GetVoxelPrefab(string prefabKey)
    {
        if(transform.parent != null && transform.parent.transform.parent != null)
        {
            var terrain = transform.parent.transform.parent.GetComponent<VoxelTerrain>();
            var prefab = terrain.VoxelPrefabs.FirstOrDefault(x => x.Key == prefabKey);
            return prefab.Prefab != null ? GameObject.Instantiate(prefab.Prefab) : GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
        else
        {
            return GameObject.CreatePrimitive(PrimitiveType.Cube);
        }
    }

    public void Raise(string prefabKey)
    {
        var offsetLocation = new Vector3(
            0,
            _curHeight,
            0);
        var curVoxel = GetVoxelPrefab(prefabKey);
        curVoxel.name = "Voxel";
        curVoxel.transform.parent = transform;
        curVoxel.transform.localPosition = offsetLocation;
        curVoxel.hideFlags = HideFlags.HideInHierarchy | HideFlags.DontSave;
        _voxels.Push(curVoxel);
        _prefabKeys.Push(prefabKey);

        _curHeight += 1;
    }

    public bool Lower()
    {
        if(_curHeight > 1)
        {
            var top = _voxels.Pop();
            _prefabKeys.Pop();
            top.transform.parent = null;
            GameObject.DestroyImmediate(top);
            _curHeight -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Clear()
    {
        while(Lower())
        {
        }
        _prefabKeys.Clear();
    }

    public string ToKeyString()
    {        
        return string.Join(",", _prefabKeys.Reverse().ToArray());
    }
}
