using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Step
{
    public string Name;
    [NonSerialized]
    public Sequence seq;
    public bool isComplete;
    public bool isRunning;
    [SerializeField]
    public List<StepAction> actions = new List<StepAction>();
    public int index;
    public float startTime = 0;
    public float elapsedTime = 0;
    public int frameCount;

    public Step(Sequence parent)
    {
        seq = parent;
        actions.Add(
            new StepAction(
                run: () =>
                {
                    Debug.Log("Action 1 launched");
                    //seq.vert1 = spawn.vertex(position, color);      //Actor vert1 would be declared in Sequence
                    //seq.vert.showTriplet = true;
                    //seq.vert.tripletSize = true;
                    //camera.aim(seq.vert1);
                },
                conditional: () => { return true; }
            )
        );
        actions.Add(
            new StepAction(
                run: () =>
                {
                    Debug.Log("Action 2 launched");
                    //seq.vertex1 = spawn.vertex(position, color);      //Actor vert1 would be declared in Sequence
                    //seq.vert.showTriplet = true;
                    //seq.vert.tripletSize = true;
                    //camera.aim(seq.vert1);
                },
                conditional: () => { return true; }
            )
        );


    }


    public void Run()
    {
        if (isComplete) isRunning = false;
        if (frameCount == 0)
        {
            startTime = Time.time;
            Debug.Log($"Starting step {Name}");
            isRunning = true;
        }

        elapsedTime = Time.time - startTime;

        if (actions.Count == 0) isComplete = true;

        if (isComplete) return;

        if (actions[index].IsComplete)
        {
            index++;         //if current is done, move on
        }
        else
        {
            if (actions[index].elapsedTime > 10)
            {
                Debug.Log("An action exceeded its maximum frameCount without completion");
                index++;
            }
        }
        if (index >= actions.Count)
        {
            isComplete = true;
            return;
        }
        actions[index].Run();
        frameCount++;
    }
}
