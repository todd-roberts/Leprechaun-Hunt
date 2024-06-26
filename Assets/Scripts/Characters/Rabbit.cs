using UnityEngine;

public class Rabbit : Character
{
    public GameObject leftBabyRabbit;
    public GameObject rightBabyRabbit;

    private Animator leftBabyAnimator;
    private Animator rightBabyAnimator;

    protected override void OnAwake()
    {
        leftBabyAnimator = leftBabyRabbit.GetComponent<Animator>();
        rightBabyAnimator = rightBabyRabbit.GetComponent<Animator>();
    }

    protected override void OnPlayAnimation(string animationName)
    {
        SyncBabyRabbits(animationName);
    }

    private void SyncBabyRabbits(string animationName)
    {
        if (animationName == "Walk" || animationName == "Idle")
        {
            leftBabyAnimator.Play(animationName);
            rightBabyAnimator.Play(animationName);
        }
    }
}
