using System;
using UnityEngine;

[Serializable]
public class StepAction
{
    public Action run;
    public Func<StepAction,bool> conditional;    // optional
    public bool runEveryFrame;
    public bool IsComplete;
    public bool isRunning;
    public int frameCount;
    public float startTime;
    public float elapsedTime;


    public StepAction()
    {
        //this.run = () => { };
        //this.conditional = () => { return true; };

    }

    public StepAction (Action run, Func<StepAction,bool> conditional = null)
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
        elapsedTime = Time.time - startTime;

        //bool runComplete = run?.Invoke() ?? false;
        if (frameCount==0 || runEveryFrame) run();


        // If no conditional, complete after first frame
        if (conditional == null && frameCount > 0)
        {
            IsComplete = true;
            return;
        }

        // If conditional exists, check it
        if (conditional != null) IsComplete = conditional(this);
        if (IsComplete) isRunning = false;

        frameCount++;
    }

    public bool Wait(float timer)
    {

        return elapsedTime >= timer;
    }

}
