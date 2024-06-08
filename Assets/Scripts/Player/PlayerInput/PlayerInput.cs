using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
public class PlayerInput : MonoBehaviour {
    // Components
    enum ControlType { controller, mouseAndKeyboard };
    CinemachineFreeLook cinemachineCam;
    Transform cameraOrientation;
    [SerializeField]Texture2D cursorSprite;

    [Header("Control Type")]
    [SerializeField] ControlType currentControls; // Current control set up, keyboard or mouse

    [Header("Input Variables")]
    [SerializeField] float combinationWindow; // The window of time you have to press two inputs at the same time to count as a combination move (StabDashAction and SlashDashAction)

    [Header("Mouse and Keyboard Camera Sensitivity")]
    [SerializeField] float MKXSensitivity; // Camera X sensitivity MK
    [SerializeField] float MKYSensitivity; // Camera Y sensitivity MK

    [Header("Controller Camera Sensitivity")]
    [SerializeField] float ControllerXSensitivity; // Camera X sensitivity for controller
    [SerializeField] float ControllerYSensitivity; // Camera Y sensitivity for controller

    // All of these are inputs for their respective inputs
    [Header("Mouse Keyboard Inputs")]
    [SerializeField] KeyCode keyboardStab;// Seperate this one because I wanna only have the header apply to one
    [SerializeField] KeyCode keyboardSlash, keyboardDash, keyboardJump, keyboardShoot, keyboardDialogue, keyboardInteract;

    [Header("Controller Inputs")]
    [SerializeField] KeyCode controllerStab; // Seperate this one because I wanna only have the header apply to one
    [SerializeField] KeyCode controllerSlash, controllerDash, controllerJump, controllerShoot, controllerDialogue, controllerInteract;

    // Controls
    KeyCode inputStab, inputSlash, inputDash, inputJump, inputShoot, inputDialogue, inputInteract;

    bool canInput = true, canAbilityInput = true;

    PlayerActionManager playerActionManager;
    DialogueManager dialogueManager;
    PlayerInteraction playerInteraction;
    // Start is called before the first frame update
    void Start() {
        
        // Getting camera components
        cameraOrientation = FindObjectOfType<Camera>().transform;
        cinemachineCam = FindObjectOfType<CinemachineFreeLook>();
        playerActionManager = GetComponentInChildren<PlayerActionManager>();
        dialogueManager = FindAnyObjectByType<DialogueManager>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        playerActionManager.combinationWindow = combinationWindow;

        //SET CURSOR TEXTURE LIKE HERE
        if(cursorSprite != null)
        {
            Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
        }
        //hide cursor right after
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SetCurrentController();
    }

    // Update is called once per frame
    void Update() {
        if (canInput) {
            CheckInputChange();
            // Initalizing input direction
            Vector3 inputDirection = Vector3.zero;

            //player input direction is calculated by multiplying forward and right by the horizontal and vertical axes
            switch (currentControls) {
                //switch so that you can't control the player with a control scheme if it isn't assigned
                case ControlType.controller:
                    inputDirection = cameraOrientation.right * Input.GetAxis("Left Stick Horizontal") + cameraOrientation.forward * Input.GetAxis("Left Stick Vertical");
                    break;
                case ControlType.mouseAndKeyboard:
                    inputDirection = cameraOrientation.right * Input.GetAxis("Horizontal") + cameraOrientation.forward * Input.GetAxis("Vertical");
                    break;
                default: break;
            }
            OtherInputs();
            Vector3 horizontalInput = new Vector3(inputDirection.x, 0, inputDirection.z);
            if (canAbilityInput) {
                CheckAbilities(horizontalInput);
            }
            else {
                horizontalInput = Vector3.zero;
            }
            playerActionManager.DirectionalInput(horizontalInput);
        }
    }
    public void DisableInput() {
        canInput = false;
    }
    public void EnableInput() {
        canInput = true;
    }
    public void DisableAbilityInput() {
        canAbilityInput = false;
        DisableCameraMovement();
    }
    public void EnableAbilityInput() {
        canAbilityInput = true;
        EnableCameraMovement();
    }

    public void DisableCameraMovement() {
        cinemachineCam.m_XAxis.m_MaxSpeed = 0;
        cinemachineCam.m_YAxis.m_MaxSpeed = 0;
    }
    public void EnableCameraMovement() {
        SetCurrentController();
    }

    private void OtherInputs() {
        // starting dialogue
        if (Input.GetKeyDown(inputInteract))
        {
            Debug.Log("interact key press");
            playerInteraction.TryInteract();
        }
        // Advancing dialogue
        if (Input.GetKeyDown(inputDialogue)) {
            dialogueManager.DialougeAdvanceInput();
        }
    }
    private void CheckAbilities(Vector3 direction) {
        if (Input.GetKeyDown(inputJump)) {
            playerActionManager.JumpInputPressed();
        }
        if (Input.GetKeyUp(inputJump)) {
            playerActionManager.JumpInputRelease();
        }
        if (Input.GetKeyDown(inputDash)) {
            playerActionManager.DashInput(direction);
        }
        if (Input.GetKey(inputStab)) {
            playerActionManager.StabInputHold();
        }
        if (Input.GetKeyUp(inputStab)) {
            playerActionManager.StabInputRelease();
        }
        if (Input.GetKeyDown(inputStab)) {
            playerActionManager.StabInputPressed(direction);
        }
        if (Input.GetKeyDown(inputSlash)) {
            playerActionManager.SlashInput(direction);
        }
        if (Input.GetKeyDown(inputShoot)) {
            playerActionManager.EnergyBlastInput();
        }
    }

