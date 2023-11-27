using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System.Collections;

namespace Player
{
    public class WallRunState : IState
    {
        private float normalizedAngleThreshold = 0.1f;

        private readonly PlayerStateMachine playerStateMachine;
        private MonoBehaviour monoBehaviour;
        private CharacterInfo currentCharacter;
        private WallStateVariables wallVariables;
        private CinemachineTiltExtension cameraTilt;

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
            if(WallStateVariables.Instance.WallRight)
                cameraTilt.SetTiltDirection(20f);
            else if(WallStateVariables.Instance.WallLeft)
                cameraTilt.SetTiltDirection(-20f);
    
            HandleMovement();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            cameraTilt = currentCharacter.vCam.GetComponent<CinemachineTiltExtension>();
            monoBehaviour.StartCoroutine(SetNormal());
            this.WallSpeed = playerStateMachine.WallSpeed;
        }

        public void OnExit()
        {
            WallStateVariables.Instance.LastWallNormal = wallNormal;
            wallNormal = Vector3.zero; 
            wallForward = Vector3.zero;
            cameraTilt.SetTiltDirection(0f);
        }

        private IEnumerator SetNormal()
        {
            yield return new Unity.VisualScripting.WaitForNextFrameUnit();

            wallRight = WallStateVariables.Instance.WallRight;
            wallNormal = wallRight ? WallStateVariables.Instance.RightWallNormal() : WallStateVariables.Instance.LeftWallNormal();
            wallForward = Vector3.Cross(wallNormal, currentCharacter.characterContainer.transform.up);
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

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }
    }
}
