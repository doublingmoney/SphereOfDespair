using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletVersion2 : MonoBehaviour
{
    [SerializeField] private string _damageableTag;
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private AudioSource _explosionSFX;
    [SerializeField] private LayerMask _explosionDamageableLayer;
    [SerializeField] private LayerMask _linecastLayer;

    [Header("Weapon Stats")]
    [Range(0f, 1f)]
    [SerializeField] private float bounceAmmount;
    [SerializeField] private bool useGravity;

    //Damage
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _explosionRadius = 0.1f;
    [SerializeField] private float _explosionForce = 0f;

    //Bullet lifetime
    [SerializeField] private int _collisionsToExplode = 0;
    [SerializeField] private float _maxBulletLifeTime = 5f;
    [SerializeField] private bool _explodeOnTouch = true;
    [SerializeField] private bool _ignorePlayerCollision = false;

    private int _collisions;
    private PhysicMaterial _physicsMaterial;
    private Rigidbody _rigidBody;
    private float _bulletLifeTime; //debug variable

    private Vector3 _oldPos;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        //Create a new Physic material
        _physicsMaterial = new PhysicMaterial();
        _physicsMaterial.bounciness = bounceAmmount;
        _physicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        _physicsMaterial.bounceCombine = PhysicMaterialCombine.Maximum;
        _bulletLifeTime = 0;
        _oldPos = transform.position;

        //Assign material to collider
        GetComponent<SphereCollider>().material = _physicsMaterial;

        //Set gravity
        _rigidBody.useGravity = useGravity;
    }

    private void Update()
    {   

        
         if (_collisions > _collisionsToExplode)
         {
            //Debug.Log("exploded due to collision limit"); 
            Explode();
         }

         _maxBulletLifeTime -= Time.deltaTime;
         if (_maxBulletLifeTime <= 0)
         {
            //Debug.Log("exploded due to time limit");
            Explode();
         }
       
    }

    private void LateUpdate()
    {
        _bulletLifeTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //Move the object forward here...
        if (Physics.Linecast(_oldPos, transform.position, out RaycastHit Hit, ~_linecastLayer))
        {
            transform.position = Hit.point;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile")) { return; }
        if (_ignorePlayerCollision && collision.collider.CompareTag("Player")) { return; }
        //Debug.Log("collider: " + collision.collider.name);
        _collisions++;

        if (collision.collider.CompareTag(_damageableTag) && _explodeOnTouch)
        {
            //Debug.Log("Exploded due to collision with enenmey tag, name: " + collision.collider.name);
            Explode();
        }
    }

    private void Explode()
    {
        if(_explosionVFX != null)
        {
            Destroy(Instantiate(_explosionVFX, transform.position, Quaternion.identity), 3f);
        }

        //Check for damageable objects in explosion radius
        Collider[] targets = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionDamageableLayer);

        foreach(var obj in targets)
        {
            //Debug.Log(obj.name);
            var objRb = obj.GetComponent<Rigidbody>();
            var objStats = obj.GetComponent<CharacterStats>();

            if(objStats != null)
            {
                objStats.Damage(_damage);
            }
            if(objRb != null)
            {
                objRb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 0f, ForceMode.Impulse);
            }
        }
        Destroy(this.gameObject);
    }
}
