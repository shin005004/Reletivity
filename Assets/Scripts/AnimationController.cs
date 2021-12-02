using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    // 플레이어 모델의 애니메이션을 담당함
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    void Update()
    {
        float playerSpeed = GameManager.gameManager.playerSpeed.magnitude;
        float lightSpeed = GameManager.gameManager.worldLS;

        // 움직이고 있다면 애니메이션을 작동함
        if (playerSpeed > 0.01f)
        {
            // 움직이는 속도에 따라서 걷는가 아니면 뛰는가를 조절하는데
            animator.SetBool("isWalking", true);
            if (playerSpeed > lightSpeed * 0.1f)
            {
                // 만약 뛰고있다면 빛의 속도에 근접할수록 배율이 증가해서 더욱 빨라진다
                // 그냥 더 쉽게 알아볼 수 있도록 넣은 것
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
