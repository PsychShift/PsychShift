using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guns.Health;
using Guns;
using System;
using Unity.VisualScripting;
public class EnemySpawner : MonoBehaviour
{
    public static System.Random Rand = new System.Random();
    [Header ("This is to attach a puzzle to stop the spawner when the puzzle is done")]
    [SerializeField]
    PuzzleKit godBoxRef;    
    //[SerializeField] GameObject[] enemyType;

    [SerializeField] private List<EBrainType> enemyTypes;
    [SerializeField] private List<GunScriptableObject> gunTypes;
    [SerializeField] private EnemyModifierDictionary enemyModifiers;
    [SerializeField] private List<AIAgression> agressionLevels;

    //[SerializeField] bool randomEnemySpawn;
    //[SerializeField] int whatEnemytoSpawn;//select int to only spawn an enemy of that type

    [SerializeField] int howManySpawn;
    [SerializeField] float howLongBeforeRespawns = 20f;
    private List<GameObject> enemySpawned;

    
    [SerializeField] Vector2 gapBetweenSpawns;
    //[Header("TURN THIS OFF MANUALLY FOR KILL EVERYONE SECTIONS")]
    private bool puzzleNotDone= true;
    [Header("Turn this on for killEveryone sections")]
    public bool killEveryone;
    [Header("This is to send a message to god box that a spawner has been cleared")]
    public PuzzleKit godBoxActionRef;
    int deathCount=0;
    public ParticleSystem spawnFX;
    

    private Vector3 stationaryPos;
    private Vector3 randomPos;
    private Vector3 chasePos;
    private Vector3[] patrolPositions;
    

    private void Start()
    {
        Debug.Log("spawning");
        enemySpawned = new();
        SetupSpawnerInfo();
        StartCoroutine(SpawnIn(UnityEngine.Random.Range(0f, 2.5f)));
    }
    private void SetupSpawnerInfo()
    {
        bool hasStationary = TryGetComponent(out StationarySpawnerInfo stationarySpawnerInfo);
        bool hasChase = TryGetComponent(out ChaseSpawnerInfo chaseSpawnerInfo);
        bool hasRandom = TryGetComponent(out RandomSpawnerInfo randomSpawnerInfo);
        bool hasPatrol = TryGetComponent(out PatrolSpawnerInfo patrolSpawnerInfo);

        if(hasStationary)
        {
            if(stationarySpawnerInfo.GuardPosition != null) 
                stationaryPos = stationarySpawnerInfo.GuardPosition.position;
        } 
        if(hasChase)
        {
            if(chaseSpawnerInfo.GoHereBeforeStarting != null)
                chasePos = chaseSpawnerInfo.GoHereBeforeStarting.position;
        }
        if(hasRandom)
        {
            if(randomSpawnerInfo.GoHereBeforeStarting != null)
                randomPos = randomSpawnerInfo.GoHereBeforeStarting.position;
        }
        if(hasPatrol)
        {
            int len = patrolSpawnerInfo.PathToFollow.Length;
            if(len > 0)
            {
                patrolPositions = new Vector3[len];

                for(int i = 0; i < len; i++)
                {
                    patrolPositions[i] = patrolSpawnerInfo.PathToFollow[i].position;
                    Debug.Log(patrolPositions[i]);
                }

                
            }
        }
    }

    private IEnumerator SpawnIn(float time = -1)
    {
        /* wait for respawn check, if youre down enemies respawn, else wait again to check
        pick from the lists :
            enemyTypes
            gunTypes
            enemyModifier
        instantiate the enemy prefab
        get the EnemyBrainSelector component 
        call swap brain
        */

        float t = time == -1 ? howLongBeforeRespawns : time;
        yield return new WaitForSeconds(t);
        int numToSpawn = Math.Abs(enemySpawned.Count - howManySpawn);

        for(int i = 0; i < numToSpawn; i++)
        {
            GameObject enemy = Instantiate(GameAssets.Instance.EnemyPrefab, transform.position, Quaternion.identity);
            // Choose enemy type stuff
            EBrainType brain = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)];

            GunScriptableObject gun = gunTypes[UnityEngine.Random.Range(0, gunTypes.Count)];

            List<EEnemyModifier> selectedModifiers = new();
            foreach(var mod in enemyModifiers)
            {
                int val = Rand.Next(100);
                int chance = mod.Value;
                if(val < chance)
                {
                    selectedModifiers.Add(mod.Key);
                }
            }

            AIAgression agression = agressionLevels[UnityEngine.Random.Range(0, agressionLevels.Count)];

            EEnemyModifier eMod = selectedModifiers.Count > 0 ? selectedModifiers[0] : EEnemyModifier.None;
            // Set enemy type
            EnemyBrainSelector selector = enemy.GetComponent<EnemyBrainSelector>();
            selector.SwapBrain(gun, brain, eMod, agression);

            // subscribe to death event
            enemy.GetComponent<EnemyHealth>().OnDeath += EnemyDeath;
            enemySpawned.Add(enemy);

            string gunTypeName = selector.GunName(gun);
            string modifierName = selector.ModifierName(eMod);

            enemy.name = gunTypeName + modifierName + "_EnemyModel";

