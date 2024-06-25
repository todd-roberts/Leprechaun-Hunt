using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public string key;
    public string text;
    public AudioClip audioClip;

    public bool isCheckpoint;
    public string nextDialogueKey;

     public bool PointsToNextDialogue()
    {
        return nextDialogueKey != null &&
            nextDialogueKey.Trim() != "";
    }

    public string animationName;
    public float requiredProximity;

    public bool RequiresProximityCheck() => requiredProximity > 0;

    public DialogueChoice choice1;
    public DialogueChoice choice2;

    public bool HasChoices() => choice1 != null && choice1.label != null && choice1.label.Trim() != "";

    public bool shouldProgressGameState;
    public GameState progressToGameState;
}
