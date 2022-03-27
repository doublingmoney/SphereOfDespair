using UnityEngine;
using UnityEngine.AI;

public class RunnerPatrol : RunnerState
{
    int currentIndex;
    int area;
    public RunnerPatrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 3;
        agent.isStopped = false;
        area = npc.GetComponent<AI>().patrolArea;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        var x = GameEnvironment.Singleton.Checkpoints[area];

        int i = 0;
        foreach (var point in x)
        {
            float distance = Vector3.Distance(npc.transform.position, point.transform.position);
            if (distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
            i++;
        }
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        //Debug.Log("NPC state: " + name.ToString());
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints[area].Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[area][currentIndex].transform.position);
        }
        if (CanSeePlayer())
        {
            nextState = new RunnerPursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}
