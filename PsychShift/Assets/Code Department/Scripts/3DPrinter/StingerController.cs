using System.Collections;
using UnityEngine;

public class StingerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform tailTip, tailBase;
    [SerializeField] private float timeToAimTip = 0.2f;
    public LaserShooter laserShooter;
    private const float amountToRotateY = 180;
    [SerializeField] private Vector3 normalRotTip = new Vector3(0, 0, -6.7f);
    [SerializeField] private Vector3 aimingRotTip = new Vector3(0, 0, -48f);

    [HideInInspector] public bool isDone = false;



    private int activateHash;
    private int deactivateHash;
    private int rightHash;
    private int speedHash;

    void OnEnable()
    {
        tailTip.localEulerAngles = normalRotTip;

        activateHash = Animator.StringToHash("ActivateTail");
        deactivateHash = Animator.StringToHash("DeActivateTail");
        rightHash = Animator.StringToHash("right");
        speedHash = Animator.StringToHash("TailSpeed");

        _animator.SetFloat(speedHash, (1 / laserShooter.defaultStats.ShootForTime));
    }
    public void FireLaser(bool right)
    {
        isDone = false;
        _animator.SetBool(rightHash, right);
        _animator.SetTrigger(activateHash);
    }
    public void TurnOnLaser()
    {
        laserShooter.Fire(true);
    }
    public void TurnOffLaser()
    {
        laserShooter.CeaseFire();
    }
    public void Done()
    {
        isDone = true;
    }

    public IEnumerator FireLaser(float fireTime, bool rotDir)
    {
        isDone = false;
        int rotDirNum = rotDir ? 1 : -1;
        float rotAmount = amountToRotateY * rotDirNum;
        float elapsedTime = 0f;

        while (elapsedTime < timeToAimTip)
        {
            tailTip.localEulerAngles = Vector3.Lerp(normalRotTip, aimingRotTip, elapsedTime / timeToAimTip);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tailTip.localEulerAngles = aimingRotTip;

        elapsedTime = 0f;

        float halfTime = fireTime / 2f;
        Quaternion startRotation = tailBase.rotation;
        Quaternion halfRotation = Quaternion.Euler(0, rotAmount, 0) * startRotation;
        while (elapsedTime < halfTime)
        {
            float t = elapsedTime / halfTime;
            tailBase.rotation = Quaternion.Lerp(startRotation, halfRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < halfTime)
        {
            float t = elapsedTime / halfTime;
            tailBase.rotation = Quaternion.Lerp(halfRotation, startRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < timeToAimTip)
        {
            tailTip.localEulerAngles = Vector3.Lerp(aimingRotTip, normalRotTip, elapsedTime / timeToAimTip);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        tailTip.localEulerAngles = normalRotTip;
        isDone = true;
    }
}

