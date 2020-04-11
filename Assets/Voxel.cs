using UnityEngine;

public class Voxel : MonoBehaviour
{
    public Vector2 WorldLocation;
    public Vector3 SceneLocation;

    public override string ToString()
    {
        return $"[{WorldLocation.x},{WorldLocation.y}-{SceneLocation.x},{SceneLocation.y},{SceneLocation.z}]";
    }
}
