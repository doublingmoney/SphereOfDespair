using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private Health_Bar HPBar;
    [SerializeField] private int _collisionDamage = 0;
    [SerializeField] private GameObject _deathEffect;

    public override void Start()
    {
        currHealth = maxHealth;
        HPBar.SetMaxHP(maxHealth);
    }
    public override void Damage(float damage)
    {
        base.Damage(damage);
        HPBar.SetHealth(currHealth);
    }
    public override void Die()
    {
        base.Die();
        //Debug.Log("Enemy Died!");
        GameEvents.Instance.EnemyDeath();
        Destroy(Instantiate(_deathEffect, this.transform.position, Quaternion.LookRotation(Vector3.up, Vector3.forward)), 3f);
        Destroy(gameObject);
    }

    //prototype only
    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.GetContact(0).otherCollider;
        CharacterStats colliderStats = collider.GetComponent<CharacterStats>();

        if (collider.CompareTag("Player"))
        {
            colliderStats.Damage(_collisionDamage);
            //Debug.Log("Target HP left: " + colliderStats.currHealth);
        }
    }
}