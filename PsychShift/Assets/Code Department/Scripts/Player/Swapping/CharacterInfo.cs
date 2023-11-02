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

        public CharacterController controller;
        public NavMeshAgent agent;

        public CharacterMovementStatsSO movementStats;
        public CharacterStatsSO characterStats;

        public CharacterInfo(GameObject characterContainer)
        {
            this.characterContainer = characterContainer;
            model = characterContainer.transform.Find("Model").gameObject;
            wallCheck = characterContainer.transform.Find("WallCheck");
            cameraRoot = characterContainer.transform.Find("CameraRoot");
        }
        public CharacterInfo(GameObject characterContainer, CharacterMovementStatsSO movementStats, CharacterStatsSO characterStats)
        {
            this.characterContainer = characterContainer;
            model = characterContainer.transform.Find("Model").gameObject;
            if(model == null) Debug.LogError($"Model is null: Confirm the Character {characterContainer.name} has a child named Model!");
            wallCheck = characterContainer.transform.Find("WallCheck");
            if(wallCheck == null) Debug.LogError($"WallCheck is null: Confirm the Character {characterContainer.name} has a child named WallCheck!");
            cameraRoot = characterContainer.transform.Find("CameraRoot");
            if(cameraRoot == null) Debug.LogError($"CameraRoot is null: Confirm the Character {characterContainer.name} has a child named CameraRoot!");

            controller = characterContainer.GetComponent<CharacterController>();
            agent = characterContainer.GetComponent<NavMeshAgent>();

            this.movementStats = movementStats;
            this.characterStats = characterStats;
        }
        public override string ToString()
        {
            return $"CharacterInfo:\n" +
                $"CharacterContainer: {characterContainer}\n" +
                $"Model: {model.transform.position}\n" +
                $"Wall Check: {wallCheck.position}\n" +
                $"CameraRoot: {cameraRoot.position}\n" +
                $"Controller: {controller}";
        }
    }
}