            switch (brain)
            {
                case EBrainType.Stationary :
                Debug.Log("stationary setup from spawner");
                enemy.GetComponent<StationaryBrain>().SpawnerSetup(stationaryPos);
                break;
                case EBrainType.Chase :
                enemy.GetComponent<ChaseBrain>().SpawnerSetup(chasePos);
                break;
                case EBrainType.Random :
                enemy.GetComponent<RandomBrain>().SpawnerSetup(randomPos);
                break;
                case EBrainType.Patrol :
                enemy.GetComponent<PatrolBrain>().SpawnerSetup(patrolPositions);
                break;
            }
            
            // Special effects on spawn
            Instantiate(spawnFX, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(UnityEngine.Random.Range(gapBetweenSpawns.x, gapBetweenSpawns.y));//gap between spawns 
        }
    } 

    
    /* private IEnumerator SpawnIn()
    {
        yield return new WaitForSeconds(spawnRate);
        for(int i = 0; i<howManySpawn; i++)
        {
                if(randomEnemySpawn)
                    enemySpawned.Add(Instantiate(enemyType[UnityEngine.Random.Range(0,enemyType.Length)], transform.position,Quaternion.identity));
                else 
                {
                   enemySpawned.Add(Instantiate(enemyType[whatEnemytoSpawn], transform.position,Quaternion.identity)); 
                }
                Instantiate(spawnFX,transform.position, Quaternion.identity);
                enemySpawned[enemySpawned.Count-1].GetComponent<EnemyHealth>().OnDeath += EnemyDeath;
                //PuzzleKit.PuzzleDone+=PuzzleFinished;
                
        } 
    } */
    
    private void EnemyDeath(Transform enemTransform)
    {
        #region this is for infinite spawn until puzzle done
        //Debug.Log("ACTIVATED");
        //IF THERE IS A GODBOX ATTACHED ENEMIES WILL SPAWN UNTIL THAT PUZZLE IS COMPLETE
        if(godBoxRef!= null && godBoxRef.puzzleDone)
        {
            puzzleNotDone = false;//means puzzle issa finished
        }
        //has a godbox and the godbox completed the puzzle 
                
        #endregion

        #region this is for killing everyone and then activating the godbox 
        //IF THERE IS AN ACTION 
        if(killEveryone)//if the puzzle is finished ,Activates godBox
        {
            if(enemySpawned.Count-2<=0)//WAS ONE MADE -2 FOR TEMP FIX
            {
                godBoxActionRef.activateCount++;
                
                godBoxActionRef.ThisActivate();
            }
            
        }

        #endregion
        enemTransform.GetComponent<EnemyHealth>().OnDeath -= EnemyDeath;
        enemySpawned.Remove(enemTransform.gameObject);
        if(puzzleNotDone && enemySpawned.Count-2<=0)//WAS ONE MADE -2 FOR TEMP FIX THIS IS TO KEEP SPAWNING ENEMIES IF THE PUZZLE IS NOT DONE
        {
            StartCoroutine(SpawnIn());
        }
    }
    void OnDestroy()
    {
        foreach(var enemy in enemySpawned)
        {
            enemy.GetComponent<EnemyHealth>().OnDeath -= EnemyDeath;
        }
        enemySpawned.Clear();
    }
    /* private void PuzzleFinished()
    {
        puzzleNotDone = false;
    } */

    public void UpdateComponents()
    {
        bool hasStationary = TryGetComponent(out StationarySpawnerInfo stationarySpawnerInfo);
        bool hasChase = TryGetComponent(out ChaseSpawnerInfo chaseSpawnerInfo);
        bool hasRandom = TryGetComponent(out RandomSpawnerInfo randomSpawnerInfo);
        bool hasPatrol = TryGetComponent(out PatrolSpawnerInfo patrolSpawnerInfo);

        if (enemyTypes.Contains(EBrainType.Stationary))
        {
            if (!hasStationary)
                gameObject.AddComponent<StationarySpawnerInfo>();
        }
        else
        {
            if (hasStationary)
                DestroyImmediate(stationarySpawnerInfo);
        }

        if (enemyTypes.Contains(EBrainType.Chase))
        {
            if (!hasChase)
                gameObject.AddComponent<ChaseSpawnerInfo>();
        }
        else
        {
            if (hasChase)
                DestroyImmediate(chaseSpawnerInfo);
        }

        if (enemyTypes.Contains(EBrainType.Random))
        {
            if (!hasRandom)
                gameObject.AddComponent<RandomSpawnerInfo>();
        }
        else
        {
            if (hasRandom)
                DestroyImmediate(randomSpawnerInfo);
        }

        if (enemyTypes.Contains(EBrainType.Patrol))
        {
            if (!hasPatrol)
                gameObject.AddComponent<PatrolSpawnerInfo>();
        }
        else
        {
            if (hasPatrol)
                DestroyImmediate(patrolSpawnerInfo);
        }
    }
    //[SerializeField]
    //bool elimEnemies;
    /* [SerializeField]
    bool waveSpawn; */
    //private void Update()
    //{
        //Choose what enemy to spawn
        
        /* if(elimEnemies)
        {
            if(enemySpawned.Count<=0)
            {
                SpawnIn();
            }
        }
        else if(waveSpawn)
        {
            
        } */
            
    //}
    /* private IEnumerator Spawnwait()
    {
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnIn());
    } */
    /* IEnumerator spawnWave()
    {
        
        while(puzzleNotDone && enemySpawned.Count>0)
        {
            SpawnIn();
            yield return new WaitForSeconds(spawnRate);
        }
        
    } */
}

[Serializable]
public class EnemyModifierDictionary : SerializableDictionary<EEnemyModifier, int> { }
