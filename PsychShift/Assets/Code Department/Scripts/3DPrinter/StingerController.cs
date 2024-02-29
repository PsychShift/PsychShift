using System.Collections;
using UnityEngine;

public class StingerController : MonoBehaviour
{
    [SerializeField] private Transform tailTip, tailBase;

    [SerializeField] private float timeToAimTip = 0.2f;
    public LaserShooter laserShooter;
    private const float amountToRotateY = 180;
    private static readonly Vector3 normalRotTip = new Vector3(180, 0, -6.7f);
    private static readonly Vector3 aimingRotTip = new Vector3(180, 0, -48f);

    [HideInInspector] public bool isDone = false;
    void OnEnable()
    {
        tailTip.localEulerAngles = normalRotTip;
    }

    public IEnumerator FireLaser(float fireTime, bool rotDir)
    {
        isDone = false;
        int rotDirNum = rotDir ? 1 : -1;
        float rotAmount = amountToRotateY * rotDirNum;
        float elapsedTime =  0f;
        
        while(elapsedTime < timeToAimTip)
        {
            tailTip.localEulerAngles = Vector3.Lerp(normalRotTip, aimingRotTip, elapsedTime / timeToAimTip);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tailTip.localEulerAngles = aimingRotTip;

        elapsedTime = 0f;

        float halfTime = fireTime/2f;
        Quaternion startRotation = tailBase.rotation;
        Quaternion halfRotation = Quaternion.Euler(0,  rotAmount, 0) * startRotation;
        while(elapsedTime < halfTime)
        {
            float t = elapsedTime / halfTime; // Calculate the interpolation factor
            tailBase.rotation = Quaternion.Lerp(startRotation, halfRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while(elapsedTime < halfTime)
        {
            float t = elapsedTime / halfTime; // Calculate the interpolation factor
            tailBase.rotation = Quaternion.Lerp(halfRotation, startRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while(elapsedTime < timeToAimTip)
        {
            tailTip.localEulerAngles = Vector3.Lerp(aimingRotTip, normalRotTip, elapsedTime / timeToAimTip);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tailTip.localEulerAngles = normalRotTip;
        isDone = true;
    }


}
