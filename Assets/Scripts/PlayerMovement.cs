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
        accelerationRate = 2.0f;
    }

    private void LateUpdate()
    {
        // 플레이어의 입력을 받아서 이를 움직임에 적용시키기
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x, 0, z));
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        // 움직임에 입력을 적용시키는 함수
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // 만약 스페이스바를 눌렀다면 모든 움직임 제한
            // 단 속도는 변할 수 있다.
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
            // 만약 속도가 빛의 속도보다 크다면 변화값의 각도를 계산하여 크기는 일정하게 각도만 변화시킴
            xSpeed = worldLightSpeed * moveDelta.normalized.x;
            zSpeed = worldLightSpeed * moveDelta.normalized.z;

            moveDelta = new Vector3(xSpeed * Time.deltaTime, 0, zSpeed * Time.deltaTime);
        }
        if (speedSize != 0f)
            transform.rotation = Quaternion.LookRotation(moveDelta.normalized);
        if (!isStop)
        {
            // 자동멈춤을 위한 과정
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
