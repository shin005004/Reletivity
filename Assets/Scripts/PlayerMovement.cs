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

    public bool isStop = false;
    public float accelerationRate = 1.0f;

    protected virtual void Start()
    {
        xSpeed = 0.01f;
    }

    private void Update()
    {
        accelerationRate = 2.0f;
    }

    private void LateUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x, 0, z));
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (isStop)
                isStop = false;
            else
                isStop = true;
        }
        // Reset MoveDelta
        Vector3 inputVector = new Vector3(input.x, 0, input.z);
        xSpeed += inputVector.x * accelerationRate * Time.deltaTime;
        zSpeed += inputVector.z * accelerationRate * Time.deltaTime;

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
        if (speedSize != 0f)
            transform.rotation = Quaternion.LookRotation(moveDelta.normalized);
        if (!isStop)
        {
            if (inputVector.x == 0)
                xSpeed += -1 * 2f * xSpeed * (float)Time.deltaTime;
            if (inputVector.z == 0)
                zSpeed += -1 * 2f * zSpeed * (float)Time.deltaTime;

            //transform.Translate(moveDelta.magnitude * transform.right);
            Vector3 deltaPosition = transform.position + transform.forward * Time.deltaTime * speedSize;
            if (deltaPosition.x > 45.0f)
                deltaPosition.x = 45.0f;
            if (deltaPosition.x < -45.0f)
                deltaPosition.x = -45.0f;
            if (deltaPosition.z > 45.0f)
                deltaPosition.z = 45.0f;
            if (deltaPosition.z < -45.0f)
                deltaPosition.z = -45.0f;
            transform.position = deltaPosition;
        }
        
    }
}
