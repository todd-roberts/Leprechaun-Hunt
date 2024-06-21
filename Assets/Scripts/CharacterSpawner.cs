using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using System;
using UnityEngine.VFX;

public class CharacterSpawner : MonoBehaviour
{
    private ARTrackedImageManager _trackedImageManager;

    [SerializeField]
    private List<CharacterMapping> characterMappings;

    [SerializeField]
    private float scanningRange = .5f;

    [SerializeField]
    private float smoothingSpeed = 5f; // Speed for smoothing the position and rotation updates

    private readonly Dictionary<string, CharacterRig> characterRigInstances = new();

    private AudioSource _audio;

    [SerializeField]
    private VisualEffect _poofVFX;

    [SerializeField]
    private AudioClip _poofSound;

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _audio = GetComponent<AudioSource>();
        PopulateCharacterInstances();
        _poofVFX.enabled = false;
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
        CharacterRig characterRig = characterRigInstances[trackedImage.referenceImage.name];

        if (!characterRig.gameObject.activeSelf)
        {
            characterRig.transform.position = trackedImage.transform.position;
            characterRig.transform.rotation =
                trackedImage.transform.rotation * Quaternion.Euler(-90, 0, 180);
            if (ImageIsCloseToCamera(trackedImage)) {
                Poof(characterRig.transform.position);
                characterRig.gameObject.SetActive(true);
            }
        }
    }

    public void Poof(Vector3 where) {
        _poofVFX.enabled = true;
        _poofVFX.transform.position = where;
        _audio.PlayOneShot(_poofSound);
        _poofVFX.Play();
    }

    private bool ImageIsCloseToCamera(ARTrackedImage trackedImage)
    {
        float distance = Vector3.Distance(trackedImage.transform.position, Camera.main.transform.position);
        return distance <= scanningRange;
    }
}

[System.Serializable]
public class CharacterMapping
{
    public string imageName;
    public GameObject characterRigPrefab;
}
