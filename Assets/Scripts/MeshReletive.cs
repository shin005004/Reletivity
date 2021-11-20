using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshReletive : MonoBehaviour
{
    [Header("Reletive to Player")]
    public GameObject reletiveTarget;
    public Vector3 reletiveVector;
    public Vector3 reletiveSpeed;

    public bool onMeshUpdate = false;
    public bool onColorUpdate = false;

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private Vector3[] originalVerticies;

    Color startColor;
    Color stopColor;
    float colorTime;
    float playerGamma;

    private void Awake()
    {
        // Mesh logic
        mesh = GetComponent<MeshFilter>().mesh;
        originalVerticies = mesh.vertices;
        for (var i = 0; i < originalVerticies.Length; i++)
            originalVerticies[i] = Vector3.Scale(originalVerticies[i], transform.localScale) + transform.position;
        mesh.MarkDynamic();

        // color Logic
        meshRenderer = GetComponent<MeshRenderer>();
        colorTime = 0.0f;
        startColor = Color.black;
        stopColor = Color.white;

        // By default 
        if (reletiveTarget!= null)
            reletiveTarget = GameObject.Find("Player");
        if (reletiveTarget == null)
            Debug.LogWarning(gameObject.name + "'s Reletive Target Not Found");
    }

    private void Update()
    {
        meshRenderer.enabled = true;
        if (onMeshUpdate)
            UpdateMesh();
        if (onColorUpdate)
        {
            ColorUpdate();
            if(colorTime >= 1.0f)
            {
                colorTime = 0f;
                Color temp = startColor;
                startColor = stopColor;
                stopColor = temp;
            }
        }
            
    }

    private void ColorUpdate()
    {
        meshRenderer.material.color = Color.Lerp(startColor, stopColor, colorTime);
        if (colorTime < 1)
            colorTime += Time.deltaTime / 0.5f / playerGamma;
    }

    private float EvaluateGamma(float objectSpeed, float lightSpeed)
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

    private void UpdateMesh()
    {
        Vector3[] newVerticies = new Vector3[originalVerticies.Length];

        for(var i = 0; i < originalVerticies.Length; i++)
        {
            reletiveVector = originalVerticies[i] - reletiveTarget.transform.position;

            // projection of distance vector and horizontal
            Vector3 playerSpeed = GameManager.gameManager.playerSpeed;
            Vector3 verticalVector = Vector3.Dot(reletiveVector, playerSpeed) / playerSpeed.magnitude * playerSpeed.normalized;
            Vector3 horizontalVector = reletiveVector - verticalVector;

            playerGamma = EvaluateGamma(playerSpeed.magnitude, GameManager.gameManager.worldLS);
            newVerticies[i] = Vector3.Scale(reletiveTarget.transform.position + horizontalVector + verticalVector /
                EvaluateGamma(playerSpeed.magnitude, GameManager.gameManager.worldLS) - transform.position, 
                new Vector3(1.0f / transform.localScale.x, 1.0f / transform.localScale.y, 1.0f / transform.localScale.z));
        }

        mesh.vertices = newVerticies;
    }
}
