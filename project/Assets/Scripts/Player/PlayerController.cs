using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private Transform _cameraTranform;
    private GameObject _dashSpot;
    private GameObject _weaponHolder;
    private float _movementX;
    private float _movementY;
    //private int _activeWeapon;

    private bool _tryingToMove = false;
    [HideInInspector] public float currentVelocity;

    [Header("Movement Settings")]
    [SerializeField] private float _torqueForce = 25f;
    [SerializeField] private float _maxAngularVelocity = Mathf.Infinity;
    //[SerializeField] private float _jumpForce = 100f;

    [Header("Dash Settings")]
    [SerializeField] public float _dashCooldown = 5f;
    [SerializeField] private float _dashDistance = 5f;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private AudioSource _dashSFX;
    [SerializeField] private GameObject _dashVFX;
    [HideInInspector] public bool _canDash = true;
    [HideInInspector] public float _dashRemainingCD = 0f;
    /*
    [Header("Other")]
    [SerializeField] private GameObject _basicWeapon;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _barrelTransform;
    [SerializeField] private Transform _bulletParent;
    [SerializeField] private AudioSource weaponShoot;
    */
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = _maxAngularVelocity;
        _cameraTranform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        _weaponHolder = this.gameObject.transform.GetChild(0).gameObject;
        _dashSpot = this.gameObject.transform.GetChild(1).gameObject;
        //_activeWeapon = 0;
    }

    private void Update()
    {
        currentVelocity = Mathf.Round(_rigidbody.velocity.magnitude);
        _weaponHolder.transform.rotation = _cameraTranform.rotation;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        if (movementVector.magnitude > 0)
        {
            _tryingToMove = true;
        }
        else
        {
            _tryingToMove = false;
        }
        _movementX = movementVector.x;
        _movementY = movementVector.y;

    }

    private void ApplyMovement()
    {
        //Create a vector from movement values and transform its direction to camera.
        Vector3 direction = _cameraTranform.TransformDirection(_movementY, 0, _movementX * -1);
        //ignore the vectors Y value (Camera up/down) and normalize.
        direction.y = 0f;
        direction = direction.normalized * _torqueForce;
        _rigidbody.AddTorque(direction);
    }


    private void OnBrake(InputValue brake)
    {
        if (brake.isPressed) { _rigidbody.angularDrag = 5; }
        else { _rigidbody.angularDrag = 0.1f; }
    }

    private void OnJump(InputValue jump)
    {

    }

    private void OnDash(InputValue boost)
    {
        //if (!_canDash || !_tryingToMove) { return; }
        if (!_canDash ) { return; }

        //Create a vector from movement values and transform its direction to camera.
        //Vector3 new_direction = _cameraTranform.TransformDirection(_movementX, 0, _movementY);
        //Vector3 new_angularDirection = _cameraTranform.TransformDirection(_movementY, 0, _movementX * -1);
        //ignore the vectors Y value (Camera up/down) and normalize
        Vector3 new_direction = _cameraTranform.forward;
        Vector3 new_angularDirection = new Vector3(new_direction.z, 0, new_direction.x * -1);



        new_direction.y = 0f;
        new_angularDirection.y = 0f;
        new_direction.Normalize();
        new_angularDirection.Normalize();

        //check if there is layer 3 or 6 collision infront of the sphere to avoid dashing out or into objects.
        //Spherecast origin is moved back by 1 (new_direction) to avoid collisions remain undetected if distance is 0.
        RaycastHit hit;
        bool collision = Physics.SphereCast(transform.position, this.GetComponent<SphereCollider>().radius,
            new_direction, out hit, _dashDistance + .5f, _dashLayerMask);

        if (collision)
        {
            _dashSpot.transform.position = transform.position + new_direction * hit.distance;
        }
        else
        {
            _dashSpot.transform.position = transform.position + new_direction * _dashDistance;
        }

        //Player is turned invisible for the duration of the dash so player doest appear twice on the screen.
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(false);

        transform.position = _dashSpot.transform.position;
        Vector3 new_velocity = new_direction * _rigidbody.velocity.magnitude;
        Vector3 new_angularVelocity = new_angularDirection * _rigidbody.angularVelocity.magnitude;
        _rigidbody.velocity = new_velocity;
        _rigidbody.angularVelocity = new_angularVelocity;

        //added coroutine to tplay effects and to turn player visible after a short delay.
        StartCoroutine("DashEffects");

        //resetting dashSpot position to player origin.
        _dashSpot.transform.position = transform.position;

    }

    private IEnumerator DashEffects()
    {
        _canDash = false;
        CameraShake.Instance.DashShake(2f, .2f);
        _dashSFX.Play();
        yield return new WaitForSeconds(.05f);
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(2).gameObject.SetActive(true);
        if (_dashVFX != null) {
            GameObject.Instantiate(_dashVFX, this.transform.position, Quaternion.LookRotation(this.transform.forward));
        }

        //Dash cooldown
        float timestamp = Time.time + _dashCooldown;
        _dashRemainingCD = _dashCooldown;
        while (_dashRemainingCD > 0)
        {
            yield return new WaitForSeconds(1f);
            _dashRemainingCD--;
        }
        _canDash = true;

    }
    /*
    private void OnWeaponSwap(InputValue key)
    {
        if (_activeWeapon == 0)
        {
            _activeWeapon = 1;
            SwapWeapon(1);

        }
        else
        {
            _activeWeapon = 0;
            SwapWeapon(0);
        }
    }

    private void SwapWeapon(int weaponType)
    {
        if (weaponType == 0)
        {
            _basicWeapon.SetActive(true);
        }
        else
        {
            _basicWeapon.SetActive(false);
        }
    }
    */
    /*
    private void OnShoot(InputValue shoot)
    {
        RaycastHit hit;
        LayerMask layerMask = ~(1 << 2 | 1 << 8); //invert layermask for layers 8 (player) and 1 (transparentFX)
        GameObject bullet = GameObject.Instantiate(_bulletPrefab, _barrelTransform.position, Quaternion.identity, _bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        weaponShoot.Play();
        if (Physics.Raycast(_cameraTranform.position, _cameraTranform.forward, out hit, Mathf.Infinity, layerMask))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = _cameraTranform.position + _cameraTranform.forward * 100;
            bulletController.hit = false;
        }
    }
    */
}
