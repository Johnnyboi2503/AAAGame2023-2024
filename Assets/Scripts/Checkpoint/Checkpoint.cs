using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    /*----------------------------------------------
     Script is to be added to checkpoint prefab
    ----------------------------------------------*/
    // place the checkpoint manager prefab into the scene, and any actual checkpoint prefabs inside of this manager.
    public int index; // Set by the checkpoint manager based on index in heirarchy
    public bool triggered = false; // Set by the manager
    public CheckPointManager checkPointManager; // Used to check if they can be used as checkpoint
    public Renderer render; // Temperary component use for testing
    [SerializeField] LevelStateController levelStateController; // Used to save the load the state of the level

    [Space]
    [SerializeField] private float interactionAudioVolume = 0.75f;


    public void ActivateCheckpoint() {
        triggered = true;
        render.material.color = Color.cyan; // Temperary visual to show it has been activated
    }

    private void Start() {
        render = GetComponent<Renderer>();
        levelStateController = FindAnyObjectByType<LevelStateController>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.CompareTag("Player"))// checks if the collider is a checkpoint
        {
            if(checkPointManager.CanActivateCheckpoint(index)) {

                // Play audio on activate checkpoint
                AudioManager.GetInstance().PlayAudioAtLocation("UISelectSFX", transform.position, interactionAudioVolume);

                render.material.color = Color.blue;
                collider.transform.GetComponent<PlayerKillable>().respawnPosition = transform.position; // setting player respawn position if valid checkpoint
                levelStateController.SaveLevelState();
                CheckpointEnterObserver.NotifyCheckpointEnter();
            }
        }
    }
}
