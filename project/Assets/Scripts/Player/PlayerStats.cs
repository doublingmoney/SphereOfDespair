public class PlayerStats : CharacterStats
{

    public override void Start()
    {

        currHealth = maxHealth;
    }

    public void Heal(float amount)
    {
        currHealth += amount;
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.PlayerDied();
    }
}
