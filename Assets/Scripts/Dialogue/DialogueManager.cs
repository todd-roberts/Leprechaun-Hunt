using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;

    [SerializeField]
    private DialogueUI _dialogueUI;
    private Character _currentCharacter;
    private DialogueSet _currentDialogueSet;
    private IEnumerator _dialogueCoroutine;
    private int _currentDialogueIndex;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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
            ShowUI();
            _instance._dialogueCoroutine = _instance.PlayDialogue(_instance._currentDialogueIndex);
            _instance.StartCoroutine(_instance._dialogueCoroutine);
        }
        else
        {
            Debug.LogWarning($"No dialogues found for game state: {GameManager.GetGameState()}");
        }
    }

    private IEnumerator PlayDialogue(int dialogueIndex)
    {
        if (_currentDialogueSet.HasMoreDialogues(dialogueIndex))
        {
            DialogueEntry dialogue = _currentDialogueSet.GetDialogueAt(dialogueIndex);
            _dialogueUI.UpdateDialogueText(_currentCharacter.name, dialogue.text, dialogue.audioClip, dialogue.choices);

            // Wait until the user clicks next button
            yield return new WaitUntil(() => _dialogueUI.IsDialogueComplete());

            if (dialogue.choices != null && dialogue.choices.Length > 0)
            {
                yield break; // Stop if there are choices to be made
            }

            // Wait for next button click
            yield return new WaitUntil(() => _dialogueUI.IsNextClicked());

            _currentDialogueIndex++;
        }

        if (_currentDialogueSet.HasMoreDialogues(_currentDialogueIndex))
        {
            _dialogueCoroutine = PlayDialogue(_currentDialogueIndex);
            StartCoroutine(_dialogueCoroutine);
        }
        else
        {
            StartCoroutine(HideNextButtonAfterDelay());
            HideUI();
        }
    }

    private IEnumerator HideNextButtonAfterDelay()
    {
        yield return new WaitForSeconds(2);
        _dialogueUI.HideNextButton();
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
            _instance._currentDialogueIndex = 0;

            if (_instance._dialogueCoroutine != null)
            {
                _instance.StopCoroutine(_instance._dialogueCoroutine);
            }
            _instance._dialogueCoroutine = _instance.PlayDialogue(_instance._currentDialogueIndex);
            _instance.StartCoroutine(_instance._dialogueCoroutine);
        }
    }

    public static void TriggerNextDialogue()
    {
        if (_instance == null || _instance._currentDialogueSet == null)
        {
            Debug.LogError("DialogueManager instance or current dialogue set is not set.");
            return;
        }

        if (_instance._currentDialogueSet.HasMoreDialogues(_instance._currentDialogueIndex))
        {
            _instance._dialogueCoroutine = _instance.PlayDialogue(_instance._currentDialogueIndex);
            _instance.StartCoroutine(_instance._dialogueCoroutine);
        }
        else
        {
            _instance.StartCoroutine(_instance.HideNextButtonAfterDelay());
            HideUI();
        }
    }

    public static Character GetCurrentCharacter()
    {
        return _instance._currentCharacter;
    }

    public static void ShowUI()
    {
        _instance._dialogueUI.gameObject.SetActive(true);
    }

    public static void HideUI()
    {
        _instance._dialogueUI.gameObject.SetActive(false);
    }
}
