using UnityEngine;
using UnityEngine.AI;

public class RunnerPursue : RunnerState
{

    float explodeDistance;
    public RunnerPursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 10;
        explodeDistance = 3;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("NPC state: " + name.ToString());
        agent.SetDestination(player.position);
        
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
               
                Explode(6,55,15);
                this.npc.GetComponent<CharacterStats>().Die();
            

                nextState = new RunnerIdle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new RunnerPatrol(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
    }
    public void Explode(float explosionRadius, float damage, float explosionForce)
    {
        //explosion trigger to apply forces to rigidbodies and to deal damage
        Collider[] surroundingObjects = Physics.OverlapSphere(npc.transform.position, explosionRadius);

        foreach (var obj in surroundingObjects)
        {

            float dist = Vector3.Distance(npc.transform.position, obj.transform.position);
            float ratio = Mathf.Clamp01(1 - dist / explosionRadius);

            var stats = obj.GetComponent<CharacterStats>();
            if (stats == null || stats.isDead || obj.CompareTag("Enemy")) { }
            else
            {
                stats.Damage(damage * ratio);
            }

            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null || obj.CompareTag("Projectile")) continue;
            rb.AddExplosionForce(explosionForce * ratio, npc.transform.position, explosionRadius, 0, ForceMode.Impulse);
        }
    }
    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}
