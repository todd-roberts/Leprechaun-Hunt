using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Leprechaun : Character
{
    [SerializeField]
    private float hoverSpeed = 2f; // Speed of bobbing

    [SerializeField]
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

    [SerializeField]
    private AudioClip rainbowVisionGrantedSound; 

    protected override void OnAwake()
    {
        _audio = GetComponent<AudioSource>();
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        GameManager.RegisterDialogueCallback("leprechaun_36", Hover);
        GameManager.RegisterDialogueCallback("leprechaun_45", GrantRainbowVision);
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.CatchLeprechaun)
        {
            _stateMachine.SetState(new TeleportingState());
        }
        else if (state == GameState.RainbowVisionGranted)
        {
            Goodbye();
        }
    }

    public void Goodbye()
    {
        FindObjectOfType<CharacterSpawner>().Poof(transform.position);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    public IEnumerator Hover()
    {
        _stateMachine.SetState(new HoveringState());
        yield return null;
    }

    public IEnumerator GrantRainbowVision()
    {
        GameObject rainbowVision = GameManager.GetRainbowVisionPanel();

        if (rainbowVision != null)
        {
            rainbowVision.SetActive(true);

            Image rainbowVisionImage = rainbowVision.GetComponent<Image>();

            // Fade in the rainbow vision
            yield return StartCoroutine(FadeImage(rainbowVisionImage, 0f, 1f, 1f));

            // Play the sound effect for granting rainbow vision
            _audio.PlayOneShot(rainbowVisionGrantedSound);

            // Hold the panel visible for a moment
            yield return new WaitForSeconds(2.5f);

            // Fade out the rainbow vision
            yield return StartCoroutine(FadeImage(rainbowVisionImage, 1f, 0f, 1f));

            // Hide the panel after fading out
            rainbowVision.SetActive(false);
        }
        else
        {
            Debug.LogError("RainbowVision panel is not assigned in the GameManager.");
        }

        yield return null;
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = image.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            image.color = color;
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
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

    public void PlayTouchedSound()
    {
        _audio.PlayOneShot(touchedSound);
    }

      public void MoveToRelativePosition(Vector3 relativePosition)
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0; // Keep the right direction horizontal
        cameraRight.Normalize();

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Keep the forward direction horizontal
        cameraForward.Normalize();

        Vector3 newPosition = Camera.main.transform.position +
                              (cameraRight * relativePosition.x + cameraForward * relativePosition.z) * teleportDistance;

        newPosition.y = Camera.main.transform.position.y;

        transform.position = newPosition;
    }

    public bool IsPlayerClose()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        return distance <= touchDistance;
    }

    public void SetTalking(bool isTalking)
    {
        if (_animator != null)
        {
            float weight = isTalking ? 1.0f : 0.0f;
            _animator.SetLayerWeight(1, weight);
        }
    }
}
