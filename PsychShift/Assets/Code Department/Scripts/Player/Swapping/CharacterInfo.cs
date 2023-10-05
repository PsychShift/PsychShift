using UnityEngine;

namespace Player
{
    public class CharacterInfo
    {
        public GameObject characterContainer;
        public GameObject model;
        public Transform cameraRoot;

        public CharacterController controller;

        public override string ToString()
        {
            return $"CharacterInfo:\n" +
                $"CharacterContainer: {characterContainer}\n" +
                $"Model: {model.transform.position}\n" +
                $"CameraRoot: {cameraRoot.position}\n" +
                $"Controller: {controller}";
        }
    }
}
