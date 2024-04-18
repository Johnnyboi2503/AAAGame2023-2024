using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    public string characterName;
    [SerializeField]
    Sprite idle;
    [SerializeField]
    Sprite happy;
    [SerializeField]
    Sprite sad;
    [SerializeField]
    Sprite angry;
    [SerializeField]
    Sprite suprised;

    public Sprite GetSprite(string key) {
        key = key.ToLower();
        switch(key) {
            case "idle":
                return idle;
            case "happy":
                return happy;
            case "sad":
                return sad;
            case "angry":
                return angry;
            case "suprised":
                return suprised;
        }
        return null;
    }
}
