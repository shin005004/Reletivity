using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject modalWindow;

    public void Load3D()
    {
        // 3D 로딩
        SceneManager.LoadScene(2);
    }
    public void Load2D()
    {
        // 2D 로딩
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        // 도움말 창 비활성
        modalWindow.SetActive(false);
    }
    public void ModalWindow()
    {
        // 도움말 창 활성
        modalWindow.SetActive(true);
    }
    public void ModalWindowDeActivate()
    {
        // 도움말 창 비활성화
        modalWindow.SetActive(false);
    }
    public void Quit()
    {
        // 종료시 실행
        Application.Quit();
    }
}
