using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthEffectController : MonoBehaviour {
   
    public Volume volume;
    public Killable killable;
    [SerializeField] AnimationCurve curve;

    [Header("Vignette")]
    private Vignette vignette;

    public float vignetteStart = 0.0f; 
    public float vignetteMax = 1.0f;    
    public bool vignetteToggle;

    [Header("Film Grain")]  
    private FilmGrain filmgrain;

    public float filmgrainStart = 0.0f;
    public float filmgrainMax = 1.0f;
    public bool filmgrainToggle;

    [Header("Panini Projection")]  
    private PaniniProjection paniniprojection;

    public float distanceStart = 0.0f;
    public float distanceMax = 1.0f;
    public bool distanceToggle;

    public float cropToFitStart = 0.0f;
    public float cropToFitMax = 1.0f;
    public bool cropToFitToggle;

    [Header("Lens Distortion")]  
    private LensDistortion lensdistortion;

    public float lensdistortionStart = 0.0f;
    public float lensdistortionMax = -1.0f;  
    public bool lensDistortionToggle;

    [Header("Bloom")]  
    private Bloom bloom;

    public float bloomStart = 0.0f;
    public float bloomMax = 10.0f;
    public bool bloomToggle;

    [Header("Chromatic Aberration")]
    private ChromaticAberration chromaticaberration;

    public float chromaticAberrationStart = 0.0f;
    public float chromaticAberrationMax = 1.0f;
    public bool chromaticAberrationToggle;

    void Start()
    {

        killable.OnHealthChange.AddListener(OnHealthChange);

        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<FilmGrain>(out filmgrain);
        volume.profile.TryGet<PaniniProjection>(out paniniprojection);
        volume.profile.TryGet<LensDistortion>(out lensdistortion);
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<ChromaticAberration>(out chromaticaberration);
    }


    void OnHealthChange() {
        UpdateVisualEffect(killable.currentHP, killable.maxHP);
    }


    void UpdateVisualEffect(float currentHealth, float maxHealth)
    {
        float healthPercentage = currentHealth / maxHealth;
        float healthProc = 1.0f - healthPercentage;

        //EFFECTS VIGNETTE
        if(vignetteToggle && vignette != null) {
            float vignetteValue = Mathf.Lerp(vignetteStart, vignetteMax, curve.Evaluate(healthProc));
            vignette.intensity.Override(vignetteValue);
        }

        //EFFECTS FILM GRAIN
        if(filmgrainToggle && filmgrain != null) {
            float filmgrainValue = Mathf.Lerp(filmgrainStart, filmgrainMax, curve.Evaluate(healthProc));
            filmgrain.intensity.Override(filmgrainValue);
        }

        //EFFECTS DISTANCE 
        if(distanceToggle && paniniprojection != null) {
            float distanceValue = Mathf.Lerp(distanceStart, distanceMax, curve.Evaluate(healthProc));
            paniniprojection.distance.Override(distanceValue);
        }
   
        //EFFECTS CROP TO FIT(ONLY WORKS WHENEVER THERE IS A DISTANCE OTHERWISE NO EFFECT)
        if(cropToFitToggle && paniniprojection != null) {
            float cropToFitValue = Mathf.Lerp(cropToFitStart, cropToFitMax, curve.Evaluate(healthProc));
            paniniprojection.cropToFit.Override(cropToFitValue);
        }
   
        //EFFECTS LENS DISTORTION
        if(lensDistortionToggle && lensdistortion != null) {
            float lensdistortionValue = Mathf.Lerp(lensdistortionStart, lensdistortionMax, curve.Evaluate(healthProc));
            lensdistortion.intensity.Override(lensdistortionValue);
        }
   
        //EFFECTS BLOOM
        if(bloomToggle && bloom != null) {
            float bloomValue = Mathf.Lerp(bloomStart, bloomMax, curve.Evaluate(healthProc));
            bloom.intensity.Override(bloomValue);
        }

        //EFFECTS CHROMATIC ABERRATION
        if(chromaticAberrationToggle && chromaticaberration != null) {
            float chromaticaberrationValue = Mathf.Lerp(chromaticAberrationStart, chromaticAberrationMax, curve.Evaluate(healthProc));
            chromaticaberration.intensity.Override(chromaticaberrationValue);
        }
    }
}
