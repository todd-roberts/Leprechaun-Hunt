using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private TMP_Text speakerNameText;

    [SerializeField]
    private Button nextButton; // Button to navigate to the next dialogue

    [SerializeField]
    private Button choiceButton1;

    [SerializeField]
    private Button choiceButton2;

    [SerializeField]
    private Image dialogueImage; // Add this line

    private bool _isTextComplete;

    private void Awake()
    {
        HideButtons();
        dialogueImage.gameObject.SetActive(false); // Ensure the image is hidden initially
    }

    private void HideButtons()
    {
        HideNextButton();
        HideChoices();
    }

    public void UpdateDialogueText(string speakerName, DialogueEntry dialogueEntry)
    {
        HideButtons();
        dialogueImage.gameObject.SetActive(false); // Hide the image initially

        speakerNameText.text = speakerName;

        if (dialogueEntry.dialogueImage != null)
        {
            dialogueImage.sprite = dialogueEntry.dialogueImage;
            dialogueImage.gameObject.SetActive(true); // Show the image if it exists
        }

        StartCoroutine(TypeOutText(dialogueEntry));
    }

    private IEnumerator TypeOutText(DialogueEntry dialogue)
    {
        dialogueText.text = "";

        int lengthOfTextWithoutSpaces = dialogue.text.Replace(" ", "").Length;

        string[] words = dialogue.text.Split(' ');

        foreach (string word in words)
        {
            string newText = dialogueText.text + word + " ";
            dialogueText.text = newText;

            float ratio = word.Length / (float)lengthOfTextWithoutSpaces;

            yield return new WaitForSeconds(ratio * dialogue.audioClip.length);
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

    public void ShowChoices(DialogueEntry dialogue)
    {
        ShowChoice(dialogue.choice1, choiceButton1);

        if (dialogue.HasChoice2())
        {
            ShowChoice(dialogue.choice2, choiceButton2);
        }
    }

    private void ShowChoice(DialogueChoice choice, Button button)
    {
        button.gameObject.SetActive(true);
        button.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = choice.label;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnChoiceSelected(choice.nextDialogueKey));
    }

    private void OnChoiceSelected(string nextDialogueKey)
    {
        HideChoices();
        DialogueManager.HandleChoice(nextDialogueKey);
    }

    public void HideChoices()
    {
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }
}
