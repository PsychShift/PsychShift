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
        //private CinemachineClampedYRotationExtension cameraClamp;

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
            {
                cameraTilt.SetTiltDirection(true);
                //currentCharacter.vCam.m_Lens.Dutch = 20f;
            }
            else if(WallStateVariables.Instance.WallLeft)
            {
                cameraTilt.SetTiltDirection(false);
                //currentCharacter.vCam.m_Lens.Dutch = -20f;
            }
    
            HandleMovement();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            cameraTilt = currentCharacter.vCam.GetComponent<CinemachineTiltExtension>();
            /* cameraClamp = currentCharacter.vCam.GetComponent<CinemachineClampedYRotationExtension>();
            cameraClamp.enabled = true;
            cameraClamp.SetBaseRotation(WallStateVariables.Instance.LastWallNormal); */
            playerStateMachine.wallRunEffect.SetActive(true);
            playerStateMachine.playerAudio.clip =playerStateMachine.wallRunSound;
            //playerStateMachine.playerAudio.PlayOneShot(playerStateMachine.wallRunSound);
            playerStateMachine.playerAudio.Play();
            playerStateMachine.playerAudio.loop = true;
            monoBehaviour.StartCoroutine(SetNormal());
            this.WallSpeed = playerStateMachine.WallSpeed;
        }

        public void OnExit()
        {
            playerStateMachine.playerAudio.loop = false;
            playerStateMachine.playerAudio.Stop();
            playerStateMachine.playerAudio.clip = null;
            playerStateMachine.wallRunEffect.SetActive(false);
            WallStateVariables.Instance.LastWallNormal = wallNormal;
            wallNormal = Vector3.zero; 
            wallForward = Vector3.zero;
            cameraTilt.SetTiltDirection(0f);
            //cameraClamp.enabled = false;
            //currentCharacter.vCam.m_Lens.Dutch = 0f;
        }
        // Lerp from float a to b by t 

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
            return Color.blue;
        }
    }
}
