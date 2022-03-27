using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magSize, bulletsPerShot;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody PlayerRigBody;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera Cam;
    public Transform PositionOfShotBullet;
    //Put empty object from where you want the bullet to spawn

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    
    public bool allowInvoke = true;

    private void Awake()
    {
        //Full mag
        bulletsLeft = magSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //placeholder for ammo(Later heat system )
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerShot + " / " + magSize / bulletsPerShot);
    }
    private void MyInput()
    {
        //check for allowing button holding
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
            Reload();
        //Reload automatically when soot while ammo is 0
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
            Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Find hit position using a raycast
        Ray ray = Cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits anything
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
            //Debug.Log(hit.point);
            //Debug.Log(hit.collider.name);
        }

        else
            targetPoint = ray.GetPoint(75);

        //Direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - PositionOfShotBullet.position;

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0); 

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, PositionOfShotBullet.position, Quaternion.identity); 
        //store instantiated bullet in currentBullet
        //Rotate bullet towards shooting direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(Cam.transform.up * upwardForce, ForceMode.Impulse);

        //muzzle flash
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, PositionOfShotBullet.position, Quaternion.identity);
        }

        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function if not invoked with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //Recoil based on instance
            if(PlayerRigBody != null)
            {
                PlayerRigBody.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
            }
        }

        //If more bullets than 1
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        //Allow shooting + invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime); //Reloads finnish function with reloadTime as delay
    }
    private void ReloadFinished()
    {
        //Fill mag
        bulletsLeft = magSize;
        reloading = false;
    }
}