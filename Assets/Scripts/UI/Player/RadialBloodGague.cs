using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialBloodGague : MonoBehaviour
{
    [SerializeField] BloodThirst bloodThirst;
    [SerializeField] EnergyBlast energyBlast;
    [SerializeField] Image radialFill;
    [SerializeField] Image blastCostBar;
    [SerializeField] Image overfedBar;
    [SerializeField] Image underfedBar;

    Material fill;
    Material blastCost;
    Material overfed;
    Material underfed;
    [SerializeField] float minVal;
    [SerializeField] float maxVal;
    [SerializeField] float barSize;

    private void Start() {
        // Getting Materials
        fill = radialFill.GetComponent<Image>().material;
        blastCost = blastCostBar.GetComponent<Image>().material;
        overfed = overfedBar.GetComponent<Image>().material;
        underfed = underfedBar.GetComponent<Image>().material;

        // Setting material values
        SetMaterialValue("_Rotation", minVal);
        SetMaterialValue("_EndPercentage", maxVal);
        blastCost.SetFloat("_BarSize", barSize);
        overfed.SetFloat("_BarSize", barSize);
        underfed.SetFloat("_BarSize", barSize);

        overfed.SetFloat("_Value", bloodThirst.maxBlood / bloodThirst.maxBloodForOverfed);
        underfed.SetFloat("_Value", bloodThirst.bloodThirstThreshold / bloodThirst.maxBloodForOverfed);

        bloodThirst.OnBloodChange.AddListener(UpdateBar);
    }
    public void UpdateBar() {
        fill.SetFloat("_Value", bloodThirst.currentBlood / bloodThirst.maxBloodForOverfed);
        blastCost.SetFloat("_Value", (bloodThirst.currentBlood-energyBlast.bloodPerShot)/ bloodThirst.maxBloodForOverfed);
    }

    private void SetMaterialValue(string key, float value) {
        fill.SetFloat(key, value);
        blastCost.SetFloat(key, value);
        overfed.SetFloat(key, value);
        underfed.SetFloat(key, value);
    }
}
