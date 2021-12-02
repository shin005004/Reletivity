using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject cameraTarget;
    //public GameObject cameraObject;

    [SerializeField] [Range(0f, 1f)] public float bias = 0.960f;

    private void Start()
    {
        if (cameraTarget == null)
        {
            Debug.LogWarning("cameraTarget is not set");
        }
        Debug.Log("Camera up and running");
    }

    private void LateUpdate()
    {
        // 이런식으로 카메라를 움직이면 카메라가 플레이어의 속도를 반영된 것처럼 느껴지는 관성의 작용이 얼추 보이는 카메라 워크를 보일 수 있다.
        Vector3 moveCamTo = cameraTarget.transform.position + cameraTarget.transform.up * 10.0f;

        // spring method
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        //Camera.main.transform.LookAt(cameraTarget.transform.position);

    }
}
