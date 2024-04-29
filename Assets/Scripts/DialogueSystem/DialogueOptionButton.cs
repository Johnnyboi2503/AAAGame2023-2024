using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DialogueOptionButton : MonoBehaviour
{
    public TMP_Text displayText;
    public Button button;

    public int assignedIndex;

    public DialogueOptionsUI optionUI;

    private void Start() {
        button.onClick.AddListener(PassButtonToUI);
    }

    private void PassButtonToUI() {
        optionUI.ClickButton(this);
    }
}
