using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    public class WallHangState : IState
    {
        //References
        PlayerStateMachine playerStateMachine;
        private CharacterInfo currentCharacter;
        public Transform camera;
        public Transform orientation;
        public Rigidbody rb;
        public bool holding;
        public bool freeze;
        //public bool unlimited;
        
        //Holding
        public float moveToLedgeSpeed;
        public float maxLedgeGrabDistance;

        public float minTimeOnLedge;
        private float timeOnLedge;

        //Jumping From ledges
        public KeyCode jumpKey = KeyCode.Space;
        public float ledgeJumpForwardForce;
        public float ledgeJumpUpwardForce;

        //Detecting
        public float ledgeDetectionLength;
        public float ledgeSphereCastRadius;
        public LayerMask whatIsLedge;

        private Transform currentLedge;

        //Exiting
         public bool exitingLedge;
        public float exitLedgeTime;
        public WallHangState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }
        public void OnEnter()
        {
            Debug.Log("Entered Wall Hang State");
            currentCharacter = playerStateMachine.currentCharacter;
            /* RaycastHit[] hits = Physics.SphereCastAll(currentCharacter.characterContainer.transform.position + Vector3.up * 1f , ledgeSphereCastRadius, playerStateMachine.cameraTransform.forward, ledgeDetectionLength, WallStateVariables.Instance.WallholdLayers, QueryTriggerInteraction.Collide);
            foreach(RaycastHit hit in hits)
            {
                if(hit.collider != null)
                    currentLedge = hit.collider.transform;
            } */
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
            FreezeOnLedge();            
        }

    private void FreezeOnLedge()
    {
        playerStateMachine.AppliedMovementX = 0;
            playerStateMachine.AppliedMovementZ = 0;
        /* Vector3 directionToLedge = currentLedge.position - currentCharacter.characterContainer.transform.position;
        float distanceToLedge = (currentCharacter.characterContainer.transform.position - currentLedge.position).magnitude;

        // Move player towards ledge
        if(distanceToLedge > 1f)
        {
            playerStateMachine.AppliedMovementX = directionToLedge.normalized.x * WallStateVariables.Instance.WallSpeed;
            playerStateMachine.AppliedMovementZ = directionToLedge.normalized.z * WallStateVariables.Instance.WallSpeed;
        } */
    }
}
}