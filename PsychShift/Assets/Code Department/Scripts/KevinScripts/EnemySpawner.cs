using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Guns.Health;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    PuzzleKit godBoxRef;

    [SerializeField]
    GameObject[] enemyType;
    [SerializeField]
    bool randomEnemySpawn;
    [SerializeField]
    int whatEnemytoSpawn;//select int to only spawn an enemy of that type
    [SerializeField]
    int howManySpawn;
    /* [SerializeField]
    float spawnRate; */
    List<GameObject> enemySpawned;

    [SerializeField]
    bool elimEnemies;
    /* [SerializeField]
    bool waveSpawn; */
    [SerializeField]
    Vector2 gapBetweenSpawns;
    private bool puzzleNotDone= true;
    public PuzzleKit godBoxActionRef;
    int deathCount=0;

    private void Start() 
    {
        enemySpawned = new();
        StartCoroutine(Spawnwait());

    }

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
    private IEnumerator SpawnIn()
    {
        for(int i = 0; i<howManySpawn; i++)
        {
                if(randomEnemySpawn)
                    enemySpawned.Add(Instantiate(enemyType[Random.Range(0,enemyType.Length)], transform.position,Quaternion.identity));
                else 
                {
                   enemySpawned.Add(Instantiate(enemyType[whatEnemytoSpawn], transform.position,Quaternion.identity)); 
                }
                enemySpawned[enemySpawned.Count-1].GetComponent<EnemyHealth>().OnDeath += EnemyDeath;
                //PuzzleKit.PuzzleDone+=PuzzleFinished;
                yield return new WaitForSeconds(Random.Range(gapBetweenSpawns.x, gapBetweenSpawns.y));//gap between spawns 
        } 

    }
    private IEnumerator Spawnwait()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(SpawnIn());
    }

    /* IEnumerator spawnWave()
    {
        
        while(puzzleNotDone && enemySpawned.Count>0)
        {
            SpawnIn();
            yield return new WaitForSeconds(spawnRate);
        }
        
    } */
    private void EnemyDeath(Transform enemTransform)
    {
        if(godBoxRef!= null && godBoxRef.puzzleComplete)
                puzzleNotDone = false;
        if(puzzleNotDone == false)
        {
            if(enemySpawned.Count-1<=0)
            {
                godBoxActionRef.activateCount++;
                godBoxActionRef.ThisActivate();
            }
            
        }
        enemTransform.GetComponent<EnemyHealth>().OnDeath -= EnemyDeath;
        enemySpawned.Remove(enemTransform.gameObject);
        if(puzzleNotDone && enemySpawned.Count-1<=0)
        {
            StartCoroutine(SpawnIn());
            
        }
    }
    private void PuzzleFinished()
    {
        puzzleNotDone = false;
    }
}
