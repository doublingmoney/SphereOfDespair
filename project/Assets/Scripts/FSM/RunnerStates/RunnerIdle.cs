using UnityEngine;
using UnityEngine.AI;

public class RunnerIdle : RunnerState
{
    public RunnerIdle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player) : base(_npc, _agent, _anim, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();

    }

    public override void Update()
    {
        //Debug.Log("NPC state: " + name.ToString());
        if (CanSeePlayer())
        {
            nextState = new RunnerPursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
        else if (Random.Range(0, 100) < 5)
        {
            nextState = new RunnerPatrol(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
