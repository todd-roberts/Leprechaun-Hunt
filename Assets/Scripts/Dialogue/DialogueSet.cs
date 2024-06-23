using System.Collections.Generic;

public class DialogueSet
{
    private List<DialogueEntry> _dialogues;
    private int _currentDialogueIndex;

    public DialogueSet(List<DialogueEntry> dialogues)
    {
        _dialogues = dialogues;
        _currentDialogueIndex = 0;
    }

    public DialogueEntry GetNextDialogue()
    {
        if (_currentDialogueIndex < _dialogues.Count)
        {
            return _dialogues[_currentDialogueIndex++];
        }

        return null;
    }

    public bool HasMoreDialogues()
    {
        return _currentDialogueIndex < _dialogues.Count;
    }

    public void Reset()
    {
        _currentDialogueIndex = 0;
    }
}