using UnityEngine;

namespace Player
{
    public class CharacterInfo
    {
        public GameObject characterContainer;
        public GameObject model;
        public Transform cameraRoot;

        public Rigidbody rb;

        public override string ToString()
        {
            return $"CharacterInfo:\n" +
                $"CharacterContainer: {characterContainer}\n" +
                $"Model: {model.transform.position}\n" +
                $"CameraRoot: {cameraRoot.position}\n" +
                $"Rigidbody: {rb}";
        }
    }
}
