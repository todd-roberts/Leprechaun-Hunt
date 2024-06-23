using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Button nextButton; // Button to navigate to the next dialogue

    private bool _isTextComplete;

    private void Awake()
    {
        nextButton.gameObject.SetActive(false);
    }

    public void UpdateDialogueText(string speakerName, DialogueEntry dialogueEntry)
    {
        speakerNameText.text = speakerName; 

        float typingSpeed = dialogueEntry.audioClip != null ? Mathf.Max(dialogueEntry.text.Length / dialogueEntry.audioClip.length, 25f) : 25f;
        _isTextComplete = false;
        nextButton.gameObject.SetActive(false);

        StartCoroutine(TypeOutText(dialogueEntry.text, typingSpeed));
    }

    private IEnumerator TypeOutText(string text, float typingSpeed)
    {
        dialogueText.text = "";
        float delay = 1 / typingSpeed;

        foreach (char character in text)
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(delay);
        }

        _isTextComplete = true;
    }

    public bool IsTextComplete() => _isTextComplete;


    public void ShowNextButton()
    {
        nextButton.gameObject.SetActive(true);
        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    public void HideNextButton()
    {
        nextButton.gameObject.SetActive(false);
    }

    private void OnNextButtonClicked()
    {
        nextButton.gameObject.SetActive(false);
        DialogueManager.PlayNextDialogue();
    }
}
