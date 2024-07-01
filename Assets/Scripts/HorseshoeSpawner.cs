using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HorseshoeSpawner : TrackedImageHandlerBase
{
    [SerializeField]
    private GameObject horseshoePrefab;

    [SerializeField]
    private AudioClip successTone;

    private AudioSource _audioSource;
    private GameObject horseshoeInstance;

    private bool placed = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (horseshoePrefab == null)
        {
            Debug.LogError("Horseshoe Prefab is not assigned in the inspector.");
        }

        if (successTone == null)
        {
            Debug.LogError("Success Tone is not assigned in the inspector.");
        }
    }

    public override IEnumerable<string> GetKeys()
    {
        yield return "horseshoe"; // Assuming the reference image name for the horseshoe is "horseshoe"
    }

    public override void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == "horseshoe")
        {
            if (ImageIsCloseToCamera(trackedImage))
            {
                if (!placed)
                {
                    horseshoeInstance = Instantiate(horseshoePrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    placed = true;
                    GameManager.SetGameState(GameState.HorseshoeFound);
                    PlaySuccessTone();
                    StartCoroutine(DestroyAfterDelay());
                }
                else
                {
                    horseshoeInstance.transform.position = trackedImage.transform.position;
                    horseshoeInstance.transform.rotation = trackedImage.transform.rotation;
                }
            }
        }
    }

    private void PlaySuccessTone()
    {
        _audioSource.PlayOneShot(successTone);
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        if (horseshoeInstance != null)
        {
            Destroy(horseshoeInstance);
        }
        Destroy(gameObject);
    }
}
