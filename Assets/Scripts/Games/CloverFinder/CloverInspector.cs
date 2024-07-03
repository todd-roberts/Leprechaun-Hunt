using System.Collections;
using UnityEngine;

public class CloverInspector : MonoBehaviour
{
    public GameObject threeLeafCloverInspect;
    public GameObject fourLeafCloverInspect;

    public AudioClip failureClip;
    public float inspectDuration = 2.0f; // Duration of the inspection
    private AudioSource _audio;
    private bool isInspecting = false;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        gameObject.SetActive(false); // Ensure the inspector starts inactive
    }

    public bool IsInspecting()
    {
        return isInspecting;
    }

    public void ShowInspector(bool isFourLeaf)
    {
        if (isInspecting) return;

        isInspecting = true;
        threeLeafCloverInspect.SetActive(!isFourLeaf);
        fourLeafCloverInspect.SetActive(isFourLeaf);

        gameObject.SetActive(true);

        if (isFourLeaf)
        {
            GameManager.PlaySuccessSound();
        }
        else
        {
            PlayFailureAudio();
        }

        StartCoroutine(CloseInspectObject());
    }

    private IEnumerator CloseInspectObject()
    {
        yield return new WaitForSeconds(inspectDuration);
        gameObject.SetActive(false);
        isInspecting = false;
    }

    public void PlayFailureAudio()
    {
        _audio.PlayOneShot(failureClip);
    }
}
