using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private float cameraMoveSpeed = 30f;

    [SerializeField] private float cameraZoomAmount = 2f;
    [SerializeField] private float minOrthographicSize = 10f;
    [SerializeField] private float maxOrthographicSize = 30f;
    [SerializeField] private float cameraZoomSpeed = 5f;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;
        transform.position += moveDir * cameraMoveSpeed * Time.deltaTime;
    }
    private void HandleZoom()
    {
        targetOrthographicSize += -Input.mouseScrollDelta.y * cameraZoomAmount;

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * cameraZoomSpeed);
        
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

}
