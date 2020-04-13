using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class VoxelTerrain : MonoBehaviour
{
    public int SceneWidth = 10;
    public int SceneDepth = 10;
    public VoxelPrefab[] VoxelPrefabs;
    public DefaultAsset DataFile;

    private List<GameObject> _voxelStackParents = new List<GameObject>();

    private void Start()
    {
        Destroy();
        Reload();
    }

    private void Update()
    {
        
    }

    private void Reset()
    {
        Destroy();
        Rebuild();
    }

    private void OnEnable()
    {
        SceneView.beforeSceneGui += OnBeforeSceneGui;
    }

    void OnBeforeSceneGui(SceneView scene)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Vector3 mousePos = e.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            Ray ray = scene.camera.ScreenPointToRay(mousePos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var parent = hit.transform.parent;
                if (parent != null)
                {
                    var voxelStack = parent.transform.GetComponent<VoxelStack>();
                    if (voxelStack != null)
                    {
                        if(e.shift)
                        {
                            var prevSelection = Selection.gameObjects.ToList();
                            prevSelection.Add(parent.gameObject);
                            Selection.objects = prevSelection.ToArray();
                        }
                        else
                        {
                            Selection.activeObject = parent.gameObject;
                            //Selection.SetActiveObjectWithContext(parent.gameObject, null);
                        }
                        e.Use();
                    }
                }
            }
        }
    }

    private GameObject CreateScene(
        string name,
        Vector3 offset)
    {
        var scene = new GameObject(name);
        scene.hideFlags = HideFlags.DontSave;
        for(int curX = 0; curX < SceneWidth; curX++)
        {
            for(int curZ = 0; curZ < SceneDepth; curZ++)
            {
                var location = new Vector3(
                    curX,
                    0,
                    curZ);
                var offsetLocation = location + offset;
                var curVoxelStackParent = new GameObject();
                curVoxelStackParent.transform.parent = scene.transform;
                curVoxelStackParent.transform.localPosition = offsetLocation;
                curVoxelStackParent.name = $"[x={location.x},z={location.z}]";
                var curVoxelStack = curVoxelStackParent.AddComponent<VoxelStack>();
                curVoxelStack.Raise(string.Empty);
                _voxelStackParents.Add(curVoxelStackParent);
            }
        }

        return scene;
    }

    public void Destroy()
    {
        var destroy = new List<Transform>();
        foreach (Transform child in transform)
        {
            destroy.Add(child);
        }
        foreach(var curDestroy in destroy)
        {
            Debug.Log($"Destroying '{curDestroy.name}'.");
            GameObject.DestroyImmediate(curDestroy.gameObject);
        }

        _voxelStackParents.Clear();
    }

    public void Rebuild()
    {
        var scene = CreateScene(
            "default",
            new Vector3(-4.5f, 0.5f, -4.5f));
        var voxelScene = scene.AddComponent<VoxelScene>();
        scene.transform.parent = transform;
    }

    public void AddScene(Vector2 worldLocation)
    {
        var scene = CreateScene(
            "New Scene",
            new Vector3(-4.5f, 0.5f, -4.5f));
        var voxelScene = scene.AddComponent<VoxelScene>();
        voxelScene.WorldLocation = worldLocation;
        scene.transform.parent = transform;

        var worldLocationOffset = new Vector3(voxelScene.WorldLocation.x * SceneWidth, 0, voxelScene.WorldLocation.y * SceneDepth);
        scene.transform.position = worldLocationOffset;
    }

    public void Reload()
    {
        string assetPath = AssetDatabase.GetAssetPath(DataFile.GetInstanceID());
        var jsonData = System.IO.File.ReadAllText(assetPath);
        var json = JObject.Parse(jsonData);
        Destroy();

        var scenesJson = json["Scenes"].Value<JArray>();
        foreach(var curSceneJson in scenesJson)
        {
            var scene = CreateScene(
                curSceneJson["Name"].Value<string>(),
                new Vector3(-4.5f, 0.5f, -4.5f));
            var voxelScene = scene.AddComponent<VoxelScene>();
            scene.transform.parent = transform;
            voxelScene.Load((JObject)curSceneJson);

            var worldLocationOffset = new Vector3(voxelScene.WorldLocation.x * SceneWidth, 0, voxelScene.WorldLocation.y * SceneDepth);
            scene.transform.position = worldLocationOffset;
        }
    }

    public void Save()
    {
        if(DataFile != null)
        {
            var json = new JObject();
            var scenes = new JArray();
            var voxelScenes = GetComponentsInChildren<VoxelScene>();
            foreach (var curVoxelScene in voxelScenes)
            {
                scenes.Add(curVoxelScene.Save());
            }
            json.Add("Scenes", scenes);

            string assetPath = AssetDatabase.GetAssetPath(DataFile.GetInstanceID());
            System.IO.File.WriteAllText(assetPath, json.ToString(Newtonsoft.Json.Formatting.Indented));
        }     
    }

    public void SaveAs()
    {
        var json = new JObject();
        var scenes = new JArray();
        var voxelScenes = GetComponentsInChildren<VoxelScene>();
        foreach(var curVoxelScene in voxelScenes)
        {
            scenes.Add(curVoxelScene.Save());
        }
        json.Add("Scenes", scenes);

        var path = EditorUtility.SaveFilePanel(
            "Save world data",
            "",
            "data.json",
            "json");

        if (path.Length != 0)
        {
            System.IO.File.WriteAllText(
                path,
                json.ToString(Newtonsoft.Json.Formatting.Indented));
        }
    }

    public VoxelScene GetSceneAtLocation(Vector2 worldLocation)
    {
        var voxelScenes = GetComponentsInChildren<VoxelScene>();
        return voxelScenes.SingleOrDefault(x => x.WorldLocation == worldLocation);
    }

}
