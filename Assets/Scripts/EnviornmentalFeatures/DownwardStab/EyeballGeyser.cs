using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EyeballGeyser : DownwardStabEffect {
    [Header("References")]
    [SerializeField] GameObject bloodGeyserPrefab;

    [Header("Geyser Size")]
    [SerializeField] float height;
    [SerializeField] float radius;
    [SerializeField] float duration;
    [SerializeField] bool isInfinate;

    [Header("Geyser Functionality")]
    [SerializeField] float geyserAcceleration;
    [SerializeField] float geyserMaxSpeed;

    [Space]
    [Header("Audio")]
    [SerializeField] private float bloodGeyserAudioVolume = 1f;
    [SerializeField] private float bloodGeyserAudioDistanceRange = 20f;


    // Variables
    float geyserTimer;
    bool isBlood = false;
    GameObject currentBloodGeyser;

    private void Awake()
    {
        FindObjectOfType<PlayerKillable>().OnDie.AddListener(ResetObject);
    }

    private void Update() {
        if (isBlood) {
            if (!isInfinate) {
                geyserTimer -= Time.deltaTime;
                if (geyserTimer <= 0f) {
                    EndBloodGeyser();
                }
            }
        }
    }

    override public void TriggerEffect() {
        StartBloodGeyser();
    }

    public void StartBloodGeyser() {

        AudioManager.GetInstance().PlayAudioAtLocation("BloodGeyser_SFX", transform.position, bloodGeyserAudioVolume, true, bloodGeyserAudioDistanceRange);

        if (!isBlood) {
            isBlood = true;
            geyserTimer = duration;


            currentBloodGeyser = Instantiate(bloodGeyserPrefab, transform);

            //Setting position and scale
            currentBloodGeyser.transform.localPosition = Vector3.up * (height / 2);
            currentBloodGeyser.transform.localScale = new Vector3(radius, height / 2, radius);
            currentBloodGeyser.GetComponent<BloodGeyser>().SetStats(geyserAcceleration, geyserMaxSpeed);
        }
    }
    private void EndBloodGeyser() {

        AudioManager.GetInstance().StopAudioOfType("BloodGeyser_SFX");

        if (isBlood) {
            isBlood = false;

            Destroy(currentBloodGeyser);
        }
    }
    private void OnDrawGizmos() {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.up * height/2, new Vector3(radius, height, radius));
    }

    private void ResetObject()
    {
        AudioManager.GetInstance().StopAudioOfType("BloodGeyser_SFX");
    }
}
