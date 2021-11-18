using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject linePrefab;
    public GameObject reletiveTarget;

    // Line Private Logic and Data
    private GameObject currentLine;
    private LineRenderer lineRenderer;
    private LineRenderer[] horizontalLines = new LineRenderer [1000];
    private LineRenderer[] verticalLines = new LineRenderer[1000];
    private Vector3[,] gridPoints = new Vector3 [1000, 1000];

    [Header("Creating Settings")]
    public int width = 1000;
    public int height = 1000;

    void Start()
    {
        if (reletiveTarget == null)
            reletiveTarget = GameObject.Find("Player");
        Debug.Log("CUrrent Grid Targeted to " + reletiveTarget.name);

        CreateLine();
    }

    private void Update()
    {
        UpdateGrid();
        UpdateLine();
    }

    private void CreateLine()
    {
        for(int i = 0; i < width; i++)
            for(int j = 0; j < height; j++)
                gridPoints[i, j] = new Vector3(i - width / 2.0f, 0f, j - width / 2.0f);

        for(int i = 0; i < width; i++)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 1;
            for(int j = 0; j < height; j++)
                lineRenderer.SetPosition(lineRenderer.positionCount++ - 1, gridPoints[i, j]);
            lineRenderer.positionCount--;
            verticalLines[i] = lineRenderer;
        }

        for (int i = 0; i < height; i++)
        {
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 1;
            for (int j = 0; j < width; j++)
                lineRenderer.SetPosition(lineRenderer.positionCount++ - 1, gridPoints[j, i]);
            lineRenderer.positionCount--;
            horizontalLines[i] = lineRenderer;
        }
    }

    private void UpdateLine()
    {
        for (int i = 0; i < width; i++)
        {
            lineRenderer = verticalLines[i];
            for (int j = 0; j < height; j++)
                lineRenderer.SetPosition(j, gridPoints[i, j]);
        }

        for (int i = 0; i < height; i++)
        {
            lineRenderer = horizontalLines[i];
            for (int j = 0; j < width; j++)
                lineRenderer.SetPosition(j, gridPoints[j, i]);
        }
    }

    private void UpdateGrid()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                gridPoints[i, j] = EvaluatePoint(new Vector3(i - width / 2.0f, 0f, j - height / 2.0f));
    }
    
    // If given an original point then returns a reletive point vector
    private Vector3 EvaluatePoint(Vector3 point)
    {
        Vector3 reletiveVector = point - reletiveTarget.transform.position;
        reletiveVector.y = 0;

        Vector3 playerSpeed = GameManager.gameManager.playerSpeed;
        Vector3 verticalVector = Vector3.Dot(reletiveVector, playerSpeed) / playerSpeed.magnitude * playerSpeed.normalized;
        Vector3 horizontalVector = reletiveVector - verticalVector;

        return reletiveTarget.transform.position + horizontalVector + verticalVector /
                EvaluateGamma(playerSpeed.magnitude, GameManager.gameManager.worldLS);
    }

    float EvaluateGamma(float objectSpeed, float lightSpeed)
    {
        float gamma;


        if (objectSpeed > lightSpeed)
            gamma = float.MaxValue;
        else
        {
            gamma = 1 / Mathf.Sqrt(1 - Mathf.Pow(objectSpeed / lightSpeed, 2f));
            gamma = Mathf.Max(gamma, 1.0f);
        }

        return gamma;
    }
}
