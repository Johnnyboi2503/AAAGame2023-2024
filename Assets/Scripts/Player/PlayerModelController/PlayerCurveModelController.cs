using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurveModelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<Transform> armature = new List<Transform>();
    [SerializeField] SlideAction slideAction;
    [SerializeField] DashThroughAction dashThroughAction;
    [SerializeField] DownwardStabAction downwardStabAction;
    [SerializeField] FlickAction flickAction;
    [SerializeField] GameObject constraint;
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform constraintTarget;

    [Header("Adjustable Variables")]
    [SerializeField] float saveRate;
    [SerializeField] int numOfPoints = 1;
    List<Vector3> upDirection = new List<Vector3>();
    List<Quaternion> armatureInitalRotation = new List<Quaternion>();

    float saveTimer = 0;
    bool saving = false;
    
    enum ActionSelected {
        SLIDE,
        DASHTRHOUGH,
        DOWNWARDSTAB,
        FLICK,
        NULL
    }

    ActionSelected currentAction = ActionSelected.NULL;
    private void Start() {
        // Sorting by y position
        for(int i = 1; i < armature.Count; i++) {
            for (int j = 0; j < armature.Count-i; j++) {
                if(armature[j].position.y < armature[j+1].position.y) {
                    // Swap
                    Transform holder = armature[j];
                    armature[j] = armature[j + 1];
                    armature[j + 1] = holder;
                }
            }
        }

        // Saving all the local rotations
        foreach (Transform part in armature) {
            armatureInitalRotation.Add(part.transform.rotation);
        }

        // Setting listeners
        slideAction.OnStartAction.AddListener(SlideSave);
        slideAction.OnEndAction.AddListener(EndSaving);

        dashThroughAction.OnStartAction.AddListener(DashThroughSave);
        dashThroughAction.OnEndAction.AddListener(EndSaving);

        downwardStabAction.OnStartAction.AddListener(DownwardStabSave);
        downwardStabAction.OnEndAction.AddListener(EndSaving);

        flickAction.OnStartAction.AddListener(FlickSave);
        flickAction.OnEndAction.AddListener(EndSaving);

        constraint.SetActive(false);
    }

    public void DashThroughSave() {
        currentAction = ActionSelected.DASHTRHOUGH;
        StartSavingInternal();
    }
    public void SlideSave() {
        currentAction = ActionSelected.SLIDE;
        StartSavingInternal();
    }
    public void DownwardStabSave() {
        currentAction = ActionSelected.DOWNWARDSTAB;
        StartSavingInternal();
    }
    public void FlickSave() {
        currentAction = ActionSelected.FLICK;
        StartSavingInternal();
    }
    public void StartSavingInternal() {
        constraint.SetActive(true);
        constraintTarget.GetComponent<SmoothFollowTarget>().SetTarget(targetTransform);
        upDirection.Clear();
        saveTimer = 0;
        saving = true;
    }

    public void EndSaving() {
        currentAction = ActionSelected.NULL;
        constraint.SetActive(false);
        saving = false;
    }

    private void Update() {
        SaveUpdate();
    }

    private void LateUpdate() {
        LateSaveUpdate();
    }

    private void LateSaveUpdate() {
        if (saving) {
            if (saveTimer <= 0) {
                SettingRotations();
                saveTimer = saveRate;
            }
        }
    }
    private void SaveUpdate() {
        if (saving) {
            saveTimer -= Time.deltaTime;
            // Additional update
            if (saveTimer <= 0) {
                switch (currentAction) {
                    case ActionSelected.DASHTRHOUGH:
                        AddUpDirectionToFront(dashThroughAction.GetActionDirection());
                        break;

                    case ActionSelected.SLIDE:
                        AddUpDirectionToFront(slideAction.GetSlideDirection());
                        break;
                    case ActionSelected.DOWNWARDSTAB:
                        AddUpDirectionToFront(Vector3.down);
                        break;
                    case ActionSelected.FLICK:
                        AddUpDirectionToFront(Vector3.up);
                        break;
                }
                SettingRotations();
            }
        }
    }

    private void SettingRotations() {
        Vector3 newUp = Vector3.up;
        // Setting the rotations values
        for (int i = 0; i < armature.Count; i++) {
            // If there is up directions
            if (upDirection.Count > i / (armature.Count / numOfPoints)) {
                newUp = upDirection[i / (armature.Count / numOfPoints)];
                armature[i].rotation = Quaternion.FromToRotation(Vector3.up, newUp) * armatureInitalRotation[i];
            }
            else {
                // If there are no up directions set it back to normal
                armature[i].rotation = armatureInitalRotation[i];
            }
        }
    }

    public void SetNumberOfPoints(int points) {
        if (points > 0) {
            numOfPoints = points;
        }
    }

    public void AddUpDirectionToFront(Vector3 newUpDirection) {
        // Adding to the front of the list
        Vector3 newOne = newUpDirection;
        Vector3 holder;
        for(int i = 0; i < upDirection.Count; i++) {
            holder = upDirection[i];
            upDirection[i] = newOne;
            newOne = holder;
        }

        // Only adding new point size if within limits
        if (upDirection.Count < numOfPoints) {
            upDirection.Add(newOne);
        }
    }
}
