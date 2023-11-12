using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem1 : MonoBehaviour
{

    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    // Start is called before the first frame update
    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    //extra visuals
    public GameObject muzzleFlash, bulletHoleGraphic;
    //public CamShake camShake;//Add when cam script is done
    //public float camShakeMagnitude, camShakeDuration;
    public TextMeshProUGUI text;


    private void Awake() 
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        InputManager1.Instance.OnShootPressed += PressedShoot;
    }
    private void OnDisable()
    {
        InputManager1.Instance.OnShootPressed -= PressedShoot;
    }
/*     private void Update()
    {
        MyInput();
        //text.SetText(bulletsLeft +" / " + magazineSize);
    }

    private void MyInput()
    {
        if(allowButtonHold)
            shooting = InputManager.Instance.PlayerShotThisFrame();
        else 
            shooting = InputManager.Instance.PlayerShotThisFrame(); 

        //shoot
        if(readyToShoot && shooting && bulletsLeft >0)
        {
            bulletsShot = bulletPerTap;
            Shoot();
        }
    } */

    private void PressedShoot()
    {
        if(readyToShoot && bulletsLeft > 0)
        {
            bulletsShot = bulletPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread); 

        Vector3 direction = fpsCam.transform.forward + new Vector3(x,y,0); 




        if(Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            //if(rayHit.collider.CompareTag("Enemy"))
                //rayHit.collider.GetComponent<EnemyDmg>().TakeDamage(damage);//SCRIPT FOR ENEMY TO TAKE DAMAGE
        }

        //Cam Shake here
        //camShake.Shake(camShakeDuration, camShakeMagnitude);

        //FX
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180,0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        
        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);
        if(bulletsShot > 0 && bulletsLeft>0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
}
