using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    public static CinemachineCameraZoom2D Instance { get; private set; }

    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10F;

    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.m_Lens.OrthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);
    }
    public void SetTargetOrthographicSize(float targetOrthographicSiz)
    {
        this.targetOrthographicSize = targetOrthographicSiz;
    }
    public void SetNormalOrthographicSize()
    {
        SetTargetOrthographicSize(NORMAL_ORTHOGRAPHIC_SIZE);
    }
}
