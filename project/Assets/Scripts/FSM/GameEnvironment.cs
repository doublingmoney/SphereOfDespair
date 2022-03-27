using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GameEnvironment
{
    private static GameEnvironment instance;
    private List<GameObject> _checkpoints = new List<GameObject>();
    private List<List<GameObject>> _patrolPoints = new List<List<GameObject>>();
    public List<List<GameObject>> Checkpoints { get { return _patrolPoints; } }

    public static GameEnvironment Singleton
    {
        get
        {
            if (instance != null) instance = null;
            if (instance == null)
            {
                instance = new GameEnvironment();
                instance._checkpoints.AddRange(GameObject.FindGameObjectsWithTag("PatrolArea"));
                instance._checkpoints = instance._checkpoints.OrderBy(waypoint => waypoint.name).ToList();

                instance.CreateList();
            }
            return instance;
        }
    }

    private void CreateList()
    {
        foreach (var obj in instance._checkpoints)
        {
            List<GameObject> gs = new List<GameObject>();
            Transform[] ts = obj.GetComponentsInChildren<Transform>();

            foreach (Transform t in ts)
            {
                if (t != null && t.gameObject != null)
                    gs.Add(t.gameObject);
            }
            _patrolPoints.Add(gs);
        }
        //Debug.Log("patorl points found: " + instance._checkpoints.Count);

    }
}
