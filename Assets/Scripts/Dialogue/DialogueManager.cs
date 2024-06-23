using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    private DialogueUI _dialogueUI;
    private Character _currentCharacter;
    private DialogueSet _currentDialogueSet;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _dialogueUI = FindObjectOfType<DialogueUI>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartDialogue(Character character)
    {
        if (_instance == null)
        {
            Debug.LogError("DialogueManager instance is not set.");
            return;
        }

        _instance._currentCharacter = character;
        _instance._currentDialogueSet = character.GetDialogueSet();

        if (_instance._currentDialogueSet != null && _instance._currentDialogueSet.HasMoreDialogues())
        {
            _instance._dialogueUI.ShowUI();
            _instance.StartCoroutine(_instance.PlayDialogues(_instance._currentDialogueSet));
        }
        else
        {
            Debug.LogWarning($"No dialogues found for game state: {GameManager.GetGameState()}");
        }
    }

    private IEnumerator PlayDialogues(DialogueSet dialogueSet)
    {
        while (dialogueSet.HasMoreDialogues())
        {
            DialogueEntry dialogue = dialogueSet.GetNextDialogue();
            _dialogueUI.UpdateDialogueText(dialogue.text, dialogue.audioClip, dialogue.choices);

            yield return new WaitUntil(() => _dialogueUI.IsDialogueComplete());

            if (dialogue.choices != null && dialogue.choices.Length > 0)
            {
                yield break; // Stop if there are choices to be made
            }
        }
    }

    public static void HandleChoice(string nextDialogueKey)
    {
        if (_instance == null)
        {
            Debug.LogError("DialogueManager instance is not set.");
            return;
        }

        if (_instance._currentCharacter != null)
        {
            DialogueSet dialogueSet = _instance._currentCharacter.GetDialogueSet();
            dialogueSet.Reset(); // Reset to start the new branch of dialogues

            _instance.StartCoroutine(_instance.PlayDialogues(dialogueSet));
        }
    }

    public static void TriggerNextDialogue()
    {
        if (_instance == null || _instance._currentDialogueSet == null)
        {
            Debug.LogError("DialogueManager instance or current dialogue set is not set.");
            return;
        }

        if (_instance._currentDialogueSet.HasMoreDialogues())
        {
            DialogueEntry dialogue = _instance._currentDialogueSet.GetNextDialogue();
            _instance._dialogueUI.UpdateDialogueText(dialogue.text, dialogue.audioClip, dialogue.choices);
        }
        else
        {
            _instance._dialogueUI.HideUI();
            // Optionally transition to another state or end dialogue
        }
    }

    public static Character GetCurrentCharacter()
    {
        return _instance._currentCharacter;
    }
}
