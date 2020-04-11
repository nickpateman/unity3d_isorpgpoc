using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VoxelScene : MonoBehaviour
{
    [SerializeField] string Name;

    private void Reset()
    {
        Flatten();
    }

    public void Flatten()
    {
        var voxelStacks = GetComponentsInChildren<VoxelStack>();
        foreach(var curVoxelStack in voxelStacks)
        {
            curVoxelStack.Clear();
        }
    }

    public void Save()
    {
        JObject json = new JObject();
        json.Add("Name", new JValue(Name));
        var stacks = new JArray();
        var voxelStacks = GetComponentsInChildren<VoxelStack>();
        foreach(var curVoxelStack in voxelStacks)
        {
            var stackData = $"({curVoxelStack.transform.name})-[{curVoxelStack.ToKeyString()}]";
            stacks.Add(new JValue(stackData));
        }

        json.Add("Stacks", stacks);
        var jsonString = json.ToString(Newtonsoft.Json.Formatting.Indented, null);
    }
}
