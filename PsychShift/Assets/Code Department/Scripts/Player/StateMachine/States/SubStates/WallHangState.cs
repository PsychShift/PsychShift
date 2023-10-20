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

        private Transform lastLedge;
        private Transform currentLedge;
        private RaycastHit ledgeHit;

        //Exiting
         public bool exitingLedge;
        public float exitLedgeTime;
        private float exitLedgeTimer;
        public WallHangState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }
        public void OnEnter()
        {
            Debug.Log("Entered Wall Hang State");
            currentCharacter = playerStateMachine.currentCharacter;
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            LedgeDetection();
            MovementHandler();
            
            if (freeze)
            {
                rb.velocity = Vector3.zero;
            }

        }
        private void MovementHandler()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        // SubState 1 - Holding onto ledge
        if (holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;

            if (timeOnLedge > minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();

            if (Input.GetKeyDown(jumpKey)) LedgeJump();
        }

        // Substate 2 - Exiting Ledge
        else if (exitingLedge)
        {
            if (exitLedgeTimer > 0) exitLedgeTimer -= Time.deltaTime;
            else exitingLedge = false;
        }
    }
        private void LedgeDetection()
    {
        bool ledgeDetected = Physics.SphereCast(currentCharacter.position, ledgeSphereCastRadius, camera.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);

        if (!ledgeDetected) return;

        float distanceToLedge = Vector3.Distance(currentCharacter.position, ledgeHit.transform.position);

        if (ledgeHit.transform == lastLedge) return;

        if (distanceToLedge < maxLedgeGrabDistance && !holding) EnterLedgeHold();
    }
    private void LedgeJump()
    {
        ExitLedgeHold();

        Invoke(nameof(DelayedJumpForce), 0.05f);
    }

        private void Invoke(string v1, float v2)
        {
            throw new NotImplementedException();
        }

        private void DelayedJumpForce()
    {
        Vector3 forceToAdd = camera.forward * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardForce;
        rb.velocity = Vector3.zero;
        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }




    private void EnterLedgeHold()
    {
        holding = true;

        //unlimited = true;
        //restricted = true;

        currentLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }
    private void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;

        Vector3 directionToLedge = currentLedge.position - currentCharacter.position;
        float distanceToLedge = Vector3.Distance(currentCharacter.position, currentLedge.position);

        // Move player towards ledge
        if(distanceToLedge > 1f)
        {
            if(rb.velocity.magnitude < moveToLedgeSpeed)
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
        }

        // Hold onto ledge
        else
        {
            if (!freeze) freeze = true;
            //if (unlimited) unlimited = false;
        }

        // Exiting if something goes wrong
        if (distanceToLedge > maxLedgeGrabDistance) ExitLedgeHold();
    }

    private void ExitLedgeHold()
    {
        exitingLedge = true;
        exitLedgeTimer = exitLedgeTime;

        holding = false;
        timeOnLedge = 0f;

        //restricted = false;
        freeze = false;

        rb.useGravity = true;

        //StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);
    }

    private void ResetLastLedge()
    {
        lastLedge = null;
    }
}
}