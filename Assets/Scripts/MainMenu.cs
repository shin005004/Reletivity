using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject modalWindow;

    public void Load3D()
    {
        SceneManager.LoadScene(2);
    }
    public void Load2D()
    {
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        modalWindow.SetActive(false);
    }
    public void ModalWindow()
    {
        modalWindow.SetActive(true);
    }
    public void ModalWindowDeActivate()
    {
        modalWindow.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
