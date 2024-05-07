using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        #region Sensitivity
        [SerializeField] private float keyBoardSensitivityX = 15F;
        [SerializeField] private float keyBoardSensitivityY = 15F;
        [SerializeField] private float controllerSensitivityX = 15F;
        [SerializeField] private float controllerSensitivityY = 15F;
        #endregion

        #region References
        private Transform currentCharacter;
        private Transform currentCameraRoot;
        private CharacterInfo currentCharacterInfo;
        public new Camera camera;
        #endregion

        #region Current Info based on input method
        private float currentSenseX;
        private float currentSenseY;
        #endregion

        public bool IsActive { get; set; }
        /* public void Awake()
        {
            return;
            camera = Camera.main;
        }
        private void SwitchSensitivity(bool isKeyboard)
        {
            return;
            if(isKeyboard)
            {
                currentSenseX = keyBoardSensitivityX;
                currentSenseY = keyBoardSensitivityY;
            }
            else
            {
                currentSenseX = controllerSensitivityX;
                currentSenseY = controllerSensitivityY;
            }
        }

        public void RotatePlayer()
        {
            return;
            Vector2 mouseDelta = InputManager.Instance.GetMouseDelta();
            mouseDelta.x *= currentSenseX;
            mouseDelta.y *= currentSenseY;

            Vector3 currentRotation = currentCameraRoot.localRotation.eulerAngles;

            currentRotation.x -= mouseDelta.y * currentSenseY;
            currentRotation.y += mouseDelta.x * currentSenseX;

            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

            currentCameraRoot.localRotation = Quaternion.Euler(currentRotation);
            currentCharacterInfo.characterContainer.transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
        }

        public void SetCharacter(CharacterInfo characterInfo)
        {
            return;
            currentCharacterInfo = characterInfo;
            currentCharacter = characterInfo.characterContainer.transform;
            currentCameraRoot = characterInfo.cameraRoot;
            camera.transform.parent = currentCameraRoot;
        }

*/
    } 

}

