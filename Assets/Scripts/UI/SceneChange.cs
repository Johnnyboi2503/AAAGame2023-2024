using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour {

    [SerializeField] string sceneName;
    [SerializeField] float loadScreenTimerLength = 2f;

    public void ChangeScene() {
        TransitionUI.instance.StartTransition(loadScreenTimerLength, 
            () => // ON FINISHED FADE IN
            {
                SceneManager.LoadScene(sceneName);
            }, 
            () => // ON FINISHED FADE OUT
            {
                // Nothing needed
            });
    }
}
