using UnityEngine;

public class CaughtState : LeprechaunState
{
    public override void Enter()
    {
        _leprechaun.GetMagicOrb().SetActive(false);

        _leprechaun.transform.position =
            Camera.main.transform.position
            + Camera.main.transform.forward * 0.20f
            + Camera.main.transform.up * -0.075f;

        _leprechaun.transform.SetParent(Camera.main.transform);

        GameManager.SetGameState(GameState.LeprechaunCaught);

        DialogueManager.StartDialogue(_leprechaun);
    }

}
