using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoxelScene : MonoBehaviour
{
    public string Name = "New Scene";
    public Vector2 WorldLocation = new Vector2(0, 0);

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
        Name = json["Name"].Value<string>();
        var worldLocationParts = json["WorldLocation"].Value<string>().Split(',');
        WorldLocation = new Vector2(
            int.Parse(worldLocationParts[0]),
            int.Parse(worldLocationParts[1]));
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
        json.Add("WorldLocation", new JValue($"{WorldLocation.x},{WorldLocation.y}"));
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
