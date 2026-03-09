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
        Add(() =>
                {
                    Debug.Log("Action 1 launched");
                    //seq.Get("vertex1").Wake();
                    seq.AddActor("Vertex1", Actor.Create<VertexActor>(null, new Vector3(1, 1, 1)));
                    //seq.vert.showTriplet = true;
                    //seq.vert.tripletSize = true;
                    //camera.aim(seq.vert1);
                },
         (self) => { return self.Wait(1); });

        Add(() =>
        {
            Debug.Log("Action 2 launched");
            //seq.Get("Vertex1").transform.position = new Vector3(1, 1, 1);
            seq.AddActor("Vertex2", VertexActor.Create<VertexActor>(null, new Vector3(0, 0, 0)));
        },
        (self) => { return self.Wait(1); });

        Add(() =>
        {
            Debug.Log("Action 3 launched");
            //seq.Get("Vertex1").transform.position = new Vector3(1, 1, 1);
            seq.AddActor("Vertex3", VertexActor.Create<VertexActor>(null, new Vector3(-1, -1, -1)));
        },
         (self) => { return self.Wait(0); });

    }

    public void Add(Action run, Func<StepAction, bool> conditional)
    {
        StepAction action = new StepAction();
        actions.Add(action);
        action.run = run;
        actions[0].conditional = conditional;
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
            isRunning = false;
            return;
        }
        actions[index].Run();
        frameCount++;
    }


    public bool xWait(float timer)
    {

        return elapsedTime >= timer;
    }

}
