using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class StateController : MonoBehaviour
{
    public State currentState;

    [HideInInspector] public Transform eyes;

    [HideInInspector] public List<Vector3> wayPointList;
    [HideInInspector] public int nextWayPoint;

    [HideInInspector] public CharacterInfoReference characterInfoReference;



    private bool aiActive;
    
    void Awake()
    {
        characterInfoReference = GetComponent<CharacterInfoReference>();
        eyes = characterInfoReference.characterInfo.cameraRoot;

        wayPointList = new List<Vector3>()
        {
            transform.position,
            transform.position + new Vector3(0, 0, 5),
            transform.position + new Vector3(5, 0, 5),
            transform.position + new Vector3(5, 0, 0)
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!aiActive)
            return;
        
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        if(currentState != null && eyes != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(eyes.position, 1);
        }
    }
}
