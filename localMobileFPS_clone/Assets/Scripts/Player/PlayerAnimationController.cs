using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator Animator;

    public void StartWalkingAnimation(bool isWalking)
    {
        Animator.SetBool("IsWalking", isWalking);
       // Animator.speed = speed;
    }

    public void IdleAnimation()
    {
        Animator.SetBool("IsWalking", false);
        Animator.SetBool("IsRunning", false);

    }
    public void StartRunningAnimation(bool isRunning)
    {
        Animator.SetBool("IsRunning", isRunning);
    }


}
