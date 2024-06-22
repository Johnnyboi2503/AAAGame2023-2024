using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/UnlockableData", order = 1)]
public class UnlockableData : ScriptableObject
{
    [SerializeField] public Sprite image;
    [SerializeField] public string title;
    [TextArea]
    [SerializeField] public string loreText;
    [SerializeField] public string sceneForUnlock;
    [SerializeField] public float timeForUnlock;
}
