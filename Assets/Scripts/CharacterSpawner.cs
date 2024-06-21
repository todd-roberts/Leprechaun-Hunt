using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;

public class CharacterSpawner : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;

    [SerializeField]
    private List<CharacterMapping> characterMappings;

    [SerializeField]
    private float smoothingSpeed = 5f; // Speed for smoothing the position and rotation updates

    private readonly Dictionary<string, CharacterRig> characterRigInstances = new();
    private readonly HashSet<string> initialTransformsSet = new();

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        PopulateCharacterInstances();
    }

    private void PopulateCharacterInstances()
    {
        foreach (CharacterMapping mapping in characterMappings)
        {
            GameObject characterRigInstance = Instantiate(mapping.characterRigPrefab);
            characterRigInstance.SetActive(false);
            CharacterRig characterRig = characterRigInstance.GetComponent<CharacterRig>();
            characterRigInstances[mapping.imageName] = characterRig;
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

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                HandleTrackedImage(trackedImage);
            }
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (characterRigInstances.TryGetValue(trackedImage.referenceImage.name, out CharacterRig characterRig))
        {
            if (!characterRig.gameObject.activeSelf)
            {
                characterRig.transform.position = trackedImage.transform.position;
                characterRig.transform.rotation = trackedImage.transform.rotation * Quaternion.Euler(-90, 0, 180);
                characterRig.gameObject.SetActive(true);
                initialTransformsSet.Add(trackedImage.referenceImage.name);
            }
            else if (initialTransformsSet.Contains(trackedImage.referenceImage.name))
            {
                // Smoothly interpolate position and rotation
                characterRig.transform.position = Vector3.Lerp(characterRig.transform.position, trackedImage.transform.position, Time.deltaTime * smoothingSpeed);
                characterRig.transform.rotation = Quaternion.Slerp(characterRig.transform.rotation, trackedImage.transform.rotation * Quaternion.Euler(-90, 0, 180), Time.deltaTime * smoothingSpeed);
            }
        }
    }
}

[System.Serializable]
public class CharacterMapping
{
    public string imageName;
    public GameObject characterRigPrefab;
}
