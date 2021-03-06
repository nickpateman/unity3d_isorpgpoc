﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelWorld : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    public Vector2 CurrentLocation = new Vector2(0, 0);

    private Vector2? _previousLocation;
    private VoxelTerrain _parentTerrain;

    void Start()
    {
        _previousLocation = null;
        _parentTerrain = GetComponent<VoxelTerrain>();

        if(_parentTerrain == null)
        {
            throw new System.Exception("VoxelTerrain component not found!");
        }
    }

    private void Reset()
    {
        _previousLocation = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLocation();
    }

    private void UpdateLocation()
    {
        if(!_previousLocation.HasValue || (_previousLocation != CurrentLocation))
        {
            var nextScene = _parentTerrain.GetSceneAtLocation(CurrentLocation);
            if(nextScene != null)
            {
                var worldLocationOffset = new Vector3(nextScene.WorldLocation.x * _parentTerrain.SceneWidth, 0, nextScene.WorldLocation.y * _parentTerrain.SceneDepth);
                var cameraOffset = new Vector3(0, 13f, -9.5f);
                var cameraRotation = new Vector3(49.0f, 0, 0);
                MainCamera.orthographic = true;
                MainCamera.orthographicSize = 4;
                MainCamera.transform.position = cameraOffset + worldLocationOffset;
                MainCamera.transform.rotation = Quaternion.Euler(cameraRotation);

                _previousLocation = CurrentLocation;
            }
        }
    }
}
