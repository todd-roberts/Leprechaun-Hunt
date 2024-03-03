using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] choiceButtons;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private DialogueManager dialogueManager;

    public float baseTypingSpeed = 25f;

    private bool isTextComplete = false;
    private string currentDialogueText = "";
    private DialogueChoice[] currentChoices = null;
    private bool choicesPresent = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateDialogueText(string text, AudioClip audioClip, DialogueChoice[] choices)
    {
        currentDialogueText = text;
        choicesPresent = choices != null && choices.Length > 0;
        currentChoices = choices;


        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        float typingSpeed = audioClip != null ? Mathf.Max(text.Length / audioClip.length, baseTypingSpeed) : baseTypingSpeed;
        isTextComplete = false;

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
        ShowChoices(choices);
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
        dialogueManager.StartDialogueForScene(choice.nextDialogueKey);
    }

    private void HideChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void OnPanelClicked()
    {
        if (!isTextComplete)
        {
            StopAllCoroutines();
            dialogueText.text = currentDialogueText;
            isTextComplete = true;
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            ShowChoices(currentChoices);
        }
    }

    public bool IsDialogueComplete()
    {
        return isTextComplete && !audioSource.isPlaying && !choicesPresent;
    }
}

