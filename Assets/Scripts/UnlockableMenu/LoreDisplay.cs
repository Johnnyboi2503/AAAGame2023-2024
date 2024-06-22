using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LoreDisplay : MonoBehaviour {
    [SerializeField] Image image;
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text title;
    public void SetDisplay(UnlockableData data) {
        image.sprite = data.image;
        text.text = data.loreText;
        title.text = data.title;
    }
}
