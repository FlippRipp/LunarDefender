using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitDrawer : MonoBehaviour
{
    [SerializeField] int vertexCount = 100;

    private LineRenderer lineRenderer;
    private PlayerInput playerInput;

    private Vector3[] positions;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        UpdatePoints();
        UpdateLineRenderer();
    }

    private void UpdatePoints()
    {
        if (playerInput == null || playerInput.PlayerMovement == null) return;

        positions = new Vector3[vertexCount];
        for (int i = 0; i < vertexCount; i++)
        {
            float angle = (360 / vertexCount) * i;
            positions[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}
