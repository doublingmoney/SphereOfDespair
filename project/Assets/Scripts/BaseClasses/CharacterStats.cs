using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float maxHealth = 100;
    public float currHealth;


    public bool isDead = false;

    public virtual void Start()
    {
        currHealth = maxHealth;
    }
    public virtual void CheckHealth()
    {
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
        }
    }

    public virtual void Damage(float damage)
    {
        currHealth -= damage;
        CheckHealth();
        if (isDead)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Override.
    }
}
