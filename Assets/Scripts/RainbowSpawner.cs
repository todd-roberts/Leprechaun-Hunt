using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RainbowScanner : TrackedImageHandlerBase
{
    [SerializeField]
    private GameObject rainbowPrefab;

    private GameObject rainbowInstance;

    public override IEnumerable<string> GetKeys()
    {
        yield return "rainbow"; // Assuming the reference image name for the rainbow is "rainbow"
    }

    public override void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == "rainbow" && trackedImage.trackingState == TrackingState.Tracking)
        {
            if (ImageIsCloseToCamera(trackedImage))
            {
                if (rainbowInstance == null)
                {
                    rainbowInstance = Instantiate(rainbowPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                }
                else
                {
                    rainbowInstance.transform.position = trackedImage.transform.position;
                    rainbowInstance.transform.rotation = trackedImage.transform.rotation;
                }

                rainbowInstance.SetActive(true);
            }
        }
    }
}
