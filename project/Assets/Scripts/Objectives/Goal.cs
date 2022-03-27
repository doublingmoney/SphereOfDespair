using UnityEngine;

public abstract class Goal : MonoBehaviour
{
    public abstract bool IsAchieved();
    public abstract void Complete();
    public abstract void GoalUpdate();
    public abstract void DrawHUD();
}
