using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.XR.ARFoundation;

public class CharacterSpawner : TrackedImageHandlerBase
{
    [SerializeField]
    private List<CharacterMapping> characterMappings;

    private readonly Dictionary<string, CharacterRig> characterRigInstances = new();

    private AudioSource _audio;

    [SerializeField]
    private VisualEffect _poofVFX;

    [SerializeField]
    private AudioClip _poofSound;

    [SerializeField]
    private Button resetButton;

    private CharacterRig currentCharacterRig;
    private ARTrackedImage currentTrackedImage;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        PopulateCharacterInstances();
        _poofVFX.enabled = false;
        resetButton.gameObject.SetActive(false);
        resetButton.onClick.AddListener(OnResetButtonClicked);
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

    public override IEnumerable<string> GetKeys()
    {
        foreach (var mapping in characterMappings)
        {
            yield return mapping.imageName;
        }
    }

    public override void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (
            !characterRigInstances.TryGetValue(
                trackedImage.referenceImage.name,
                out CharacterRig characterRig
            )
        )
        {
            // If the tracked image does not correspond to a character, do nothing
            return;
        }

        if (!ImageIsCloseToCamera(trackedImage))
        {
            resetButton.gameObject.SetActive(false);
            return;
        }

        resetButton.gameObject.SetActive(true);
        currentCharacterRig = characterRig;
        currentTrackedImage = trackedImage;

        if (!characterRig.gameObject.activeSelf && !characterRig.CharacterDetached())
        {
            characterRig.gameObject.SetActive(true);
            SnapRigToImage(trackedImage, characterRig);
            Poof(characterRig.transform.position);
        }
    }

    private void SnapRigToImage(ARTrackedImage trackedImage, CharacterRig characterRig)
    {
        characterRig.transform.position = trackedImage.transform.position;
        characterRig.transform.rotation =
            trackedImage.transform.rotation * Quaternion.Euler(-90, 0, 180);
    }

    public void Poof(Vector3 where)
    {
        _poofVFX.enabled = true;
        _poofVFX.transform.position = where;
        _audio.PlayOneShot(_poofSound);
        _poofVFX.Play();
    }

    private void OnResetButtonClicked()
    {
        if (currentCharacterRig != null && currentTrackedImage != null)
        {
            SnapRigToImage(currentTrackedImage, currentCharacterRig);
        }
    }

    public void ToggleOffResetButton() => resetButton.gameObject.SetActive(false);
}

[System.Serializable]
public class CharacterMapping
{
    public string imageName;
    public GameObject characterRigPrefab;
}
