using UnityEngine;
using Cinemachine;
using Player;

namespace ImpactSystem.Effects
{
    public class SpecialEffectManager : MonoBehaviour
    {
        public CinemachineVirtualCamera currentCamera => PlayerStateMachine.Instance.currentCharacter.vCam;
    }

}
