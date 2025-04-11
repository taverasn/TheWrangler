using UnityEngine;

public class AnimationHandler_Base : MonoBehaviour
{
    [field: SerializeField] public Animator animator { get; private set; }
    [SerializeField] private float lerpSpeed = 5f;

    public void SetSpeed(float speed)
    { 
        float currentValue = speed;
        if(speed == 0)
        {
            currentValue = Mathf.Lerp(animator.GetFloat("Speed"), speed, Time.deltaTime * lerpSpeed);
        }
        animator.SetFloat("Speed", currentValue);
    }
    
    public void SetIsDead(bool isDead)
    {
        animator.SetBool("isDead", isDead);
        if(isDead )
        {
            PlayTargetAnimation("Death");
        }
    }

    public void TriggerPunch()
    {
        animator.SetTrigger("Punch");
    }
    
    public void TriggerHit()
    {
        PlayTargetAnimation("Hit");
    }

    public void PlayTargetAnimation(string animation)
    {
        animator.CrossFade(animation, 0.2f);
    }
}
