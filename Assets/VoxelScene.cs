using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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

    public void Load(JObject json)
    {
        var stacksJson = json["Stacks"].Value<JArray>();
        var stacksList = JsonConvert.DeserializeObject<List<string>>(stacksJson.ToString());
        var voxelStacks = GetComponentsInChildren<VoxelStack>().ToList();
        foreach (var curVoxelStack in voxelStacks)
        {
            curVoxelStack.Clear();
            var stack = stacksList.Single(x => x.StartsWith($"({curVoxelStack.transform.name})"));
            var stackKeys = stack.Split('-')[1].Trim(new char[] { '[', ']' }).Split(',');
            foreach(var curKey in stackKeys)
            {
                if(!string.IsNullOrEmpty(curKey))
                {
                    curVoxelStack.Raise(curKey);
                }
            }
        }
    }

    public JObject Save()
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
        return json;
    }
}
