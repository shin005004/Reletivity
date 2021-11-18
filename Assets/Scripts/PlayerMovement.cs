using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // not going to use rigidbody physics cause it's jank


    // Reletivity logic
    // we wont have rotational speed
    public float speedSize = 0f;

    // Movement logic
    [Header("Physics")]
    public float xSpeed = 0f;
    public float zSpeed = 0f;
    private Vector3 moveDelta;

    public float accelerationRate = 1.0f;

    protected virtual void Start()
    {
        xSpeed = 0.01f;
    }

    private void Update()
    {
        accelerationRate = GameManager.gameManager.worldLS / 3f;
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x, 0, z));
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // Reset MoveDelta
        xSpeed += input.x * accelerationRate * Time.deltaTime;
        zSpeed += input.z * accelerationRate * Time.deltaTime;

        moveDelta = new Vector3(xSpeed * Time.deltaTime, 0, zSpeed * Time.deltaTime);

        // Player's speed should not overrun lightspeed
        speedSize = Mathf.Sqrt(xSpeed * xSpeed + zSpeed * zSpeed);

        float worldLightSpeed = GameManager.gameManager.worldLS;

        if (speedSize > worldLightSpeed)
        {
            xSpeed = worldLightSpeed * moveDelta.normalized.x;
            zSpeed = worldLightSpeed * moveDelta.normalized.z;

            moveDelta = new Vector3(xSpeed * Time.deltaTime, 0, zSpeed * Time.deltaTime);
        }
        if(speedSize != 0f)
            transform.rotation = Quaternion.LookRotation(moveDelta.normalized);
        //transform.Translate(moveDelta.magnitude * transform.right);
        transform.position += transform.forward * Time.deltaTime * speedSize;
    }
}
