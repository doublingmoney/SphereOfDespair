using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    float rotationSpeed;
    EnemyShoot shootController;
    AudioSource shootSFX;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.ATTACK;
        shootSFX = _npc.GetComponent<AudioSource>();
        shootController = npc.GetComponent<EnemyShoot>();
        rotationSpeed = 2f;
    }

    public override void Enter()
    {

        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        shootSFX.Play();
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("NPC state: " + name.ToString());
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;
        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        
        shootController.Shoot(player);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        shootSFX.Stop();
        base.Exit();
    }
}
