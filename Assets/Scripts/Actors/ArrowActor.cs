using UnityEngine;

public class ArrowActor : Actor
{
    public float width = .1f;
    public float length = 1f;
    public int segments = 16;
    public float headWidthFactor = 2f;
    public float headLengthFactor = .25f;


    protected override void Start()
    {
        base.Start();
        Generate();
    }

    protected override void Update()
    {
        base.Update();
        if (Target) transform.LookAt(Target.transform.position);
    }


    public void Direction (Vector3 dir)
    {
        transform.forward = dir;

    }


    public void PointAt(Actor actor)
    {
        Target = actor;
    }

    protected override void Generate()
    {
        base.Generate();
        //create the arrow mesh
        Mesh cylinder = Shapes.Cylinder.Generate(length, width, segments);
        Mesh cone = Shapes.Cone.Generate(length * headLengthFactor, width * headWidthFactor, segments);
        var combine = new CombineInstance[2];
        combine[0].mesh = cylinder;
        combine[0].transform = Matrix4x4.Translate(new Vector3(0, 0, +length*.5f));
        combine[1].mesh = cone;
        combine[1].transform = Matrix4x4.Translate(new Vector3(0, 0, length));
        Mesh final = new Mesh();
        final.CombineMeshes(combine, true, true);
        filter.mesh = final;
    }

}
