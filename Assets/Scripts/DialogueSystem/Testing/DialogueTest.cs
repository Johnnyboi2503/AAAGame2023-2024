using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTest : MonoBehaviour {
    [SerializeField]
    DialogueInteraction dialogueInteraction;
    DialogueManager manager;

    UnityEvent endDialogue = new UnityEvent();
    private void Start() {
        manager = FindAnyObjectByType<DialogueManager>();
        endDialogue.AddListener(TestEndDialougeEvent);
        dialogueInteraction.PopulateData();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            manager.StartDialogue(dialogueInteraction, endDialogue);
        }
    }
    private void TestEndDialougeEvent() {
        Debug.Log(name + " end dialogue test");
    }
}
