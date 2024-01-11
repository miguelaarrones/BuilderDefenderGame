using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private const string PLAYER_PREFS_EDGE_SCROLLING = "EdgeScrolling";

    public static CameraHandler Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private float cameraMoveSpeed = 30f;

    [SerializeField] private float cameraZoomAmount = 2f;
    [SerializeField] private float minOrthographicSize = 10f;
    [SerializeField] private float maxOrthographicSize = 30f;
    [SerializeField] private float cameraZoomSpeed = 5f;

    private float orthographicSize;
    private float targetOrthographicSize;
    private bool edgeScrolling;

    private void Awake()
    {
        Instance = this;
        edgeScrolling = PlayerPrefs.GetInt(PLAYER_PREFS_EDGE_SCROLLING, 1) == 1;
    }

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
        
        if (edgeScrolling)
        {
            float edgeScrollingSize = 30f;
            if (Input.mousePosition.x > Screen.width - edgeScrollingSize)
            {
                x = 1f;
            }
            if (Input.mousePosition.x < edgeScrollingSize)
            {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - edgeScrollingSize)
            {
                y = 1f;
            }
            if (Input.mousePosition.y < edgeScrollingSize)
            {
                y = -1f;
            }
        }

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

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt(PLAYER_PREFS_EDGE_SCROLLING, edgeScrolling ? 1 : 0);
    }

    public bool GetEdgeScrolling() => edgeScrolling;
}
