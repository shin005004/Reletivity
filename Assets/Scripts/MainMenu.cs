using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject modalWindow;

    public void Load3D()
    {
        // 3D �ε�
        SceneManager.LoadScene(2);
    }
    public void Load2D()
    {
        // 2D �ε�
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        // ���� â ��Ȱ��
        modalWindow.SetActive(false);
    }
    public void ModalWindow()
    {
        // ���� â Ȱ��
        modalWindow.SetActive(true);
    }
    public void ModalWindowDeActivate()
    {
        // ���� â ��Ȱ��ȭ
        modalWindow.SetActive(false);
    }
    public void Quit()
    {
        // ����� ����
        Application.Quit();
    }
}
