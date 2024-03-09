using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//namespace Player
//{
public class SensitivityController : MonoBehaviour
{
    public Slider slider;
    public float mouseSensitivity = 100f;
    //public Transform playerBody;
    float xRotation = 0f;

    private static SensitivityController instance;
    public static SensitivityController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindFirstObjectByType<SensitivityController>();
            }
            return instance;
        }
    }

    //private Transform currentCharacter;
    //private Transform currentCameraRoot;
    //private CharacterInfo currentCharacterInfo;
    //public new Camera camera;
    //public Action<float> UpdatedSpeed;

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("currentSensitivity", mouseSensitivity);
        //UpdatedSpeed?.Invoke(mouseSensitivity);
        CinemachinePOVExtension.horizontalSpeed = mouseSensitivity;
        CinemachinePOVExtension.verticalSpeed = mouseSensitivity;
        slider.value = mouseSensitivity/10;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /* void Update()
    {
        PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    } */

    public void AdjustSpeed(float newSpeed)
    {
        mouseSensitivity = newSpeed * 10;
        Debug.Log(mouseSensitivity);
        PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);
        CinemachinePOVExtension.horizontalSpeed = mouseSensitivity;
        CinemachinePOVExtension.verticalSpeed = mouseSensitivity;
        //UpdatedSpeed.Invoke(mouseSensitivity);
    }
}
//}