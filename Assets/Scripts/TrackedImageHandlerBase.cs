using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public abstract class TrackedImageHandlerBase : MonoBehaviour
{
    [SerializeField]
    private float scanningRange = .5f;

    public float ScanningRange
    {
        get => scanningRange;
        set => scanningRange = value;
    }

    public abstract IEnumerable<string> GetKeys();
    public abstract void HandleTrackedImage(ARTrackedImage trackedImage);

    protected bool ImageIsCloseToCamera(ARTrackedImage trackedImage)
    {
        float distance = Vector3.Distance(trackedImage.transform.position, Camera.main.transform.position);
        return distance <= scanningRange;
    }
}
