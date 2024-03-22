using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[System.Serializable]
public class MoveStateMB : IState
{
    public float minSqrMagFromTarget;
    public float alwaysRecheckPathAtTime = 1f;
    public float targetToCloseRecheckPathTime = 0.25f;
    public NavMeshSurface surface;


    HangingRobotController controller;
    HangingAnimatorController animController;


    private float longTimeCheck;
    private float shortTimeCheck;
    
    public MoveStateMB(HangingRobotController controller, HangingAnimatorController animController, NavMeshSurface surface, float alwaysRecheckPathAtTime, float targetToCloseRecheckPathTime)
    {
        this.controller = controller;
        this.animController = animController;
        this.surface = surface;
        this.alwaysRecheckPathAtTime = alwaysRecheckPathAtTime;
        this.targetToCloseRecheckPathTime = targetToCloseRecheckPathTime;
    }

    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        float time = Time.time;
        longTimeCheck = time + alwaysRecheckPathAtTime;
        shortTimeCheck = time + targetToCloseRecheckPathTime; 
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if(ShouldCalculatePath())
        {
            Bounds bounds = surface.navMeshData.sourceBounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            Vector3 randomPos = new Vector3(Random.Range(min.x, max.x), min.y, Random.Range(min.z, max.z));

            controller.agent.SetDestination(randomPos);
        }
    }

    public bool ShouldCalculatePath()
    {
        float time = Time.time;
        if(time < shortTimeCheck)
        {
            return false;
        }
        else
        {
            shortTimeCheck = time + targetToCloseRecheckPathTime;
            Vector3 dif = controller.transform.position - controller.target.position;
            dif.y = 0;
            float sqrMagFromTarget = dif.sqrMagnitude;
            if (sqrMagFromTarget < minSqrMagFromTarget)
            {
                return true;
            }
            if(time > longTimeCheck)
            {
                longTimeCheck = time + alwaysRecheckPathAtTime;
                return true;
            }
        }

        return false;        
    }
}
