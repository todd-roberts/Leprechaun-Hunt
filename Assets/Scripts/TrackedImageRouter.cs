using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageRouter : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;

    [SerializeField]
    private List<TrackedImageHandlerBase> handlerObjects;

    private readonly Dictionary<string, TrackedImageHandlerBase> handlers = new();

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();

        foreach (var handlerObject in handlerObjects)
        {
            RegisterHandler(handlerObject);
        }
    }

    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    public void RegisterHandler(TrackedImageHandlerBase handler)
    {
        foreach (var key in handler.GetKeys())
        {
            handlers[key] = handler;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                RouteTrackedImage(trackedImage);
            }
        }
    }

    private void RouteTrackedImage(ARTrackedImage trackedImage)
    {
        if (handlers.TryGetValue(trackedImage.referenceImage.name, out TrackedImageHandlerBase handler))
        {
            handler.HandleTrackedImage(trackedImage);
        }
    }
}
