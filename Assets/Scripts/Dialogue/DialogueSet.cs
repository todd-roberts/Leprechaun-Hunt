using System.Collections.Generic;

public class DialogueSet
{
    private List<DialogueEntry> _dialogues;
    private int _currentIndex = 0;

    public DialogueSet(List<DialogueEntry> dialogues)
    {
        _dialogues = dialogues;
    }

    public bool HasMoreDialogues()
    {
        return _currentIndex < _dialogues.Count;
    }

    public bool HasMoreDialogues(int index)
    {
        return index < _dialogues.Count;
    }

    public DialogueEntry GetNextDialogue()
    {
        if (HasMoreDialogues())
        {
            return _dialogues[_currentIndex++];
        }
        return null;
    }

    public DialogueEntry GetDialogueAt(int index)
    {
        if (index >= 0 && index < _dialogues.Count)
        {
            return _dialogues[index];
        }
        return null;
    }

    public void Reset()
    {
        _currentIndex = 0;
    }
}
