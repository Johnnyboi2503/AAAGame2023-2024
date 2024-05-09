using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibPlatform : MonoBehaviour
{

    [Header("Variables")]
    // [SerializeField] float launchForce; //force applied to player when the ribs close
    [SerializeField] float waitTime; //time the platform waits before closing
    [SerializeField] float timeClosed; //time the platform is closed before reopening
    [SerializeField] bool waitingToClose; //set to true as soon as the player lands on the platform for the first time
    [SerializeField] float bloodLostOnHit; //blood lost when hit by knockback volume
    [SerializeField] float closeCooldown; // Cooldown between closing jaws, count down starts after opening
    [SerializeField] Transform holdInPlace;

    [SerializeField] FlyTrapTempVisual tempVisual; // Reference to visual script

    [Space]
    [Header("Audio")]
    [SerializeField] private float porkRibsAudioVolume = 0.75f;
    [SerializeField] private float porkRibsAudioDistanceRange = 10f;

    Timer timerTillClosed = new Timer();
    Timer timerTillOpened = new Timer();

    bool hitPlayer = false;
    bool ribsClosed = false;
    float cooldownTimer;
    GameObject player = null;

    // Update is called once per frame
    void Update()
    {
        timerTillClosed.UpdateTimer();
        timerTillOpened.UpdateTimer();

        // Cooldown timer
        if (!ribsClosed) {
            cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
        }
        else {
            if (player != null) {
                player.transform.position = holdInPlace.position;
            }
        }
    }

    private void CloseRibs() {
        AudioManager.GetInstance().StopAudioOfType("PorkRibs_Platform_SFX");
        timerTillOpened.StartTimer(timeClosed, OpenRibs);

        // Enabling collider
        ribsClosed = true;

        // Closing ribs
        tempVisual.StopShake();
        tempVisual.Close();
    }

    private void OpenRibs()
    {
        cooldownTimer = closeCooldown;
        hitPlayer = false;
        tempVisual.Open();

        //play opening animation
        ribsClosed = false;
        waitingToClose = false;
        player = null;
    }

    public void FoundPlayer(PlayerInput playerInput) {
        // Checkking
        if (!ribsClosed && !waitingToClose && cooldownTimer <= 0) {
            AudioManager.GetInstance().PlayAudioAtLocation("PorkRibs_Platform_SFX", transform.position, porkRibsAudioVolume, true, porkRibsAudioDistanceRange);
            timerTillClosed.StartTimer(waitTime, CloseRibs);
            waitingToClose = true;
            tempVisual.Shake();
        }

        // Stunning and dealing damage to player
        if (!hitPlayer && ribsClosed) {
            // Affecting player
            BloodThirst bloodThirst = playerInput.GetComponent<BloodThirst>();
            bloodThirst.LoseBlood(bloodLostOnHit); //player loses blood when they get hit by the volume
            playerInput.gameObject.GetComponentInChildren<PlayerStun>().Stun(timeClosed); // Stunning the player
            player = playerInput.gameObject;

            // Updating values
            hitPlayer = true;
        }
    }
}
