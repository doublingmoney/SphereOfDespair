using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{

    [SerializeField]
    private GameObject _bulletDecal;
    [SerializeField]
    private Transform _decalParent;
    [SerializeField]
    private GameObject _ImpactParticleSystem;

    [SerializeField]
    private float _speed = 50f;
    [SerializeField]
    private float _timeToDestroy = 3f;
    [SerializeField]
    private float _damage = 10f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void Start()
    {
        Destroy(gameObject, _timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < .0001f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Respawn" || collision.gameObject.layer == 1)
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

        ContactPoint contact = collision.GetContact(0);
        if (_ImpactParticleSystem != null)
        {
            _ImpactParticleSystem.transform.position = contact.point;
            _ImpactParticleSystem.transform.forward = contact.normal;
            Destroy(GameObject.Instantiate(_ImpactParticleSystem, contact.point + contact.normal, Quaternion.LookRotation(contact.normal)), 5f);
        }

        CharacterStats colliderStats = contact.otherCollider.GetComponent<CharacterStats>();
        if (colliderStats != null && !contact.otherCollider.CompareTag("Explosive"))
        {
            colliderStats.Damage(_damage);
            //Debug.Log("Target HP left: " + colliderStats.currHealth);
            Destroy(gameObject);

        }
        else
        {
            //Debug.Log("target missed");
            Destroy(gameObject);
        }



    }
    /*
    private void OnTriggerEnter(Collider collider)
    {
        //if collider = enemy, decrease health, play hit effect
        //if collider != enemy, play hit effet
        CharacterStats hitStats = collider.GetComponent<CharacterStats>();
        if(hitStats != null)
        {
            hitStats.Damage(_damage);
            Debug.Log("Target HP left: " + hitStats.currHealth);
            Destroy(gameObject);

        } else
        {
            Debug.Log("target missed");
            Destroy(gameObject);
        }
    }*/
}
