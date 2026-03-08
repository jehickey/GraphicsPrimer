using System;
using UnityEngine;

public class StepAction
{
    public readonly Action run;
    public readonly Func<bool> conditional;    // optional
    public bool runEveryFrame;
    public bool IsComplete;
    public bool isRunning;
    public int frameCount;
    public float startTime;
    public float elapsedTime;



    public StepAction (Action run, Func<bool> conditional = null)
    {
        this.run = run;
        this.conditional = conditional;

        // If no conditional is given complete immediately after first update
        runEveryFrame = conditional != null;
    }

    public void Run()
    {
        if (IsComplete) isRunning = false;
        if (IsComplete) return;

        if (frameCount == 0)
        {
            startTime = Time.time;
            isRunning = true;
            Debug.Log($"Starting action");
        }

        //bool runComplete = run?.Invoke() ?? false;
        if (frameCount==0 || runEveryFrame) run();


        // If no conditional, complete after first frame
        if (conditional == null && frameCount > 0)
        {
            IsComplete = true;
            return;
        }

        // If conditional exists, check it
        if (conditional != null) IsComplete = conditional();

        frameCount++;
    }
}
