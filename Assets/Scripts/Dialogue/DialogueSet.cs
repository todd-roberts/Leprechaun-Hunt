using System.Collections.Generic;
using UnityEngine;

public class DialogueSet
{
    private Dictionary<string, DialogueEntry> _dialogues;
    private string _currentKey;
    private string _lastCheckpointKey;

    public DialogueSet(List<DialogueEntry> dialogues)
    {
        _dialogues = new Dictionary<string, DialogueEntry>();
        foreach (var dialogue in dialogues)
        {
            _dialogues[dialogue.key] = dialogue;
        }

        _currentKey = dialogues[0].key;
        _lastCheckpointKey = _currentKey;
    }

    public bool HasMoreDialogues()
    {
        return _dialogues[_currentKey].nextDialogueKey != null 
            && _dialogues[_currentKey].nextDialogueKey.Trim() != "";
    }

    public DialogueEntry GetInitialDialogue()
    {
        _currentKey = _lastCheckpointKey;
        return _dialogues[_currentKey];
    }

    public DialogueEntry GetNextDialogue()
    {
        if (_dialogues[_currentKey].nextDialogueKey == null)
        {
            return null;
        }

        _currentKey = _dialogues[_currentKey].nextDialogueKey;

        if (_dialogues[_currentKey].isCheckpoint)
        {
            _lastCheckpointKey = _currentKey;
        }

        return _dialogues[_currentKey];
    }

    public void SetCurrentDialogue(string key)
    {
        if (_dialogues.ContainsKey(key))
        {
            _currentKey = key;
        }
    }

    public DialogueEntry GetCurrentDialogue() => _dialogues[_currentKey];

}
