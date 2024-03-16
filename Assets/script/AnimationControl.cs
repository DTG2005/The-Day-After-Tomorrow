using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
   Animator animator;
    int isRunningHash;
    int isKickingHash;
    int isShootingHash;
    bool isKicking; // Check if the character is currently kicking
    bool isShooting; // Check if the character is currently shooting
    bool wasRunningBeforeAction; // Check if the character was running before kicking or shooting

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isKickingHash = Animator.StringToHash("isKicking");
        isShootingHash = Animator.StringToHash("isShooting");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool forwardPressed = Input.GetKey("w");

        // Handling space bar for kicking
        if (Input.GetKeyDown(KeyCode.Space) && !isRunning && !isShooting)
        {
            isKicking = true;
            animator.SetBool(isKickingHash, true);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isKicking = false;
            animator.SetBool(isKickingHash, false);
        }

        // Handling left shift for shooting
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRunning && !isKicking)
        {
            isShooting = true;
            animator.SetBool(isShootingHash, true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isShooting = false;
            animator.SetBool(isShootingHash, false);
        }

        // Update running state based on movement input
        // Ensure the character is not in the middle of kicking or shooting action
        if (!isKicking && !isShooting)
        {
            if (!isRunning && forwardPressed)
            {
                animator.SetBool(isRunningHash, true);
            }
            else if (isRunning && !forwardPressed)
            {
                animator.SetBool(isRunningHash, false);
            }
        }
    }
}