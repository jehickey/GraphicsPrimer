using UnityEngine;

public class VertexActor : Actor
{
    protected override void Start()
    {
        base.Start();
        Mesh mesh = Shapes.Icosphere.Generate(4);
        filter.mesh = mesh;
    }

    protected override void Update()
    {
        
    }




}
