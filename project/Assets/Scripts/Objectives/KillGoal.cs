using UnityEngine;

public class KillGoal : Goal
{
    [SerializeField] private int id;
    [SerializeField] private int requiredKills = 25;
    [SerializeField] int kills = 0;

    private void Start()
    {
        GameEvents.Instance.onEnemyDeath += AddKillCount;
        GoalUpdate();
    }

    private void OnDisable()
    {
        GameEvents.Instance.onEnemyDeath -= AddKillCount;
    }

    private void AddKillCount()
    {
        kills++;
        GoalUpdate();
    }

    public override void GoalUpdate()
    {
        //Debug.Log("updating goal");
        GameEvents.Instance.GoalUpdate(id, kills, requiredKills);
    }
    public override void Complete()
    {
        GameEvents.Instance.GoalComplete(id);
    }

    public override void DrawHUD()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 50;
        GUILayout.Label(string.Format("Enemies destroyed {0}/{1}", kills, requiredKills), style);
    }

    public override bool IsAchieved()
    {
        return (kills >= requiredKills);
    }
}
