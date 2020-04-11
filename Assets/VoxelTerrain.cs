using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class VoxelTerrain : MonoBehaviour
{
    [SerializeField] int SceneWidth = 10;
    [SerializeField] int SceneDepth = 10;
    public VoxelPrefab[] VoxelPrefabs;

    private List<GameObject> _voxelStackParents = new List<GameObject>();

    private void Start()
    {
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
        SceneView.beforeSceneGui += OnScene;
    }

    void OnScene(SceneView scene)
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
                        Selection.SetActiveObjectWithContext(parent.gameObject, null);
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
        foreach (Transform child in transform)
        {
            Debug.Log($"Destroying '{child.name}'.");
            child.gameObject.transform.parent = null;
            GameObject.DestroyImmediate(child.gameObject);
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
}
