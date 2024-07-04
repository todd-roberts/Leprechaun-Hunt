using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class RainbowScanner : TrackedImageHandlerBase
{
    [SerializeField]
    private GameObject rainbowPrefab;

    [SerializeField]
    private Button resetButton;

    private AudioSource _audio;
    [SerializeField]
    private AudioClip spawnSound;

    private GameObject rainbowInstance;

    private ARTrackedImage currentTrackedImage;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        resetButton.gameObject.SetActive(false);
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    public override IEnumerable<string> GetKeys()
    {
        yield return "rainbow"; 
    }

    public override void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        bool shouldProcessRainbowImage =
            GameManager.GetGameState() == GameState.RainbowVisionGranted
            && trackedImage.referenceImage.name == "rainbow";

        if (shouldProcessRainbowImage)
        {
            ProcessRainbowImage(trackedImage);
        }
    }

    private void ProcessRainbowImage(ARTrackedImage trackedImage)
    {
        if (ImageIsCloseToCamera(trackedImage))
        {
            resetButton.gameObject.SetActive(true);
            currentTrackedImage = trackedImage;

            if (rainbowInstance == null)
            {
                _audio.PlayOneShot(spawnSound);

                rainbowInstance = Instantiate(
                    rainbowPrefab,
                    trackedImage.transform.position,
                    trackedImage.transform.rotation
                );
                rainbowInstance.SetActive(true);
            }
        }
        else
        {
            resetButton.gameObject.SetActive(false);
        }
    }

    private void OnResetButtonClicked()
    {
        if (rainbowInstance != null && currentTrackedImage != null)
        {
            _audio.PlayOneShot(spawnSound);
            rainbowInstance.transform.position = currentTrackedImage.transform.position;
            rainbowInstance.transform.rotation = currentTrackedImage.transform.rotation;
        }
    }
}
