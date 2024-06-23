using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public string key;
    public string text;
    public AudioClip audioClip;
    public DialogueChoice[] choices;

    public bool isCheckpoint;
    public string nextDialogueKey;

    public string animationName;

    public float requiredProximity;
}
