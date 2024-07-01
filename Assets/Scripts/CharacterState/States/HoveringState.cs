using UnityEngine;

public class HoveringState : LeprechaunState
{
    public override void Enter()
    {
        _leprechaun.Detach();
        _leprechaun.transform.position = Camera.main.transform.position + Camera.main.transform.forward * .5f;
        _leprechaun.transform.parent = Camera.main.transform;
        _leprechaun.GetMagicOrb().SetActive(true);
    }

    public override void Update()
    {
        _leprechaun.BobUpAndDown();
        FaceCamera();
    }
}
