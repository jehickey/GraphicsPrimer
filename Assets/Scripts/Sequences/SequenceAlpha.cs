using UnityEngine;

public class SequenceAlpha : Sequence
{

    public VertexActor vertex1;

    public SequenceAlpha()
    {
        Name = "Sequence Alpha";
    }

    public override void Load()
    {
        base.Load();
        steps.Add(new Step(this));
    }

}
