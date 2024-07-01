using UnityEngine;

public class Leprechaun : Character
{
    [SerializeField]
    private float hoverSpeed = 2f; // Speed of bobbing

    [SerializeField ]
    private float hoverHeight = 0.05f; // Height of bobbing

    [SerializeField]
    public float teleportDelay = 10f; // Configurable delay

    [SerializeField]
    public float teleportDistance = 1f; // Configurable distance

    [SerializeField]
    public int requiredTouches = 10; // Number of touches to catch

    [SerializeField]
    public float touchDistance = 0.5f; // Configurable touch distance

    [SerializeField]
    private GameObject magicOrb; // Reference to the magic orb object

    public int touchCounter = 0; // Counter for touches

    private AudioSource _audio;

    [SerializeField]
    private AudioClip teleportSound; // Sound effect for teleporting

    [SerializeField]
    private AudioClip touchedSound; // Sound effect for being touched

    protected override void OnAwake()
    {
        _audio = GetComponent<AudioSource>();
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        GameManager.RegisterDialogueCallback("leprechaun_36", Hover);
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.CatchLeprechaun)
        {
            _stateMachine.SetState(new WizardState());
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    public void Hover()
    {
        _stateMachine.SetState(new HoveringState());
    }

    public GameObject GetMagicOrb()
    {
        return magicOrb;
    }   

    public void BobUpAndDown()
    {
        Vector3 position = transform.position;
        position.y += Mathf.Sin(Time.time * hoverSpeed) * hoverHeight * Time.deltaTime;
        transform.position = position;
    }

    public void PlayTeleportSound()
    {
        _audio.PlayOneShot(teleportSound);
    }

    public void PlayTouchedSound() {
        _audio.PlayOneShot(touchedSound);
    }
}
