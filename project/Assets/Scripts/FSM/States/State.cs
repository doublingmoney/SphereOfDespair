using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Animator anim;
    protected Transform player;
    protected State nextState;
    protected State lastState;
    protected NavMeshAgent agent;

    float visDist = 60f;
    float visAngle = 120f;
    float shootDist = 25f;
    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        stage = EVENT.ENTER;
        player = _player;
    }
    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            lastState = this;
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            LayerMask layerMask = ~(1 << 10);
            Physics.Linecast(npc.transform.position, player.transform.position, out RaycastHit hitInfo, layerMask, QueryTriggerInteraction.Ignore);
            //Debug.Log(hitInfo.collider.name.ToString());
            if (hitInfo.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
        {
            LayerMask layerMask = ~(1 << 10);
            Physics.Linecast(npc.transform.position, player.transform.position, out RaycastHit hitInfo, layerMask, QueryTriggerInteraction.Ignore);
            //Debug.Log(hitInfo.collider.name.ToString());
            if (hitInfo.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


}
