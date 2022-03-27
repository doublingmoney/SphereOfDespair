using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform player;
    State currentState;
    RunnerState currentState1;
    [Header("0 = patrol, 1 = runner")]
    public int agentType;
    public int patrolArea = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (agentType == 0)
        {
            currentState = new Idle(gameObject, agent, anim, player);
        } else if (agentType == 1)
        {
            currentState1 = new RunnerIdle(gameObject, agent, anim, player);
        }
        
    }

    private void Update()
    {
        if(currentState != null)
        {
            currentState = currentState.Process();
        }
        if (currentState1 != null)
        {
            currentState1 = currentState1.Process();
        }
    }
    
}
