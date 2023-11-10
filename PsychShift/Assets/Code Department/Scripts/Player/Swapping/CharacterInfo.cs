using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [System.Serializable]
    public class CharacterInfo
    {
        public GameObject characterContainer;
        public GameObject model;
        public Transform wallCheck;
        public Transform cameraRoot;
        public Cinemachine.CinemachineVirtualCamera vCam;

        public CharacterController controller;
        public NavMeshAgent agent;

        public CharacterMovementStatsSO movementStats;
        public CharacterStatsSO characterStats;

        [HideInInspector] public EnemyBrain enemyBrain;

        public CharacterInfo(GameObject characterContainer)
        {
            this.characterContainer = characterContainer;
            model = characterContainer.transform.Find("Model").gameObject;
            wallCheck = characterContainer.transform.Find("WallCheck");
            cameraRoot = characterContainer.transform.Find("CameraRoot");
            enemyBrain = characterContainer.GetComponent<EnemyBrain>();
        }
        public CharacterInfo(GameObject characterContainer, Cinemachine.CinemachineVirtualCamera vCam, CharacterMovementStatsSO movementStats, CharacterStatsSO characterStats)
        {
            this.characterContainer = characterContainer;
            model = characterContainer.transform.Find("Model").gameObject;
            if(model == null) Debug.LogError($"Model is null: Confirm the Character {characterContainer.name} has a child named Model!");
            wallCheck = characterContainer.transform.Find("WallCheck");
            if(wallCheck == null) Debug.LogError($"WallCheck is null: Confirm the Character {characterContainer.name} has a child named WallCheck!");
            cameraRoot = characterContainer.transform.Find("CameraRoot");
            if(cameraRoot == null) Debug.LogError($"CameraRoot is null: Confirm the Character {characterContainer.name} has a child named CameraRoot!");

            controller = characterContainer.GetComponent<CharacterController>();
            enemyBrain = characterContainer.GetComponent<EnemyBrain>();
            agent = characterContainer.GetComponent<NavMeshAgent>();

            this.vCam = vCam;

            this.movementStats = movementStats;
            this.characterStats = characterStats;
        }
        public override string ToString()
        {
            return $"CharacterInfo:\n" +
                $"CharacterContainer: {characterContainer.name}\n" +
                $"Model: {model.transform.name}\n" +
                $"Wall Check: {wallCheck.position}\n" +
                $"CameraRoot: {cameraRoot.position}\n" +
                $"Controller: {controller}\n" +
                $"NavMeshAgent: {agent}\n" +
                $"vCam: {vCam}";
        }
    }
}
