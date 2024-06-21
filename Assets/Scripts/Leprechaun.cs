using System.Collections.Generic;

public enum LeprechaunAnimation
{
    Idle,
    Wave
}

public class Leprechaun : Character {
    private readonly Dictionary<LeprechaunAnimation, string> _animationNameMap = new()
    {
        { LeprechaunAnimation.Idle, "Lep_looking_around" },
        { LeprechaunAnimation.Wave, "Lep_waving_B" }
    };

    public void PlayAnimation(LeprechaunAnimation animation)
    {
        _animator.Play(_animationNameMap[animation]);
    }


    protected override void ProcessGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Initial:
                PlayAnimation(LeprechaunAnimation.Wave);
                break;
            default:
                break;
        }
    }
}
