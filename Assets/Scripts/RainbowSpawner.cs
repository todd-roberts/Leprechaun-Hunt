using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class RainbowScanner : TrackedImageHandlerBase
{
    [SerializeField]
    private GameObject rainbowPrefab;

    private AudioSource _audio;
    [SerializeField]
    private AudioClip spawnSound;

    private GameObject rainbowInstance;

    private bool placed = false;

    public override IEnumerable<string> GetKeys()
    {
        yield return "rainbow"; 
    }

    private void Awake() {
        _audio = GetComponent<AudioSource>();
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
        if (ImageIsCloseToCamera(trackedImage) && !placed)
        {
            if (rainbowInstance == null)
            {
                _audio.PlayOneShot(spawnSound);
                
                rainbowInstance = Instantiate(
                    rainbowPrefab,
                    trackedImage.transform.position,
                    trackedImage.transform.rotation
                );
            }
            else
            {
                rainbowInstance.transform.position = trackedImage.transform.position;
                rainbowInstance.transform.rotation = trackedImage.transform.rotation;
            }

            rainbowInstance.SetActive(true);
        }
        else if (rainbowInstance != null)
        {
            placed = true;
        }
    }
}
