using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueOptionsUI : MonoBehaviour
{
    [SerializeField]
    GameObject button;

    public void InitalizeButtons(List<string> buttonText, List<int> buttonIndex) {
        for (int i = 0; i < buttonText.Count; i++) {
            GameObject currentButton = Instantiate(button, transform);

            // Initalizing buttons
            if(currentButton.TryGetComponent<DialogueOptionButton>(out DialogueOptionButton dialogueOptionButton)) {
                dialogueOptionButton.displayText.text = "> "+buttonText[i];
                dialogueOptionButton.assignedIndex = buttonIndex[i];
                dialogueOptionButton.optionUI = this;
            }
        }
    }

    public void DestroyButtons() {
        // Destorying all the children on this object
        for(int i = transform.childCount-1; i >= 0; i--) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void ClickButton(DialogueOptionButton currentButton) {
        // Assigning index for dialogue manager
        DialogueManager manager = FindAnyObjectByType<DialogueManager>();
        manager.decisionIndex = currentButton.assignedIndex;
        manager.selectedOption = true;
        AudioManager.GetInstance().PlayGlobalAudio("UISelectSFX", manager.dialogueVolume);
    }
}
