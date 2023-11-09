using UnityEngine;

namespace Player
{
    public class VaultState : IState
    {
        private readonly PlayerStateMachine playerStateMachine;
        private CharacterInfo currentCharacter;
        private float playerHeight = 2f;
        private float playerRadius = 0.5f;
        private float time = 0f;
        private float vaultTime = 0.5f;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 currentTargetPosition;
        public VaultState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }

        public void OnEnter()
        {
            Debug.Log("Entered Vault State");
            currentCharacter = playerStateMachine.currentCharacter;
            time = 0f;
            CheckVault();
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
            if(time < vaultTime)
            {
                currentTargetPosition = Vector3.Lerp(startPosition, targetPosition, time / vaultTime);
                playerStateMachine.AppliedMovementX = currentTargetPosition.x;
                playerStateMachine.AppliedMovementY = currentTargetPosition.y;
                playerStateMachine.AppliedMovementZ = currentTargetPosition.z;

                time += Time.deltaTime;
            }
            else
                playerStateMachine.IsVaulting = false;
        }

        private void CheckVault()
        {
            if (Physics.Raycast(playerStateMachine.currentCharacter.model.transform.position, playerStateMachine.currentCharacter.model.transform.transform.forward, out var firstHit, 1f))
                if (Physics.Raycast(firstHit.point + (playerStateMachine.currentCharacter.model.transform.transform.forward * playerRadius) + (Vector3.up * 0.6f * (playerHeight + 10)), Vector3.down, out var secondHit, (playerHeight + 8)))        
                    SetTarget(secondHit.point);
        }
        private void SetTarget(Vector3 targetPos)
        {
            targetPosition = targetPos;
        }
    }
}
