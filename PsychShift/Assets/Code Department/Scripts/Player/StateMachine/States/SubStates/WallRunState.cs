using System.Collections;
using UnityEngine;

/*
Stops when player stops moving or jumps
if the player looks in a different direction, they continue in the original direction.
clamp the player to the wall
*/
namespace Player
{
    public class WallRunState : IState
    {
        private float normalizedAngleThreshold = 0.1f;





        private readonly PlayerStateMachine playerStateMachine;
        private MonoBehaviour monoBehaviour;
        private CharacterInfo currentCharacter;
        private WallStateVariables wallVariables;
        public WallRunState(PlayerStateMachine playerStateMachine, MonoBehaviour monoBehaviour)
        {
            this.playerStateMachine = playerStateMachine;
            this.monoBehaviour = monoBehaviour;
            wallVariables = WallStateVariables.Instance;
        }


        private Vector3 wallNormal;
        private Vector3 wallForward;
        private float WallSpeed;

        private bool wallRight;
        public void Tick()
        {
            HandleMovement();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            monoBehaviour.StartCoroutine(SetNormal());
            this.WallSpeed = playerStateMachine.WallSpeed;
            Debug.Log("entered wallrun");
        }

        public void OnExit()
        {
            Debug.Log("exit wallrun");
            WallStateVariables.Instance.LastWallNormal = wallNormal;
            wallNormal = Vector3.zero; 
            wallForward = Vector3.zero;
        }

        private IEnumerator SetNormal()
        {
            yield return new WaitForSeconds(0.05f);

            wallRight = WallStateVariables.Instance.WallRight;
            wallNormal = wallRight ? WallStateVariables.Instance.RightWallNormal() : WallStateVariables.Instance.LeftWallNormal();
            wallForward = Vector3.Cross(wallNormal, currentCharacter.characterContainer.transform.up);
            Debug.Log(wallNormal);
            yield return null;
        }

        private void HandleMovement()
        {
            if ((currentCharacter.model.transform.forward - wallForward).magnitude > (currentCharacter.model.transform.forward - -wallForward).magnitude)
                wallForward = -wallForward;
            
            playerStateMachine.AppliedMovementX = wallForward.x * WallSpeed;
            //playerStateMachine.AppliedMovementY = wallForward.y;
            playerStateMachine.AppliedMovementZ = wallForward.z * WallSpeed;

        }
    }
}
