using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private Transform landerStartPositionTransform;
    [SerializeField] private Transform cameraStartTargetTransform;
    [SerializeField] private float zoomedOutOrthographicSize;

    public int GetLevelNumber()
    {
        return levelNumber;
    }
    public Vector3 GetLanderStartPosition()
    {
        return landerStartPositionTransform.position;
    }
    public Transform GetCameraStartTargetTransform() 
    { 
        return cameraStartTargetTransform;
    }
    public float GetzoomedOutOrthographicSize()
    {
        return zoomedOutOrthographicSize;
    }
}
