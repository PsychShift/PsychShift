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
            currentCharacter = playerStateMachine.currentCharacter;
            SetSubState();
        }

        public void OnExit()
        {
            stateMachine._currentSubState = currentSubState;
        }

        public void Tick()
        {
            //wallVariables.OrganizeHitsList();
            SubStateTick();
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
        public Dictionary<Vector3, RaycastHit> DirectionHits = new Dictionary<Vector3, RaycastHit>
        {
            { Vector3.forward, new RaycastHit() },
            { Vector3.right, new RaycastHit() },
            { Vector3.right + Vector3.forward, new RaycastHit() },
            { Vector3.right - Vector3.forward, new RaycastHit() },
            { Vector3.left, new RaycastHit() },
            { Vector3.left + Vector3.forward, new RaycastHit() },
            { Vector3.left - Vector3.forward, new RaycastHit() }
        };
        public Vector3 LastWallPosition { get; set; }
        public Vector3 LastWallNormal { get; set; }

        public void CheckWalls(Transform charTransform)
        {
            Dictionary<Vector3, RaycastHit> tempDirectionHits = new Dictionary<Vector3, RaycastHit>();
            foreach (var item in DirectionHits)
            {
                Vector3 dir = charTransform.TransformDirection(item.Key);
                RaycastHit hit;
                Physics.Raycast(charTransform.position, dir, out hit, 1.6f, wallLayer);
                tempDirectionHits.Add(item.Key, hit);
            }
            DirectionHits.Clear();
            DirectionHits.AddRange(tempDirectionHits);
            /* foreach(var item in DirectionHits)
            {
                Vector3 dir = charTransform.TransformDirection(item.Key);
                RaycastHit hit = item.Value;
                Color c = Color.green;
                if(hit.collider == null)
                    c = Color.red;
                Debug.DrawRay(charTransform.position, dir, c, 1);
            } */

            WallInForwardDirection();
            WallToSides();
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

        public bool SideWall;
        private void WallToSides()
        {
            if(ForwardWall) return;
            foreach (var item in DirectionHits)
            {
                if(item.Value.collider != null)
                {
                    SideWall = true;
                    return;
                }
            }
            SideWall = false;
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
