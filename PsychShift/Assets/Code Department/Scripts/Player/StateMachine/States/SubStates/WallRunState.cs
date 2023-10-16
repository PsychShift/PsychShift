using System.Linq;
using UnityEngine;

namespace Player
{
    public class WallRunState : IState
    {
        private float normalizedAngleThreshold = 0.1f;





        private readonly PlayerStateMachine playerStateMachine;
        private CharacterInfo currentCharacter;
        private WallStateVariables wallVariables;
        public WallRunState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            wallVariables = WallStateVariables.Instance;
        }

        public void Tick()
        {
            HandleMovement();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            Debug.Log("WallRunState");
        }

        public void OnExit()
        {
            
        }

        private void HandleMovement()
        {
            RaycastHit[] Hits = WallStateVariables.Instance.SetUpHitsList();
            if (Hits.Length > 0)
            {
                OnWall(Hits[0]);
                wallVariables.LastWallPosition = Hits[0].point;
                wallVariables.LastWallNormal = Hits[0].normal;
            }
        }
        void OnWall(RaycastHit hit)
        {
            float d = Vector3.Dot(hit.normal, Vector3.up);
            if (d >= -normalizedAngleThreshold && d <= normalizedAngleThreshold)
            {
                // Vector3 alongWall = Vector3.Cross(hit.normal, Vector3.up);
                float vertical = InputManager.Instance.MoveAction.ReadValue<Vector2>().y;
                Vector3 alongWall = playerStateMachine.currentCharacter.characterContainer.transform.TransformDirection(Vector3.forward);

                Debug.DrawRay(playerStateMachine.currentCharacter.characterContainer.transform.position, alongWall.normalized * 10, Color.green);
                Debug.DrawRay(playerStateMachine.currentCharacter.characterContainer.transform.position, wallVariables.LastWallNormal * 10, Color.magenta);

                Vector3 movement = alongWall * vertical * 2f;
                playerStateMachine.AppliedMovementX = movement.x * playerStateMachine.WalkSpeed;
                playerStateMachine.AppliedMovementZ = movement.z * playerStateMachine.WalkSpeed;
                playerStateMachine.AppliedMovementY = 0;
                
            }
        }
        private void HandleGravity()
        {
            playerStateMachine.CurrentMovementY = playerStateMachine.gravityValue;
            playerStateMachine.AppliedMovementY = playerStateMachine.gravityValue;
        }
    }
}
