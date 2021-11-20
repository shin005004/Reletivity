using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Menu
    public GameObject pauseMenuUI;
    public GameObject infoPanel;
    public TextMeshProUGUI speedOfLight;
    public TextMeshProUGUI speedOfPlayer;
    public TextMeshProUGUI gammaText;
    public bool menuKeyDown;
    #endregion

    public static GameManager gameManager;
    public PlayerMovement playerObject;

    public Vector3 playerSpeed;

    // worldLightSpeed;
    public float worldLS;
    public int speedOfLightTarget;
    public float worldTime = 0.0f;
    private float changeFactor = 0.1f;
    private float speedOfLightStep;
    private bool menuFrozen = false;

    private void Awake()
    {
        gameManager = this;
        playerSpeed = new Vector3(0.001f, 0.0001f, 0.0001f);
        speedOfLightTarget = 200;
        menuKeyDown = false;
    }

    private void Start()
    {
        if (playerObject == null)
            Debug.LogWarning("Player not found");
        Debug.Log("Gamemannager Running");
    }

    private void Update()
    {
        // CHANGE the speed of light (N, M)
        int temp2 = (int)(Input.GetAxis("Speed of Light"));
        if (temp2 < 0 && speedOfLightTarget <= 10)
        {
            temp2 = 0;
            speedOfLightTarget = (int)10;
        }
        if (temp2 != 0)
        {
            speedOfLightTarget += temp2;

            speedOfLightStep = Mathf.Abs((float)(worldLS - speedOfLightTarget) / 20);
        }

        // Smooth damp speed of light to target
        if (worldLS < speedOfLightTarget * .995)
            worldLS += speedOfLightStep;
        else if (worldLS > speedOfLightTarget * 1.005)
            worldLS -= speedOfLightStep;
        else if (worldLS != speedOfLightTarget)
            worldLS = speedOfLightTarget;



        if (Input.GetAxis("Menu Key") > 0 && !menuKeyDown)
        {
            menuKeyDown = true;
            ChangeMenuState();
        }
        else if (!(Input.GetAxis("Menu Key") > 0))
        {
            menuKeyDown = false;
        }

        playerSpeed = new Vector3(playerObject.xSpeed, 0f, playerObject.zSpeed);
        speedOfLight.text = string.Format("빛의 속도: {0:0.00}", worldLS);
        speedOfPlayer.text = string.Format("현재 속도: {0:0.00C}", playerSpeed.magnitude / worldLS);
        gammaText.text = string.Format("색변화주기: {0:0.00}초", 1 / Mathf.Sqrt(1 - playerSpeed.magnitude / worldLS * playerSpeed.magnitude / worldLS));

        if (Input.GetKey(KeyCode.M))
            worldLS += changeFactor * Time.deltaTime;
        if (Input.GetKey(KeyCode.N))
            worldLS -= changeFactor * Time.deltaTime;
        if(Input.GetKey(KeyCode.L))
            changeFactor += 1.0f * Time.deltaTime;
        if (Input.GetKey(KeyCode.K))
            changeFactor += 1.0f * Time.deltaTime;
    }

    public void ChangeMenuState()
    {
        if (menuFrozen)
        {
            menuFrozen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            pauseMenuUI.SetActive(false);
            infoPanel.SetActive(true);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().velocity = Vector3.zero;
            menuFrozen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            pauseMenuUI.SetActive(true);
            infoPanel.SetActive(false);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
