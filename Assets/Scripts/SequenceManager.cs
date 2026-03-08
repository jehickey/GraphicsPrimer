using System;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
    [SerializeField]
    public List<Sequence> sequences = new List<Sequence>();
    public int index = 0;
    public int frameCount = 0;
    public bool isComplete;

    void Start()
    {
        if (sequences.Count == 0) Add(new SequenceAlpha());
    }

    void Update()
    {
        //if there's no sequences, we're done.
        if (sequences.Count == 0) isComplete= true;

        //all out of sequences
        if (isComplete)
        {
            return;
        }

        //if current sequence is complete (or null) move on
        if (sequences[index]!=null && sequences[index].IsComplete)
        {
            index++;
        }
        else
        {
            if (sequences[index].elapsedTime > 10)
            {
                Debug.Log("A Sequence exceeded its maximum frameCount without completion");
                index++;
            }
        }
        if (index >= sequences.Count)
        {
            isComplete = true;
            Debug.Log("SequenceManager complete!");
            return;
        }
        sequences[index].Run();

    }

    void Add(Sequence sequence)
    {
        sequences.Add(sequence);
        sequence.Load();
    }

}
