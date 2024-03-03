using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public string text;
    public AudioClip audioClip;
    public DialogueChoice[] choices;
    public string nextDialogueKey;
}

[System.Serializable]
public class DialogueChoice
{ 
    public string label;
    public string nextDialogueKey;
}

[System.Serializable]
public class SceneDialogue
{
    public string sceneName;
    public DialogueEntry[] dialogues;
}

