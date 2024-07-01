using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.VFX;

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

    private void Awake()
    {
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

    public override IEnumerable<string> GetKeys()
    {
        foreach (var mapping in characterMappings)
        {
            yield return mapping.imageName;
        }
    }

    public override void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (!characterRigInstances.TryGetValue(trackedImage.referenceImage.name, out CharacterRig characterRig))
        {
            // If the tracked image does not correspond to a character, do nothing
            return;
        }

        if (!characterRig.gameObject.activeSelf && !characterRig.CharacterDetached())
        {
            characterRig.transform.position = trackedImage.transform.position;
            characterRig.transform.rotation =
                trackedImage.transform.rotation * Quaternion.Euler(-90, 0, 180);
            if (ImageIsCloseToCamera(trackedImage))
            {
                Poof(characterRig.transform.position);
                characterRig.gameObject.SetActive(true);
            }
        }
    }

    public void Poof(Vector3 where)
    {
        _poofVFX.enabled = true;
        _poofVFX.transform.position = where;
        _audio.PlayOneShot(_poofSound);
        _poofVFX.Play();
    }
}

[System.Serializable]
public class CharacterMapping
{
    public string imageName;
    public GameObject characterRigPrefab;
}
