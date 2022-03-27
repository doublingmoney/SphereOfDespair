using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    //Assignables
    public string damageableTag;
    public Rigidbody rigbody;
    public GameObject explosion;
    public LayerMask damageTargetTag;

    //Stats
    [Range(0f, 1f)]
    public float bounceAmmount;
    public bool useGravity;

    //Damage
    public float DamageOfExplosion;
    public float RangeOfExplosion;
    public float ForceOfExplosion;

    //Lifetime
    public int maxCollisions;
    public float maxLifetimeOfBullet;
    public bool explodeBulletOnTouch = true;

    int collisions;
    PhysicMaterial physics_material;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions)
            Explode();

        //Count down lifetime
        maxLifetimeOfBullet -= Time.deltaTime;
        if (maxLifetimeOfBullet <= 0)
            Explode();
    }

    private void Explode()
    {
        //explosion
        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, RangeOfExplosion, damageTargetTag);
        for (int i = 0; i < enemies.Length; i++)
        {
            //Get component of enemy and call Take Damage

            enemies[i].GetComponent<CharacterStats>().Damage(DamageOfExplosion);

            //Add explosion force for rigidbody enemies
            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(ForceOfExplosion, transform.position, RangeOfExplosion);
        }

        //Delay to make sure all works 
        Invoke("Delay", 0.05f);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //so taht bullets doesn`t collide with each other
        if (collision.collider.CompareTag("Projectile"))
            return;

        //Collisions count up
        collisions++;

        //Explode when bullet hits an enemy directly
        if (collision.collider.CompareTag(damageableTag) && explodeBulletOnTouch)
            Explode();
    }

    private void Setup()
    {
        //Create a new Physic material
        physics_material = new PhysicMaterial();
        physics_material.bounciness = bounceAmmount;
        physics_material.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_material.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_material;

        //Set gravity
        rigbody.useGravity = useGravity;
    }

    /// Visualize range of explosion 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, RangeOfExplosion);
    }
}
