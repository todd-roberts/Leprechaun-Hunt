using UnityEngine;

public class CaughtState : LeprechaunState
{
    public override void Enter()
    {
        _leprechaun.transform.position = Camera.main.transform.position + new Vector3(0, 0, -.5f);
        _leprechaun.transform.SetParent(Camera.main.transform);
        GameManager.SetGameState(GameState.LeprechaunCaught);
        //_leprechaun.PlayAnimation("Caught");
         DialogueManager.StartDialogue(_leprechaun);
    }
}
