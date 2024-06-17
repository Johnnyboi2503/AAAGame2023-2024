using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState {
    public List<BloodOrbSpawnPoint> bloodOrbSpawners;
    public List<LevelEnemyData> enemyStates;
    public List<SpeedEffect> playerSpeedEffects;

    public LevelState(List<BloodOrbSpawnPoint> _bloodOrbSpawners, List<LevelEnemyData> _enemyStates, List<SpeedEffect> _playerSpeedEffects) {
        bloodOrbSpawners = _bloodOrbSpawners;
        enemyStates = _enemyStates;
        playerSpeedEffects = _playerSpeedEffects;
    }
}

// Used for saving and instantiating enemies
public enum EnemyPrefab {
    MELEE,
    RANGED,
    BOMB,
    FLICK
}

public class LevelStateController : MonoBehaviour {
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>(); // References to prefabs index follows the order of the enum defition
    LevelState recentState;
    [SerializeField] float initalSaveTimer = 0.1f;

    private void Start() {
        FindObjectOfType<PlayerKillable>().OnDie.AddListener(LoadLevelState);
        Invoke("SaveLevelState", initalSaveTimer);
    }

    public void SaveLevelState() {
        Debug.Log("save state");
        recentState = new LevelState(SaveBloodOrbState(), SaveEnemyLevelState(), SavePlayerSpeedEffect());
    }

    public void LoadLevelState() {
        ClearWorldState();
        LoadBloodOrbs();
        LoadLevelEnemyState();
        LoadPlayerSpeed();
    }

    private void ClearWorldState() {
        ClearBloodOrbs();
        ClearPlayerSpeedEffect();
    }

    private List<BloodOrbSpawnPoint> SaveBloodOrbState() {
        BloodOrbSpawnPoint[] orbSpawnPoints = FindObjectsOfType<BloodOrbSpawnPoint>();
        List<BloodOrbSpawnPoint> output = new List<BloodOrbSpawnPoint>();

        foreach (BloodOrbSpawnPoint orbSpawnPoint in orbSpawnPoints) {
            if (orbSpawnPoint.HasBloodOrb()) {
                output.Add(orbSpawnPoint);
            }
        }

        return output;
    }
    private List<LevelEnemyData> SaveEnemyLevelState() {
        LevelEnemyState[] enemies = FindObjectsOfType<LevelEnemyState>();
        List<LevelEnemyData> output = new List<LevelEnemyData>();

        foreach (LevelEnemyState enemy in enemies) {
            if (enemy.CanSaveEnemy()) {
                output.Add(new LevelEnemyData(enemy.initalPosition, enemy.enemyPrefab, enemy.gameObject));
            }
        }

        return output;
    }

    private List<SpeedEffect> SavePlayerSpeedEffect() {
        List<SpeedEffect> currentPlayerSpeedEffect = FindAnyObjectByType<MovementModification>().speedEffects;
        List<SpeedEffect> output = new List<SpeedEffect>();

        foreach (SpeedEffect effect in currentPlayerSpeedEffect) {
            output.Add(new SpeedEffect(effect.duration, effect.percentSpeed));
        }

        return output;
    }

    private void ClearBloodOrbs() {
        // Clearing orbs from spawners
        BloodOrbSpawnPoint[] orbSpawnPoints = FindObjectsOfType<BloodOrbSpawnPoint>();
        foreach (BloodOrbSpawnPoint orbSpawnPoint in orbSpawnPoints) {
            orbSpawnPoint.ClearBloodOrb();
        }

        // Clearing stray blood orbs
        BloodOrb[] bloodOrbs = FindObjectsOfType<BloodOrb>();
        foreach(BloodOrb bloodOrb in bloodOrbs) {
            Destroy(bloodOrb.gameObject);
        }
    }
    private void ClearPlayerSpeedEffect() {
        FindAnyObjectByType<MovementModification>().speedEffects.Clear();
    }

    private void LoadBloodOrbs() {
        foreach(BloodOrbSpawnPoint spawnPoint in recentState.bloodOrbSpawners) {
            spawnPoint.ResetBloodOrb();
        }
    }

    private void LoadPlayerSpeed() {
        MovementModification movement = FindAnyObjectByType<MovementModification>();
        foreach(SpeedEffect effect in recentState.playerSpeedEffects) {
            movement.speedEffects.Add(effect);
        }
    }

    private void LoadLevelEnemyState() {
        for(int i = 0; i  < recentState.enemyStates.Count; i++) {
            LevelEnemyData currentData = recentState.enemyStates[i];

            if (currentData.currentObject == null) {
                // Instantiate enemy if they were destroyed already
                currentData.currentObject = Instantiate(GetEnemyPrefab(currentData.prefab), currentData.initalPosition, Quaternion.identity);
            }
            else {
                currentData.currentObject.transform.position = currentData.initalPosition; // Moving enemy if they are not destoryed
            }
        }
    }

    private GameObject GetEnemyPrefab(EnemyPrefab prefab) {
        switch(prefab) {
            case EnemyPrefab.MELEE:
                return enemyPrefabs[0];
            case EnemyPrefab.RANGED:
                return enemyPrefabs[1];
            case EnemyPrefab.BOMB:
                return enemyPrefabs[2];
            case EnemyPrefab.FLICK:
                return enemyPrefabs[3];
        }
        return null;
    }
}
