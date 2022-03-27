using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public Goal[] goals;

    private int _goalsCompleted;

    private void Awake()
    {
        goals = GetComponents<Goal>();
    }

    private void Start()
    {
        GameEvents.Instance.onGoalComplete += AddCompletedGoal;
    }

    private void AddCompletedGoal(int id)
    {
        _goalsCompleted++;
    }
    /*
    private void OnGUI()
    {
        foreach (var goal in goals)
        {
            goal.DrawHUD();
        }
    }
    */

    private void Update()
    {
        //Debug.Log(goals.Length);
        if (goals.Length == _goalsCompleted)
        {
            GameEvents.Instance.GoalsCompleted();
        }
        foreach (var goal in goals)
        {
            if (goal.IsAchieved())
            {
                goal.Complete();
                Destroy(goal);
            }
        }
    }

}
