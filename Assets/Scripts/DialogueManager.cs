using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private List<SceneDialogue> sceneDialoguesList = new List<SceneDialogue>();
    public Dictionary<string, DialogueEntry[]> sceneDialoguesDictionary = new Dictionary<string, DialogueEntry[]>();

    private DialogueUI dialogueUI;

    void Awake()
    {
        sceneDialoguesDictionary = sceneDialoguesList.ToDictionary(sceneDialogue => sceneDialogue.sceneName, sceneDialogue => sceneDialogue.dialogues);
        //audioSource = GetComponent<AudioSource>();
        dialogueUI = FindObjectOfType<DialogueUI>();
    }

    void Start()
    {
        StartDialogueForScene("Start");
    }

    public void StartDialogueForScene(string sceneName)
    {
        if (sceneDialoguesDictionary.TryGetValue(sceneName, out DialogueEntry[] dialogues))
        {
            StartCoroutine(PlayDialogues(dialogues));
        }
        else
        {
            Debug.LogWarning($"No dialogues found for scene: {sceneName}");
        }
    }

    private IEnumerator PlayDialogues(DialogueEntry[] dialogues)
    {
        string nextScene = null;

        foreach (var dialogue in dialogues)
        {
            dialogueUI.UpdateDialogueText(dialogue.text, dialogue.audioClip, dialogue.choices);

            if (dialogue.audioClip != null)
            {
                yield return new WaitUntil(() => dialogueUI.IsDialogueComplete());
            }

            if (dialogue.nextDialogueKey != null)
            {
                nextScene = dialogue.nextDialogueKey;
            }
        }

        if (nextScene != null)
        {
            StartDialogueForScene(nextScene);
        }
    }
}
