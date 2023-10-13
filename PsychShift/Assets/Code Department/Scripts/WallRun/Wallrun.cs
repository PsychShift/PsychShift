using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Player;
public class WallRun : RootState
{
    // public float WallRunSpeed; this needs to go into PlayerStateMachine will have to reset walk speed after wallrunning using a copy variable.
    public float wallMaxDistance = 1;
    public float wallGravityDownForce = 3f;
    public float minimumHeight = 1.2f;

    public float jumpDuration;
    public float wallBouncing = 3;
    public float maxAngleRoll = 20;
    [Range(0f, 1f)]
    public float normalizedAngleThreshold = 0.1f;

    public Volume wallRunVolume;

    Vector3[] directions;
    RaycastHit[] hits;

    bool isWallRunning = false;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;
    float elapsedTimeSinceJump = 0;
    float elapsedTimeSinceWallAttach = 0;
    float elapsedTimeSinceWallDetatch = 0;
    bool jumping;

    float cameraTransitionDuration = 0.5f;

    float lastVolumeValue = 0;

    private PlayerStateMachine playerStateMachine;

    public WallRun(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    bool isPlayergrounded() => playerStateMachine.currentCharacter.controller.isGrounded;

    public bool IsWallRunning() => isWallRunning;

    bool CanWallRun()
    {
        float verticalAxis = InputManager.Instance.MoveAction.ReadValue<Vector2>().y;

        return !isPlayergrounded() && verticalAxis > 0 && VerticalCheck();
        // Start is called before the first frame update
    }

    bool VerticalCheck()
    {
        return !Physics.Raycast(playerStateMachine.currentCharacter.characterContainer.transform.position, Vector3.down, minimumHeight);
    }
    void Start()
    {
        jumpDuration = playerStateMachine.maxJumpTime / 2;
        directions = new Vector3[]{
            Vector3.right,
            Vector3.right + Vector3.forward,
            Vector3.right - Vector3.forward,
            Vector3.left,
            Vector3.left + Vector3.forward,
            Vector3.left - Vector3.forward
        };
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LateUpdate()
    {
        isWallRunning = false;

        if (InputManager.Instance.IsJumpPressed)
        {
            jumping = true;
        }

        if (CanAttach())
        {
            hits = new RaycastHit[directions.Length];

            for (int i = 0; i < directions.Length; i++)
            {
                Transform charTransform = playerStateMachine.currentCharacter.characterContainer.transform;
                Vector3 dir = charTransform.TransformDirection(directions[i]);
                Physics.Raycast(charTransform.position, dir, out hits[i], wallMaxDistance);
                if (hits[i].collider != null)
                {
                    Debug.DrawRay(charTransform.position, dir * hits[i].distance, Color.green);
                }
                else
                {
                    Debug.DrawRay(charTransform.position, dir * wallMaxDistance, Color.red);
                }
            }

            if (CanWallRun())
            {
                hits = hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
                if (hits.Length > 0)
                {
                    OnWall(hits[0]);
                    lastWallPosition = hits[0].point;
                    lastWallNormal = hits[0].normal;
                }
            }
        }

        if (isWallRunning)
        {
            elapsedTimeSinceWallDetatch = 0;
            // Turn on camera tilt
            //if (elapsedTimeSinceWallAttach == 0 && wallRunVolume != null)
            //    lastVolumeValue = wallRunVolume.weight;

            elapsedTimeSinceWallAttach += Time.deltaTime;
            // Apply wall run gravity
            //m_PlayerCharacterController.WalkSpeed += Vector3.down * wallGravityDownForce * Time.deltaTime;
        }
        else
        {
            elapsedTimeSinceWallAttach = 0;
            // Turn off camera tilt
            //if (elapsedTimeSinceWallDetatch == 0 && wallRunVolume != null)
            //    lastVolumeValue = wallRunVolume.weight;
            elapsedTimeSinceWallDetatch += Time.deltaTime;
        }

        if (wallRunVolume != null)
            HandleVolume();
    }

    private void HandleVolume()
    {
        float w = 0;
        if(isWallRunning)
        {
            w = Mathf.Lerp(lastVolumeValue, 1, elapsedTimeSinceWallAttach / cameraTransitionDuration);
        }
        else
        {
            w = Mathf.Lerp(lastVolumeValue, 0, elapsedTimeSinceWallDetatch / cameraTransitionDuration);
        }

        SetVolumeWeight(w);
    }

    private void SetVolumeWeight(float weight)
    {
        wallRunVolume.weight = weight;
    }

    bool CanAttach()
    {
        if (jumping)
        {
            elapsedTimeSinceJump += Time.deltaTime;
            if (elapsedTimeSinceJump > jumpDuration)
            {
                elapsedTimeSinceJump = 0;
                jumping = false;
            }
            return false;
        }

        return true;
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
            Debug.DrawRay(playerStateMachine.currentCharacter.characterContainer.transform.position, lastWallNormal * 10, Color.magenta);

            //m_PlayerCharacterController.WalkSpeed = alongWall * vertical * wallSpeedMultiplier;
            
            isWallRunning = true;
        }
    }

    float CalculateSide()
    {
        if (isWallRunning)
        {
            Vector3 heading = lastWallPosition - playerStateMachine.currentCharacter.characterContainer.transform.position;
            Vector3 perp = Vector3.Cross(playerStateMachine.currentCharacter.characterContainer.transform.forward, heading);
            float dir = Vector3.Dot(perp, playerStateMachine.currentCharacter.characterContainer.transform.up);
            return dir;
        }
        return 0;
    }

    //public float GetCameraRoll()
    //{
        //float dir = CalculateSide();
        //float cameraAngle = m_PlayerCharacterController.playerCamera.transform.eulerAngles.z;
        //float targetAngle = 0;
        //if (dir != 0)
        //{
            //targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        //}
        //return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(elapsedTimeSinceWallAttach, elapsedTimeSinceWallDetatch) / cameraTransitionDuration);
    //}

    public Vector3 GetWallJumpDirection()
    {
        if (isWallRunning)
        {
            return lastWallNormal * wallBouncing + Vector3.up;
        }
        return Vector3.zero;
    }

}
