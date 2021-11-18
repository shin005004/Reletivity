using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text textBox;

    public static GameManager gameManager;
    public PlayerMovement playerObject;

    public Vector3 playerSpeed;

    // worldLightSpeed;
    public float worldLS;

    public float worldTime = 0.0f;

    private float changeFactor = 0.1f;

    private void Awake()
    {
        gameManager = this;
        playerSpeed = new Vector3(0.001f, 0.0001f, 0.0001f);
    }

    private void Start()
    {
        if (playerObject == null)
            Debug.LogWarning("Player not found");
        Debug.Log("Gamemannager Running");
    }

    private void Update()
    {
        playerSpeed = new Vector3(playerObject.xSpeed, 0f, playerObject.zSpeed);
        textBox.text = worldLS.ToString();
        if (Input.GetKey(KeyCode.M))
            worldLS += changeFactor * Time.deltaTime;
        if (Input.GetKey(KeyCode.N))
            worldLS -= changeFactor * Time.deltaTime;
        if(Input.GetKey(KeyCode.L))
            changeFactor += 1.0f * Time.deltaTime;
        if (Input.GetKey(KeyCode.K))
            changeFactor += 1.0f * Time.deltaTime;
    }
}
