using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStateChangeObserver : MonoBehaviour
{
    public delegate void OnDialogueStateChange(bool dialogueActive);
    public static event OnDialogueStateChange DialogueStateChange;

    public static void NotifyDialogueStateChange(bool dialogueActive)
    {
        DialogueStateChange?.Invoke(dialogueActive);
    }

    /*
    void OnEnable(){
        DialogueStateChangeObserver.DialogueStateChange += OnDialogueStateChange;
    }
    void OnDisable(){
        DialogueStateChangeObserver.DialogueStateChange -= OnDialogueStateChange;
    }

    void OnDialogueStateChange(bool dialogueActive){
        Debug.Log("Dialogue is active: " + dialogueActive);
    }
    
    */
}
