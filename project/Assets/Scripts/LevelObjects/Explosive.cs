using System.Collections;
using UnityEngine;

public class Explosive : CharacterStats
{
    [Header("logic settings")]
    [SerializeField] private float _triggerForce = 20f;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float _upwardsModifier = 5f;
    [SerializeField] private float _explosionForce = 500f;
    [SerializeField] private float _damage = 75f;
    [SerializeField] private float _explosionDelay = 0.2f;

    [Header("Effects")]
    [SerializeField] private GameObject _particle;

    //[SerializeField] private GameObject _destroyed;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).otherCollider.CompareTag("Projectile") || collision.relativeVelocity.magnitude >= _triggerForce)
        {
            this.Damage(maxHealth);
        }
    }
    public override void Damage(float damage)
    {

        currHealth -= damage;
        StartCoroutine("ExplosionDelay");
    }

    private IEnumerator ExplosionDelay()
    {
        yield return new WaitForSeconds(_explosionDelay);
        CheckHealth();
    }
    public override void CheckHealth()
    {
        if (currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
            Die();
        }
    }
    public override void Die()
    {
        base.Die();
        PlayEffects();
        Explode();
    }

    private void Explode()
    {
        //explosion trigger to apply forces to rigidbodies and to deal damage
        Collider[] surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var obj in surroundingObjects)
        {

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            float ratio = Mathf.Clamp01(1 - dist / _explosionRadius);

            var stats = obj.GetComponent<CharacterStats>();
            if (stats == null || stats.isDead) { }
            else
            {
                stats.Damage(_damage * ratio);
            }

            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null || obj.CompareTag("Projectile")) continue;
            rb.AddExplosionForce(_explosionForce * ratio, transform.position, _explosionRadius, _upwardsModifier, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }

    public void PlayEffects()
    {
        Destroy(Instantiate(_particle, transform.position, Quaternion.identity), 5f);
        //play destoy animation
    }
}
