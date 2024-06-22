using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnemyData {
    public Vector3 initalPosition { get; set; }
    public EnemyPrefab prefab { get; set; }
    public GameObject currentObject { get; set; }

    public LevelEnemyData(Vector3 initalPosition, EnemyPrefab prefab, GameObject currentObject) {
        this.initalPosition = initalPosition;
        this.prefab = prefab;
        this.currentObject = currentObject;
    }
}

public class LevelEnemyState : MonoBehaviour
{
    [SerializeField] public EnemyPrefab enemyPrefab; // A reference to the prefab of the enemy
    public Vector3 initalPosition; // Position at the start of the level
    bool canSaveState = true; // boolean to check if you can save enemy

    private void Start() {
        initalPosition = transform.position;
    }

    public bool CanSaveEnemy() {
        return canSaveState;
    }
}
