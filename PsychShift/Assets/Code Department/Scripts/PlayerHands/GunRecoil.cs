using System.Collections;
using System.Collections.Generic;
using Guns;
using Guns.Demo;
using Player;
using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public static GunRecoil Instance;
    public RecoilConfigScriptableObject config;

    private Vector3 currentRotation;
    public Vector3 rot;

    void Awake()
    {
        Instance = this;
    }


    void FixedUpdate()
    {
        currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, config.returnSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, currentRotation, config.rotationSpeed * Time.fixedDeltaTime);
        PlayerStateMachine.Instance.currentCharacter.vCam.Follow.transform.localRotation = Quaternion.Euler(rot);
    }

    public void Fire()
    {
        currentRotation += new Vector3(-config.recoilRotation.x, Random.Range(-config.recoilRotation.y, config.recoilRotation.y), Random.Range(-config.recoilRotation.z, config.recoilRotation.z));
    }

    public void SetUpGun(RecoilConfigScriptableObject config)
    {
        this.config = config;
    }
}
