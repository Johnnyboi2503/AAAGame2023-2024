using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnlockableButton : MonoBehaviour
{
    [SerializeField] UnlockableData data;
    [SerializeField] Image image;
    [SerializeField] Button button; // Set interatable based on data time for unlockable
    [SerializeField] TMP_Text unlockInfo;

    [SerializeField] LoreDisplay loreDisplay;
    [SerializeField] StopwatchRecord record;

    private void OnEnable() {
        image.sprite = data.image;
        unlockInfo.text = data.sceneForUnlock + ": " + data.timeForUnlock;

        // Checking record if its complete
        if (record.stopwatchRecord.ContainsKey(data.sceneForUnlock)) {
            Debug.Log(record.stopwatchRecord[data.sceneForUnlock] + "---" + data.timeForUnlock);
            if(record.stopwatchRecord[data.sceneForUnlock] <= data.timeForUnlock) {
                button.interactable = true;
            }
            else { 
                button.interactable = false; 
            }
        }
        else { button.interactable = false; }
    }

    public void OnButtonPress() {
        loreDisplay.SetDisplay(data);
    }
}
