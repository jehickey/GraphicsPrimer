using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sequence
{
    public string Name;
    public bool IsComplete;
    public bool isRunning;
    [SerializeField]
    public List<Step> steps = new List<Step>();
    public int Count;
    public int index = 0;
    public float startTime = 0;
    public float elapsedTime = 0;
    public int frameCount;

    [SerializeField]
    private Dictionary<string, Actor> actors = new Dictionary<string, Actor>();

    public Sequence() {
        Name = "Unnamed Sequence";
    }

    public Actor Get(string name)
    {
        if (!actors.ContainsKey(name)) return null;
        return actors[name];
    }

    public Actor AddActor(string name, Actor actor, bool sleep=false)
    {
        if (!actor) return actor;
        if (sleep) actor.Hide();
        actors[name] = actor;
        return actor;
    }


    public virtual void Load()
    {
        steps.Clear();
        Debug.Log("Sequence initializing");
    }

    public void Run()
    {
        if (IsComplete) isRunning = false;

        if (frameCount == 0) {
            startTime = Time.time;
            isRunning = true;
            Load();
            Debug.Log($"Starting sequence {Name}");
        }
        if (IsComplete) return;
        elapsedTime = Time.time - startTime;
        Count = steps.Count;


        if (steps[index]==null || steps[index].isComplete)
        {
            index++;
        }
        else
        {
            if (steps[index].elapsedTime > 10)
            {
                Debug.Log("A Step exceeded its maximum frameCount without completion");
                index++;
            }
        }
        if (Count == 0 || index >= Count) { IsComplete = true; isRunning = false; return; }
        steps[index].Run();
        frameCount++;
    }

}
