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
        Vector3 moveCamTo = cameraTarget.transform.position + cameraTarget.transform.up * 10.0f;

        // spring method
        Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
        //Camera.main.transform.LookAt(cameraTarget.transform.position);

    }
}
