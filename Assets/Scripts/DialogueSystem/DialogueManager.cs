using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public bool completedLine = true;
    public bool selectedOption = false;

    public int decisionIndex;

    [SerializeField]
    public DialogueTextBox textBox;
    [SerializeField]
    public DialogueOptionsUI optionsUI;
    [SerializeField]
    List<CharacterData> characterDatas = new List<CharacterData>();

    private bool dialogueInterupt = false;

    public void StartDialogue(DialogueInteraction dialogueInteraction, UnityEvent actionAfterDialogue = null)
    {
        if (!DialogueIsActive())
        {
            // Enabling cursor
            FindAnyObjectByType<PlayerInput>().DisableAbilityInput();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Initalize values
            completedLine = false;
            selectedOption = false;
            dialogueInterupt = false;
            textBox.gameObject.SetActive(true);

            StartCoroutine(RunDialogue(dialogueInteraction.dialogues, actionAfterDialogue));
        }
    }

    public void DialougeAdvanceInput() {
        // If the text box is active advance the dialogue
        if (DialogueIsActive()) {
            if (textBox.textComplete && !optionsUI.isActiveAndEnabled) {
                completedLine = true;
            }
            else {
                textBox.CompleteLine();
            }
        }
    }
    public void DialogueSkipInput() {
        if (DialogueIsActive()) {
            dialogueInterupt = true;
            selectedOption = true;
            completedLine = true;
        }
    }

    private IEnumerator RunDialogue(string[][] dialogues, UnityEvent actionAfterDialogue = null) {
        // Variables for decision format
        int endIndex = 0;
        bool inDecision = false;

        for (int i = 0; i < dialogues.Length; i++)
        {
            if(dialogueInterupt) { break; } // Dialogue interupt that ends dialogue interaction

            // Saving data to simplify referencing
            string rowName = dialogues[i][0];
            string rowDialogue = dialogues[i][1];
            string rowPose = dialogues[i][2];

            // When finding the start of a decision dialogue start process
            if (rowName == "_StartDecision") {
                // Searching and setting data in decision dialogue
                List<string> buttonText = new List<string>();
                List<int> buttonIndex = new List<int>();
                for(int j = i; j < dialogues.Length; j++) {
                    if(dialogues[j][0] == "_Decision") {
                        buttonText.Add(dialogues[j][1]);
                        buttonIndex.Add(j);
                    }
                    if(dialogues[j][0] == "_EndDecision") {
                        endIndex = j;
                        break;
                    }
                }

                // Activating UI element + initalziing buttons
                optionsUI.gameObject.SetActive(true);
                optionsUI.InitalizeButtons(buttonText, buttonIndex);

                yield return new WaitUntil(() => selectedOption);

                // After player has pressed a button start dialogue
                selectedOption = false;
                inDecision = true;
                i = decisionIndex;

                optionsUI.DestroyButtons();
                optionsUI.gameObject.SetActive(false);

                if (dialogueInterupt) { break; } // Dialogue interupt that ends dialogue interaction
            }
            else {
                // If in a decision dialogue end it once you find the end of them
                if (inDecision && rowName == "_Decision" || rowName == "_EndDecision") {
                    i = endIndex;
                    inDecision = false;
                }
                else {
                    // Setting text name
                    string dialogueName = null;
                    if (rowName != "") {
                        dialogueName = rowName;
                    }
                    textBox.InitalizeName(dialogueName);

                    // Setting portrait
                    Sprite poseSprite = null;
                    if (rowPose != "" && rowPose != " ") {
                        // Seaching and assigning character data
                        foreach (CharacterData characterData in characterDatas) {
                            if (characterData.characterName == textBox.nameText.text) {
                                poseSprite = characterData.GetSprite(rowPose);
                                break;
                            }
                        }
                    }
                    textBox.InitalizePortrait(poseSprite);

                    // Start typing
                    textBox.StartTyping(rowDialogue);
                    yield return new WaitUntil(() => completedLine);
                    completedLine = false;

                    if (dialogueInterupt) { break; } // Dialogue interupt that ends dialogue interaction
                }
            }
        }

        // Disabling cursor
        FindAnyObjectByType<PlayerInput>().EnableAbilityInput();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Disabling textbox once dialogue is finished
        optionsUI.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);

        if (actionAfterDialogue != null)
        {
            actionAfterDialogue.Invoke();
        }
    }

    public bool DialogueIsActive() {
        return textBox.isActiveAndEnabled;
    }
}
