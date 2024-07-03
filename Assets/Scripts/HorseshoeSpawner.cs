using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HorseshoeSpawner : TrackedImageHandlerBase
{
    [SerializeField]
    private GameObject horseshoePrefab;

    private GameObject horseshoeInstance;

    private bool placed = false;

    public bool allowFastForward = false;

    public override IEnumerable<string> GetKeys()
    {
        yield return "horseshoe"; 
    }

    public override void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (!allowFastForward && GameManager.GetGameState() != GameState.FindHorseShoe)
        {
            return;
        }
        
        if (trackedImage.referenceImage.name == "horseshoe")
        {
            if (ImageIsCloseToCamera(trackedImage))
            {
                if (!placed)
                {
                    horseshoeInstance = Instantiate(horseshoePrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    placed = true;
                    GameManager.SetGameState(GameState.HorseshoeFound);
                    GameManager.PlaySuccessSound();
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
