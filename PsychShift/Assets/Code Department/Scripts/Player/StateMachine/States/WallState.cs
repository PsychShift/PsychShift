using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class WallState : RootState, IState
    {
        private CharacterInfo currentCharacter;

        public WallState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;

        }

        public void OnEnter()
        {
            WallStateVariables.Instance.TimeOnWall = 0f;
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;
            playerStateMachine.CurrentMovementY = 0;
            playerStateMachine.AppliedMovementY = 0;
            SetSubState();
        }
        //Kevin added this call public funct from substate that swaps var that changes wallhang substate
        public void OnExit()
        {
            stateMachine._currentSubState = currentSubState;
        }

        public void Tick()
        {
            //Call public funct here when the button is pressed//Kevin Added this
            /*if(InputManager.Instance.PlayerSwitchedModeThisFrame())
            {
                //Call funct here
                WallHangState.canHangChange();
            }*/
            WallStateVariables.Instance.TimeOnWall += Time.deltaTime;
            SubStateTick();
            //wallVariables.OrganizeHitsList();
            WallStateVariables.Instance.CheckWalls(currentCharacter.model.transform);
            //Debug.Log("Forward Wall == " + WallStateVariables.Instance.ForwardWall + " Side Wall == " + WallStateVariables.Instance.SideWall);
        }
    }

    public class WallStateVariables
    {
        private static WallStateVariables instance;

        public static WallStateVariables Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WallStateVariables();
                }
                return instance;
            }
        }
        public LayerMask wallLayer;
        public LayerMask WallholdLayers;
        public float WallSpeed { get; set; }
        public float TimeOnWall { get; set; }
        public float TimeOffWall { get; set; }

        public Dictionary<Vector3, RaycastHit> DirectionHits = new Dictionary<Vector3, RaycastHit>
        {
            { Vector3.forward, new RaycastHit() },
            { Vector3.right, new RaycastHit() },
            { Vector3.right + Vector3.forward, new RaycastHit() },
            { Vector3.right - Vector3.forward, new RaycastHit() },
            { Vector3.left, new RaycastHit() },
            { Vector3.left + Vector3.forward, new RaycastHit() },
            { Vector3.left - Vector3.forward, new RaycastHit() },
            { Vector3.back, new RaycastHit( )}
        };
        public Vector3 LastWallPosition;
        public Vector3 LastWallNormal;

        public bool WallLeft;
        public bool WallRight;
        public void CheckWalls(Transform charTransform)
        {
            Dictionary<Vector3, RaycastHit> tempDirectionHits = new Dictionary<Vector3, RaycastHit>();
            foreach (var item in DirectionHits)
            {
                Vector3 dir = charTransform.TransformDirection(item.Key);
                RaycastHit hit;
                Physics.Raycast(charTransform.position + Vector3.up, dir, out hit, 2.5f, wallLayer);

                Color c = Color.red;
                if(hit.normal != Vector3.zero)
                    c = Color.green;
                Debug.DrawRay(charTransform.position + Vector3.up, dir, c, 1);
                
                tempDirectionHits.Add(item.Key, hit);
            }
            DirectionHits.Clear();
            DirectionHits.AddRange(tempDirectionHits);

            WallInForwardDirection();
            LeftRightCheck();
        }

        public void LeftRightCheck()
        {
            WallRight = DirectionHits[Vector3.right].normal.magnitude > 0 || DirectionHits[Vector3.right + Vector3.forward].normal.magnitude > 0 || DirectionHits[Vector3.right - Vector3.forward].normal.magnitude > 0;
            WallLeft = DirectionHits[Vector3.left].normal.magnitude > 0 || DirectionHits[Vector3.left + Vector3.forward].normal.magnitude > 0 || DirectionHits[Vector3.left - Vector3.forward].normal.magnitude > 0;
        }
        public Vector3 RightWallNormal()
        {
            return DirectionHits[Vector3.right].normal.magnitude > 0 ? DirectionHits[Vector3.right].normal :
                   DirectionHits[Vector3.right + Vector3.forward].normal.magnitude > 0 ? DirectionHits[Vector3.right + Vector3.forward].normal :
                   DirectionHits[Vector3.right - Vector3.forward].normal.magnitude > 0 ? DirectionHits[Vector3.right - Vector3.forward].normal :
                   Vector3.zero;
        }
        public Vector3 LeftWallNormal()
        {
            return DirectionHits[Vector3.left].normal.magnitude > 0 ? DirectionHits[Vector3.left].normal :
                   DirectionHits[Vector3.left + Vector3.forward].normal.magnitude > 0 ? DirectionHits[Vector3.left + Vector3.forward].normal :
                   DirectionHits[Vector3.left - Vector3.forward].normal.magnitude > 0 ? DirectionHits[Vector3.left - Vector3.forward].normal :
                   Vector3.zero;
        }

        public bool CheckOnWall()
        {
            foreach(var item in DirectionHits)
            {
                
                if(item.Value.collider != null) return true;
            }

            return false;
        }

        public bool ForwardWall;
        private void WallInForwardDirection()
        {
            if(DirectionHits[Vector3.forward].collider != null)
            {
                ForwardWall = true;
                return;
            }
            ForwardWall = false;
        }

        private float ledgeDetectionLength = 0.3f;
        private float ledgeSphereCastRadius = 4.0f;
        private float maxLedgeGrabDistance = 4.0f;
        public bool LedgeDetection(CharacterInfo currentCharacter, Transform cameraTransform)
        {
            bool ledgeDetected = Physics.SphereCast(currentCharacter.characterContainer.transform.position + Vector3.up * 1f , ledgeSphereCastRadius, cameraTransform.forward, out RaycastHit ledgeHit, ledgeDetectionLength, WallholdLayers);

            if (!ledgeDetected) return false;

            float distanceToLedge = Vector3.Distance(currentCharacter.characterContainer.transform.position, ledgeHit.transform.position);

            return distanceToLedge < maxLedgeGrabDistance;
        }
        

        public RaycastHit[] SetUpHitsList()
        {
            RaycastHit[] hits = new RaycastHit[DirectionHits.Count];
            int i = 0;
            foreach(var item in DirectionHits)
            {
                hits[i] = item.Value;
                i++;
            }
            hits = hits.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();

            return hits;
        }

        public void PrintDictionary()
        {
            string print = "";
            foreach (var item in DirectionHits)
            {
                print += "{Key:" + item.Key.ToString() + ", Value: ";
                if(item.Value.collider == null)
                    print += "null}";
                else
                    print += item.Value.collider.name;
                print += "/n";
            }
            //Debug.Log(print);
        }
    }
}
