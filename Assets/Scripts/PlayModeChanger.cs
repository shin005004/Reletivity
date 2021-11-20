using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeChanger : MonoBehaviour
{
    public GameObject player;
    public GameObject floor;
    public GameObject editor;

    private bool editMode = true;
    private float startFrame = 0.0f;
    private bool loadingCheck = true;

    void Start()
    {
        editor.SetActive(true);
        floor.SetActive(true);
    }

    void Update()
    {
        if(startFrame > 1.0f && loadingCheck)
        {
            player.SetActive(false);
            loadingCheck = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (loadingCheck)
            startFrame += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(editMode)
            {
                editMode = false;
                editor.SetActive(false);
                player.SetActive(true);
                floor.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                editMode = true;
                editor.SetActive(true);
                player.SetActive(false);
                floor.SetActive(true);
            }
        }
    }
}
