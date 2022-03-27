using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletControllerAIVariant : MonoBehaviour
{
    [SerializeField]
    private GameObject _ImpactParticleSystem;

    [SerializeField]
    private float _speed = 50f;
    [SerializeField]
    private float _timeToDestroy = 5f;
    [SerializeField]
    private float _damage = 10f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }
    private  Rigidbody _rb;
    public Vector3 direction { get; set; }
    bool forceApplied = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(gameObject, _timeToDestroy);
    }

    void FixedUpdate()
    {
        if(direction != null)
        {
            if (!forceApplied)
            {
                Fire();
                forceApplied = true;
            }
        }
    }
    public void Fire()
    {
        _rb.AddForce(direction.normalized * _speed, ForceMode.Impulse);
    }
  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Respawn" || collision.gameObject.layer == 1 || collision.gameObject.layer == 10)
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

        ContactPoint contact = collision.GetContact(0);

        _ImpactParticleSystem.transform.position = contact.point;
        _ImpactParticleSystem.transform.forward = contact.normal;
        //Destroy(GameObject.Instantiate(_ImpactParticleSystem, contact.point + contact.normal, Quaternion.LookRotation(contact.normal)), 5f);

        CharacterStats colliderStats = contact.otherCollider.GetComponent<CharacterStats>();
        if (colliderStats != null && !contact.otherCollider.CompareTag("Explosive"))
        {
            colliderStats.Damage(_damage);
            //Debug.Log("Target HP left: " + colliderStats.currHealth);
            Destroy(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
