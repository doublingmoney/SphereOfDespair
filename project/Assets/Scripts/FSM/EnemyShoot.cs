using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform[] _barrels;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private AudioSource _shootSFX;
    [SerializeField] private float shootCD = 0.75f;
    [SerializeField] private GameObject _muzzleFlash;
    private int _barrelCount;
    private int _nextBarrelID;
    private Transform _nextBarrel;
    [SerializeField] private LayerMask _rayIgnoreLayer;

    float shootCDTimer;
    [SerializeField] private float _forwardForce = 1f;
    [SerializeField] private float _upwardForce = 0f;

    private void Start()
    {
        _barrelCount = _barrels.Length;
        _nextBarrelID = 0;
        shootCDTimer = 0;
    }

    public void Shoot(Transform target)
    {
        shootCDTimer -= Time.deltaTime;
        if (shootCDTimer > 0) return;
        shootCDTimer = shootCD;

        if (_nextBarrelID >= _barrelCount)
        {
            _nextBarrelID = 0;
        }
        _nextBarrel = _barrels[_nextBarrelID];
        _nextBarrelID++;

        Vector3 direction = Vector3.Normalize(target.position - _nextBarrel.transform.position);
        Ray ray = new Ray(_nextBarrel.transform.position, direction);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~_rayIgnoreLayer, QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
        } else 
        {
            targetPoint = ray.GetPoint(75);
        }

        //instantiate the bullet, get its rigid body, rotate it to shooting direction and apply forces
        GameObject bullet = Instantiate(_bulletPrefab, _nextBarrel.position, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bullet.transform.forward = direction;

        bulletRb.AddForce(direction * _forwardForce, ForceMode.Impulse);
        bulletRb.AddForce(direction * _upwardForce, ForceMode.Impulse);

        //muzzle flash and sound effect
        if (_shootSFX != null) _shootSFX.Play();
        if (_muzzleFlash != null) Instantiate(_muzzleFlash, _nextBarrel.forward, Quaternion.identity);
    }
}
