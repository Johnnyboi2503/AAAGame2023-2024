using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionUI : MonoBehaviour
{
    private static TransitionUI _instance;
    public static TransitionUI instance
    {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TransitionUI>();
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<GameObject>("TransitionUI")).GetComponent<TransitionUI>();
                    DontDestroyOnLoad(instance.transform.parent.gameObject);
                }
            }
            return _instance;
        }
    }

    private Animator _animator;

    private float pauseTimeLength;

    UnityEvent onFinishedFadeIn = new UnityEvent();
    UnityEvent onFinishedFadeOut = new UnityEvent();

    ///-////////////////////////////////////////////////////////////////
    ///
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    public void StartTransition(float argPauseTimeLength, UnityAction onFinishedFadeIn, UnityAction onFinishedFadeOut)
    {
        pauseTimeLength = argPauseTimeLength;
        this.onFinishedFadeIn.AddListener(onFinishedFadeIn);
        this.onFinishedFadeOut.AddListener(onFinishedFadeOut);

        StartFadeInAnimation();
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void StartFadeInAnimation()
    {
        Debug.Log("START FADE IN");
        _animator.Play("Fade_In");
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    public void FadeInAnimationCallback()
    {
        Debug.Log("FADE IN CALLBACK");
        onFinishedFadeIn.Invoke();
        StartCoroutine(StartPauseTimer());
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    /// Delays the screen from fading out
    ///
    private IEnumerator StartPauseTimer()
    {
        yield return new WaitForSeconds(pauseTimeLength);
        StartFadeOutAnimation();
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    private void StartFadeOutAnimation()
    {
        Debug.Log("START FADE OUT");
        _animator.Play("Fade_Out");
    }

    ///-////////////////////////////////////////////////////////////////
    ///
    public void FadeOutAnimationCallback()
    {
        Debug.Log("FINISH FADE OUT");
        onFinishedFadeOut.Invoke();
        Destroy(this.transform.parent.gameObject);
    }
}