    private void SetCurrentController() {
        // Setting controls for camera
        switch (currentControls) {
            case ControlType.controller:
                SetControllerControls();
                break;
            case ControlType.mouseAndKeyboard:
                SetMouseKeyboardControls();
                break;
            default:
                break;
        }
    }
    // This is used to swap the control scheme whenever the player presses any key in any scheme
    private void CheckInputChange() {

        switch (currentControls)
        {
            //depending on the control scheme you're using, the game will check
            //if the control scheme that isn't in use gets input on the camera look axis
            case ControlType.controller:
                if (PressedKeyboardMouse())
                {
                    SetMouseKeyboardControls();
                    Debug.Log("switch to keyboard");

                }
                break;
            case ControlType.mouseAndKeyboard:
                if (PressedController())
                {
                    SetControllerControls();
                    Debug.Log("switch to controller");

                }
                break;
            default : break;
        }
       
        
    }

    private bool PressedController() {//checking if camera look has been input on the controller
        if(Mathf.Abs(Input.GetAxisRaw("Right Stick Horizontal")) > 0.1f)
        {
            return true;
        }
        if(Mathf.Abs(Input.GetAxisRaw("Right Stick Vertical")) > 0.1f)
        {
            return true;
        }
        if(Mathf.Abs(Input.GetAxisRaw("Left Stick Horizontal")) > 0.1f)
        {
            return true;
        }
        if (Mathf.Abs(Input.GetAxisRaw("Left Stick Vertical")) > 0.1f)
        {
            return true;
        }

        return false;
    }
    private bool PressedKeyboardMouse() {// ^^ same but for keyboard
        if(Mathf.Abs( Input.GetAxisRaw("Mouse X")) > 0.1f)
        {
            return true;
        }
        if(Mathf.Abs(Input.GetAxisRaw("Mouse Y")) > 0.1f)
        {
            return true;
        }
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
        {
            return true;
        }
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.1f)
        {
            return true;
        }
        return false;
    }
    private void SetControllerControls() {
        // Setting axis for controller
        cinemachineCam.m_XAxis.m_InputAxisName = "Right Stick Horizontal";
        cinemachineCam.m_YAxis.m_InputAxisName = "Right Stick Vertical";


        // Controller keycode enums are offset when they are set in the editor this is to correct them (controller inputs here)
        // Setting inputs for controller
        inputStab = controllerStab + 4;
        inputSlash = controllerSlash + 4;
        inputDash = controllerDash + 4;
        inputJump = controllerJump + 4;
        inputShoot = controllerShoot + 4;
        inputDialogue = controllerDialogue + 4;
        inputInteract = controllerInteract + 4; // added interact

        //set the sensitivity of the camera with ControllerXsensitivity and ControllerYsensitivity
        if (canAbilityInput) {
            cinemachineCam.m_XAxis.m_MaxSpeed = ControllerXSensitivity;
            cinemachineCam.m_YAxis.m_MaxSpeed = ControllerYSensitivity;
        }

        currentControls = ControlType.controller;
    }

    private void SetMouseKeyboardControls() {
        // Setting axis for keyboard
        cinemachineCam.m_XAxis.m_InputAxisName = "Mouse X";
        cinemachineCam.m_YAxis.m_InputAxisName = "Mouse Y";

        // Setting inputs for keyboard
        inputStab = keyboardStab;
        inputSlash = keyboardSlash;

        //inputDownwardStab = keyboardDownwardStab;
        inputDash = keyboardDash;
        inputJump = keyboardJump;

        // added interact
        inputInteract = keyboardInteract;

        inputShoot = keyboardShoot + 7; // Keycode enums are offset when set in the editor (mouse inputs here)
        inputDialogue = keyboardDialogue + 7;

        //set the sensitivity of the camera with MKXSensitivity and MKYSensitivity
        if (canAbilityInput) {
            cinemachineCam.m_XAxis.m_MaxSpeed = MKXSensitivity;
            cinemachineCam.m_YAxis.m_MaxSpeed = MKYSensitivity;
        }
        currentControls = ControlType.mouseAndKeyboard;
    }

    public void SetMKSensitivity(float xSensitivity, float ySensitivity) {
        MKXSensitivity = xSensitivity;
        MKYSensitivity = ySensitivity;

        cinemachineCam.m_XAxis.m_MaxSpeed = MKXSensitivity;
        cinemachineCam.m_YAxis.m_MaxSpeed = MKYSensitivity;
    }

}
