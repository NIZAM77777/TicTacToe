using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayAnimation()
    {
        animator.SetTrigger("Click");
    }

  
}