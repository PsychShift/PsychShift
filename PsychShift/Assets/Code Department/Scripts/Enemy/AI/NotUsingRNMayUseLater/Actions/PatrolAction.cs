using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolAction", menuName = "PluggableAI/Actions/PatrolAction", order = 0)]
public class PatrolAction : Action
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.characterInfoReference.characterInfo.agent.destination = controller.wayPointList[controller.nextWayPoint];
        controller.characterInfoReference.characterInfo.agent.isStopped = false;

        if(controller.characterInfoReference.characterInfo.agent.remainingDistance
        < controller.characterInfoReference.characterInfo.agent.stoppingDistance
        && !controller.characterInfoReference.characterInfo.agent.pathPending)
        {
            controller.nextWayPoint = (controller.nextWayPoint + 1) % controller.wayPointList.Count;
        }
    }
}
