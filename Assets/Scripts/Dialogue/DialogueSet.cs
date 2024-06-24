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

    public DialogueEntry GetInitialDialogue()
    {
        _currentKey = _lastCheckpointKey;
        
        return GetCurrentDialogue();
    }

    public DialogueEntry GetNextDialogue() => 
        SetCurrentDialogue(GetCurrentDialogue().nextDialogueKey);
    

    public DialogueEntry SetCurrentDialogue(string key)
    {
        _currentKey = key;

         _lastCheckpointKey = GetCurrentDialogue().isCheckpoint ? _currentKey : _lastCheckpointKey;

        return GetCurrentDialogue();
    }

    public DialogueEntry GetCurrentDialogue() => _dialogues[_currentKey];

}
