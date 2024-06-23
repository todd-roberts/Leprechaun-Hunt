using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button nextButton; // Button to navigate to the next dialogue

    private bool isTextComplete = false;
    private bool nextClicked = false;
    private string currentDialogueText = "";
    private DialogueChoice[] currentChoices = null;
    private bool choicesPresent = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        nextButton.gameObject.SetActive(false);
    }

    public void UpdateDialogueText(string speakerName, string text, AudioClip audioClip, DialogueChoice[] choices)
    {
        speakerNameText.text = speakerName; // Set the speaker name here
        currentDialogueText = text;
        choicesPresent = choices != null && choices.Length > 0;
        currentChoices = choices;

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        float typingSpeed = audioClip != null ? Mathf.Max(text.Length / audioClip.length, 25f) : 25f;
        isTextComplete = false;
        nextButton.gameObject.SetActive(false);
        nextClicked = false;

        StartCoroutine(TypeOutText(text, typingSpeed, choices));
    }

    private IEnumerator TypeOutText(string text, float typingSpeed, DialogueChoice[] choices)
    {
        dialogueText.text = "";
        float delay = 1 / typingSpeed;

        foreach (char character in text)
        {
            dialogueText.text += character; 
            yield return new WaitForSeconds(delay);
        }

        isTextComplete = true;
        if (!audioSource.isPlaying)
        {
            ShowNextButton();
        }
    }

    private void ShowChoices(DialogueChoice[] choices)
    {
        if (choices == null || choices.Length == 0)
        {
            return;
        }

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = choices[i].label;
                int choiceIndex = i; // Local copy for closure
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choices[choiceIndex]));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceSelected(DialogueChoice choice)
    {
        HandleChoice(choice);
        HideChoices();
    }

    private void HandleChoice(DialogueChoice choice)
    {
        // Call HandleChoice on the static DialogueManager
        DialogueManager.HandleChoice(choice.nextDialogueKey);
    }

    private void HideChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public bool IsDialogueComplete()
    {
        return isTextComplete && !audioSource.isPlaying && !choicesPresent;
    }

    public bool IsNextClicked()
    {
        return nextClicked;
    }

    private void ShowNextButton()
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
        nextClicked = true;
        nextButton.gameObject.SetActive(false);
        DialogueManager.TriggerNextDialogue();
    }
}
