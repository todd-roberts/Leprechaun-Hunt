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

    private IEnumerator PlayDialogue(DialogueEntry dialogue)
    {
        if (dialogue.animationName != null && dialogue.animationName.Trim() != "")
        {
            _currentCharacter.PlayAnimation(dialogue.animationName);
        }

        _dialogueUI.UpdateDialogueText(_currentCharacter.name, dialogue);

        _audio.PlayOneShot(dialogue.audioClip);

        yield return new WaitUntil(() => _instance.IsDialogueComplete());

        DialogueSet dialogueSet = _currentCharacter.GetDialogueSet();
        DialogueEntry currentDialogue = dialogueSet.GetCurrentDialogue();

        if (dialogueSet.HasMoreDialogues()) {
            if (currentDialogue.requiredProximity == 0)
            {
                _dialogueUI.ShowNextButton();
            }
            else {
                yield return new WaitUntil(() => _instance.ProximityCheck());
                PlayNextDialogue();
            }
        }
        else
        {
            _instance.StartCoroutine(_instance.CompleteDialogue());
        }
    }

    public static void PlayNextDialogue()
    {
        DialogueSet dialogueSet = _instance._currentCharacter.GetDialogueSet();

        DialogueEntry nextDialogue = dialogueSet.GetNextDialogue();

        _instance.StartCoroutine(_instance.PlayDialogue(nextDialogue));
    }

    public bool IsDialogueComplete()
    {
        return _instance._dialogueUI.IsTextComplete() && !_audio.isPlaying;
    }

    private IEnumerator CompleteDialogue()
    {
        yield return new WaitForSeconds(_delayBeforeHidingUI);
        HideUI();
        _currentCharacter.Idle();
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
