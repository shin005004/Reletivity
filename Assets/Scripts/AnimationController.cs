using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // �÷��̾� ���� �ִϸ��̼��� �����
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    void Update()
    {
        float playerSpeed = GameManager.gameManager.playerSpeed.magnitude;
        float lightSpeed = GameManager.gameManager.worldLS;

        // �����̰� �ִٸ� �ִϸ��̼��� �۵���
        if (playerSpeed > 0.01f)
        {
            // �����̴� �ӵ��� ���� �ȴ°� �ƴϸ� �ٴ°��� �����ϴµ�
            animator.SetBool("isWalking", true);
            if (playerSpeed > lightSpeed * 0.1f)
            {
                // ���� �ٰ��ִٸ� ���� �ӵ��� �����Ҽ��� ������ �����ؼ� ���� ��������
                // �׳� �� ���� �˾ƺ� �� �ֵ��� ���� ��
                float multiplier = 10.0f / lightSpeed * playerSpeed;
                animator.SetFloat("runMultiplier", multiplier);
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
