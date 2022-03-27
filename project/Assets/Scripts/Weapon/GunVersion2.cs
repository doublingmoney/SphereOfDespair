using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunVersion2 : MonoBehaviour
{
    [Header("Causes errors if empty")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private LayerMask _rayIgnoreLayer;

    [Header("Optional")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private float _recoilForce;

    [Header("Weapon Stats")]
    [SerializeField] private float _timeBetweenShots = 0.5f;
    [SerializeField] private int _bulletsPerShot = 1;
    [SerializeField] private int _batteryDrainPerShot = 1;
    [SerializeField] private float _spread = 0;
    [Header("Weapon Cooldown")]
    public int _batterySize = 100;
    [SerializeField] private int _cooldownRate = 10;
    [SerializeField] private float _cooldownDelay = 2f;
    [SerializeField] private float _cooldownDelayMax = 5f;
    [SerializeField] private bool _autoFire = false;
    

    [Header("Bullet forces")]
    [SerializeField] private float _forwardForce = 1f;
    [SerializeField] private float _upwardForce = 0f;

    //Booleans for weapon state
    private bool _shooting, _readyToShoot, _onCooldown, _recharging;
    private float _timeSinceLastShot;
    [HideInInspector]
    public int _batteryRemaining;

    private void Awake()
    {
        _batteryRemaining = _batterySize;
        _timeSinceLastShot = 0;
    }
    
    private void Update()
    {
        GetInputType(); //depends on _autoFire boolean
        CheckIfReadyToShoot(); //simply checks conditions and sets _readyToShoot boolean

        //check time since last shot and initiate recharge
        _timeSinceLastShot += Time.deltaTime;
        if(_timeSinceLastShot >= _cooldownDelay && !_recharging && _batteryRemaining < _batterySize)
        {
            _recharging = true;
            StartCoroutine("RechargeSmallCD");
        }

        //needs check if charge goes to 0 and then initiate long CD ignoring small CD

    }
    private void LateUpdate()
    {
        if(_readyToShoot && _shooting)
        {
            if(_autoFire && _timeSinceLastShot >= _timeBetweenShots)
            {
                Shoot();
                _timeSinceLastShot = 0;
            } else if(!_autoFire)
            {
                Shoot();
            }  
        }
    }

    private IEnumerator RechargeSmallCD()
    {
        float delay = 1 / _cooldownRate;

        while(_timeSinceLastShot >= _cooldownDelay)
        {
            _batteryRemaining++;
            if (_batteryRemaining >= _batterySize)
            {
                _batteryRemaining = _batterySize;
                break;
            }
            yield return new WaitForSeconds(delay);
        }
        _recharging = false;
    }

    private void MaxCooldown()
    {
        _onCooldown = true;
    }

    private void CheckIfReadyToShoot()
    {
        if(_onCooldown || _batteryRemaining <= 0)
        {
            _readyToShoot = false;
        } else
        {
            _readyToShoot = true;
        }
       
    }
    private void GetInputType()
    {
        //check if weapon is autoFire
        if (_autoFire)
            _shooting = Input.GetKey(KeyCode.Mouse0);
        else
            _shooting = Input.GetKeyDown(KeyCode.Mouse0);
    }
    private void Shoot()
    {
        //find hit position using raycast
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Ray to middle of the viewPort
        //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        Vector3 targetpoint;

        //check if ray hits anything. Else set point X units away from camera.
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~_rayIgnoreLayer, QueryTriggerInteraction.Ignore)) 
        { 
            targetpoint = hit.point;
            //Debug.Log(hit.collider.name);
        }
        else { targetpoint = ray.GetPoint(75);  }

        Vector3 direction = targetpoint - _barrelTransform.position;
        //loop for shotsPerShot to create many projectiles.
        for (int i = 0; i < _bulletsPerShot; i++ )
        {
            //Randomize spread and calculate direction from barrel point to target.
            float x = Random.Range(-_spread, _spread);
            float y = Random.Range(-_spread, _spread);
            Vector3 directionWS = Vector3.Normalize(direction + new Vector3(x, y, 0));

            //instantiate the bullet, get its rigid body, rotate it to shooting direction and apply forces
            GameObject bullet = Instantiate(_bullet, _barrelTransform.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bullet.transform.forward = directionWS;

            bulletRb.AddForce(directionWS * _forwardForce, ForceMode.Impulse);
            bulletRb.AddForce(directionWS * _upwardForce, ForceMode.Impulse);
        }

        //muzzle flash and sound effect
        if (_shootSound != null) _shootSound.Play();
        if (_muzzleFlash != null) Instantiate(_muzzleFlash, _barrelTransform.position, Quaternion.identity);

        _batteryRemaining -= _batteryDrainPerShot;

        if(_playerRb != null)
        {
            _playerRb.AddForce(direction.normalized * -1 * _recoilForce, ForceMode.Impulse);
        }
    }
}
