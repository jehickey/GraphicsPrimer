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

    public Sequence() {
        Name = "Unnamed Sequence";
    }

    public virtual void Load()
    {
        steps.Clear();
    }

    public void Run()
    {
        if (IsComplete) isRunning = false;

        if (frameCount == 0) {
            startTime = Time.time;
            isRunning = true;
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
