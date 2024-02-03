using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIVigilScuff : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               
    public float startWaitTime = 4;                 
    public float timeToRotate = 2;                  
    public float speedWalk = 6;                     
    public float speedRun = 9;
    public float robotDown = 4.5f;
    float robotDownOG;
    float speedRunOG;
    
    float speedWalkOG;                      
 
    public float viewRadius = 15;                   
    public float viewAngle = 90;                    
    public LayerMask playerMask;                    
    public LayerMask obstacleMask;                  
    public float meshResolution = 1.0f;             
    public int edgeIterations = 4;                  
    public float edgeDistance = 0.5f;               
 
 
    public Transform[] waypoints;                   
    int m_CurrentWaypointIndex;                     
 
    Vector3 playerLastPosition = Vector3.zero;      
    Vector3 m_PlayerPosition;                       
 
    float m_WaitTime;                               
    float m_TimeToRotate;                           
    bool m_playerInRange;                           
    bool m_PlayerNear;                              
    bool m_IsPatrol;                                
    bool m_CaughtPlayer;  
    bool shutDown = false;                          
 
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                 
        m_TimeToRotate = timeToRotate;
        speedRunOG = speedRun;
        speedWalkOG = speedWalk;
        robotDownOG = robotDown;
 
        m_CurrentWaypointIndex = 0;                 
        navMeshAgent = GetComponent<NavMeshAgent>();
 
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    
    }
 
    private void Update()
    {
        EnviromentView();                       
        if(shutDown == false)
        {
            if (!m_IsPatrol)
            {
                Chasing();
            }
            else
            {
                Patrolling();
            }
        }
        else
        {
            ShutDown();
        }
    }
 
    private void Chasing()
    {
        
        m_PlayerNear = false;                       
        playerLastPosition = Vector3.zero;          
        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);          
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    
        {
                if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
 
    private void Patrolling()
    {
        if (m_PlayerNear)
        {
            
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
 
    private void OnAnimatorMove()
    {
 
    }
 
    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
 
    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
 
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
 
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }
 
    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }
    public void ShutDown()
    {
        Debug.Log("Shutting Down");
        if(robotDown>=0)
        {
            shutDown = true;
            Stop();
            robotDown-=Time.deltaTime;
        }
        else
        {
            Move(speedWalk);
            //navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            shutDown = false;
            robotDown = robotDownOG;
            
        }
        
        /*
        speedRun = speedRunOG;
        speedWalk = speedWalkOG;
        shutDown = false;*/

    }
 
    void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;             
                    m_IsPatrol = false;                 
                }
                else
                {

                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
 
                m_playerInRange = false;                
            }
            if (m_playerInRange)
            {
 
                m_PlayerPosition = player.transform.position;       
            }
        }
    }
}
