using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralEnvironment : MonoBehaviour
{
    public GameObject[] LevelLayouts;
    public GameObject[] structuresLeft;
    public GameObject[]structuresLeftFront;
    public GameObject[] structuresRight;
    public GameObject[] structuresRightFront;
    public GameObject[] structuresMiddle;
    public GameObject[] structuresMiddleFront;
    //public GameObject spawners;//spawn these on set spots 
    //potentially make a script that handles spawners outside of this script.
    //Make this script to only build the environment

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(LevelLayouts[Random.Range(0, LevelLayouts.Length)]);
        Instantiate(structuresLeft[Random.Range(0,structuresLeft.Length)]);
        Instantiate(structuresLeftFront[Random.Range(0,structuresLeftFront.Length)]);
        //Instantiate(structuresMiddle[Random.Range(0,structuresMiddle.Length)]);
        //Instantiate(structuresMiddleFront[Random.Range(0,structuresMiddleFront.Length)]);
        Instantiate(structuresRight[Random.Range(0,structuresRight.Length)]);
        Instantiate(structuresRightFront[Random.Range(0,structuresRightFront.Length)]);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
