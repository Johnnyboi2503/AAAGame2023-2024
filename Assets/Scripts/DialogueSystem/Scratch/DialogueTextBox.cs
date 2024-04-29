using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueTextBox : MonoBehaviour
{
    // References
    [SerializeField]
    public TextMeshProUGUI nameText;
    [SerializeField]
    TextMeshProUGUI dialogue;
    [SerializeField]
    Image portrait;

    // Other Variables
    [SerializeField]
    float textSpeed;
    string fullText;
    public bool textComplete = true;

    public void InitalizeName(string _name) {
        // Initalize values
        if (_name != null) {
            nameText.text = _name;
        }
    }

    public void InitalizePortrait(Sprite _portrait) {
        // Initalize values
        if (_portrait != null) {
            portrait.sprite = _portrait;
        }
    }

    public void StartTyping(string _fullText) {
        // Completeing previous line
        CompleteLine();

        // Initalize values
        textComplete = false;
        dialogue.text = "";
        fullText = _fullText;

        // Starting typing
        StartCoroutine(TypeLine());
    }

    public void CompleteLine() {
        dialogue.text = fullText;
        textComplete = true;
        StopAllCoroutines();
    }

    IEnumerator TypeLine() {
        foreach (char c in fullText.ToCharArray()) {
            dialogue.text += c;
            if(dialogue.text == fullText) {
                textComplete = true;
            }
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
