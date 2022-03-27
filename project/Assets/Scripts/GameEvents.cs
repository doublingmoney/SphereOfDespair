using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    public event Action onEnemyDeath;
    public void EnemyDeath()
    {
        if (onEnemyDeath != null)
        {
            onEnemyDeath();
        }
    }

    public event Action<int> onGoalComplete;
    public void GoalComplete(int id)
    {
        if (onGoalComplete != null)
        {
            onGoalComplete(id);
        }
    }

    public event Action<int, int, int> onGoalUpdate;
    public void GoalUpdate(int id, int current, int max)
    {
        if (onGoalUpdate != null)
        {
            onGoalUpdate(id, current, max);
        }
    }

    public event Action onGoalsCompleted;
    public void GoalsCompleted()
    {
        if (onGoalComplete != null)
        {
            onGoalsCompleted();
        }
    }

    public event Action<int> onPickup;
    public void Pickup(int id)
    {
        if (onPickup != null)
        {
            onPickup(id);
        }
    }

}
