using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public PlayerInteraction playerDistance;
    public DialogueInteraction npcDialogue;
    public DialogueManager dialogueManager;
    public GameObject npcPortraitCanvas;
    public GameObject npcBoxCanvas;

    private void Start() {
        npcDialogue.PopulateData();
    }

    private void Update()
    {
        // makes the pop up box face the player
        npcBoxCanvas.transform.LookAt(npcBoxCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

        Vector3 offsetToCamera = npcPortraitCanvas.transform.position - Camera.main.transform.position;
        Vector3 horizontalCameraPosition = npcPortraitCanvas.transform.position + new Vector3(offsetToCamera.x, 0f, offsetToCamera.z);
        npcPortraitCanvas.transform.LookAt(horizontalCameraPosition, Vector3.up);
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
        npcBoxCanvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        npcBoxCanvas.SetActive(false);
    }
}
