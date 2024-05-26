using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public PlayerInteraction playerDistance;
    public DialogueInteraction npcDialogue;
    public DialogueManager dialogueManager;
    public GameObject npcCanvas;

    private void Start() {
        npcDialogue.PopulateData();
    }

    private void Update()
    {
        // makes the pop up box face the player
        npcCanvas.transform.LookAt(npcCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    // starts the dialogue using idalogue manager
    public override void Interact()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(npcDialogue);
    }

    // activates the canvas gameobject 
    public override void InteractPopUp()
    {
        npcCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        npcCanvas.SetActive(false);
    }
}
