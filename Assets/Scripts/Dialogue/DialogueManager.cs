using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;

    [SerializeField]
    private DialogueUI _dialogueUI;

    private AudioSource _audio;
    private Character _currentCharacter;

    [SerializeField]
    private float _delayBeforeHidingUI = 3;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _audio = GetComponent<AudioSource>();
            HideUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void StartDialogue(Character character)
    {
        _instance._currentCharacter = character;

        DialogueSet dialogueSet = _instance._currentCharacter.GetDialogueSet();

        DialogueEntry dialogue = dialogueSet.GetInitialDialogue();

        ShowUI();

        _instance.StartCoroutine(_instance.PlayDialogue(dialogue));
    }

    private void PlayDialogueCoroutine(DialogueEntry dialogue)
    {
        StartCoroutine(PlayDialogue(dialogue));
    }

    private IEnumerator PlayDialogue(DialogueEntry dialogue)
    {
        if (!string.IsNullOrEmpty(dialogue.animationName))
        {
            _currentCharacter.PlayAnimation(dialogue.animationName);
        }

        _dialogueUI.UpdateDialogueText(_currentCharacter.name, dialogue);

        _audio.PlayOneShot(dialogue.audioClip);

        Leprechaun leprechaun = _currentCharacter.GetComponent<Leprechaun>();

        if (leprechaun != null)
        {
            leprechaun.SetTalking(true);
        }

        yield return new WaitUntil(() => IsDialogueComplete());

        if (leprechaun != null)
        {
            leprechaun.SetTalking(false);
        }

        if (dialogue.isTrigger)
        {
            yield return GameManager.TriggerDialogueCallback(dialogue.key);
        }

        if (dialogue.HasChoices())
        {
            _dialogueUI.ShowChoices(dialogue);
        }
        else if (dialogue.PointsToNextDialogue())
        {
            if (dialogue.RequiresProximityCheck())
            {
                yield return new WaitUntil(() => ProximityCheck());
                PlayNextDialogue();
            }
            else
            {
                _dialogueUI.ShowNextButton();
            }
        }
        else
        {
            StartCoroutine(CompleteDialogue());
        }
    }

    public static void PlayNextDialogue()
    {
        DialogueSet dialogueSet = _instance._currentCharacter.GetDialogueSet();
        
        DialogueEntry currentDialogue = dialogueSet.GetCurrentDialogue();
        DialogueEntry nextDialogue = dialogueSet.SetCurrentDialogue(currentDialogue.nextDialogueKey);

        _instance.PlayDialogueCoroutine(nextDialogue);
    }

    public static void HandleChoice(string nextDialogueKey)
    {
        DialogueSet dialogueSet = _instance._currentCharacter.GetDialogueSet();

        DialogueEntry nextDialogue = dialogueSet.SetCurrentDialogue(nextDialogueKey);

        _instance.PlayDialogueCoroutine(nextDialogue);
    }

    public static bool IsDialogueComplete()
    {
        return _instance._dialogueUI.IsTextComplete() && !_instance._audio.isPlaying;
    }

    private IEnumerator CompleteDialogue()
    {
        yield return new WaitForSeconds(_delayBeforeHidingUI);

        HideUI();

        _currentCharacter.Idle();
        
        UpdateGameState();
    }

    private void UpdateGameState() {
        DialogueEntry dialogue = _currentCharacter.GetDialogueSet().GetCurrentDialogue();

        if (dialogue.shouldProgressGameState)
        {
            GameManager.SetGameState(dialogue.progressToGameState);
        }
    }

    public static void Despawn()
    {
        HideUI();
        _instance._audio.Stop();
    }

    public static void ShowUI()
    {
        _instance._dialogueUI.gameObject.SetActive(true);
    }

    public static void HideUI()
    {
        _instance._dialogueUI.gameObject.SetActive(false);
    }

    private bool ProximityCheck()
    {
        DialogueEntry dialogue = _currentCharacter.GetDialogueSet().GetCurrentDialogue();

        if (dialogue.requiredProximity == 0)
        {
            return true;
        }

        float distanceToCharacter = Vector3.Distance(
            Camera.main.transform.position,
            _currentCharacter.transform.position
        );

        return distanceToCharacter <= dialogue.requiredProximity;
    }
}