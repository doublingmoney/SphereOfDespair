using UnityEngine;
using UnityEngine.AI;

public class RunnerAttack : RunnerState
{
    float rotationSpeed;
    float explodeDistance;
    EnemyShoot shootController;
    AudioSource shootSFX;

    public RunnerAttack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        shootSFX = _npc.GetComponent<AudioSource>();
        rotationSpeed = 2f;
    }

    public override void Enter()
    {

        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("NPC state: " + name.ToString());
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

        //Explode(6, 59, 15);

        if (!CanAttackPlayer())
        {
            nextState = new RunnerIdle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        shootSFX.Stop();
        base.Exit();
    }

    private void Explode(float explosionRadius, float damage, float explosionForce)
    {
        //explosion trigger to apply forces to rigidbodies and to deal damage
        Collider[] surroundingObjects = Physics.OverlapSphere(npc.transform.position, explosionRadius);

        foreach (var obj in surroundingObjects)
        {

            float dist = Vector3.Distance(npc.transform.position, obj.transform.position);
            float ratio = Mathf.Clamp01(1 - dist / explosionRadius);

            var stats = obj.GetComponent<CharacterStats>();
            if (stats == null || stats.isDead || !obj.CompareTag("Enemy")) { }
            else
            {
                stats.Damage(damage * ratio);
            }

            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null || obj.CompareTag("Projectile")) continue;
            rb.AddExplosionForce(explosionForce * ratio, npc.transform.position, explosionRadius, 0, ForceMode.Impulse);
        }
        //npc.gameObject.GetComponent<CharacterStats>().Die();
    }
}
