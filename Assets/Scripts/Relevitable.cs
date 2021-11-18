using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relevitable : MonoBehaviour
{
    [Header("Reletive to Player")]
    public GameObject reletiveBody;
    public GameObject reletiveTarget;
    public Vector3 reletiveVector;
    public Vector3 reletiveSpeed;

    public Vector3 originalPlace;
    public bool scalerTure = false;
    public bool distancerTrue = false;

    // shows the direction forward
    [Tooltip("For Debug purposes")] public bool onGizmos = false;

    private void Start()
    {
        // By default 
        reletiveTarget = GameObject.Find("Player");
        if(reletiveTarget == null)
            Debug.LogWarning(gameObject.name + "'s Reletive Target Not Found");
        originalPlace = transform.position;
    }

    private void Update()
    {
        UpdateReletive();
    }

    private void UpdateReletive()
    {
        // Look at the direction of the target object
        reletiveVector = reletiveTarget.transform.position - transform.position;
        reletiveVector.y = 0;   // y should always be 0

        float targetAngle = Vector3.SignedAngle(transform.forward, reletiveVector, transform.up);
        transform.Rotate(0f, targetAngle, 0f);
        reletiveBody.transform.Rotate(0f, -targetAngle, 0f);

        // need the size of the angle between playerspeed and playerposition vector
        float reletiveAngle = Vector3.Angle(reletiveVector, GameManager.gameManager.playerSpeed);
        float speedSize = GameManager.gameManager.playerSpeed.magnitude;
        
        // (speed horizontal, 0, speed toward the target)
        reletiveSpeed = new Vector3(speedSize * Mathf.Sin(reletiveAngle * Mathf.Deg2Rad), 0, speedSize * Mathf.Cos(reletiveAngle * Mathf.Deg2Rad));

        // scale with gamma
        // scale.z is toward, scale.x is horizontal
        // Debug.Log(reletiveAngle);

        // Length Constraction
        if(scalerTure)
            transform.localScale = new Vector3(1 / EvaluateGamma(reletiveSpeed.x, GameManager.gameManager.worldLS), 1f,
                1 / EvaluateGamma(reletiveSpeed.z, GameManager.gameManager.worldLS));
        
        if(distancerTrue)
        {
            float distanceGamma = 1 / EvaluateGamma(reletiveSpeed.z, GameManager.gameManager.worldLS);
            transform.position = originalPlace * distanceGamma + reletiveTarget.transform.position * (1 - distanceGamma);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * (reletiveTarget.transform.position - transform.position).magnitude);
    }
}
