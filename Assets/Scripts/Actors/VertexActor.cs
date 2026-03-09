using UnityEngine;

public class VertexActor : Actor
{
    public bool ShowWidget = false;
    private OrientationWidget widget;


    protected override void Start()
    {
        base.Start();
        Mesh mesh = Shapes.Icosphere.Generate(4);
        filter.mesh = mesh;

        widget = Actor.Create<OrientationWidget>(transform);
        widget.Scale = Scale * 2;
    }

    protected override void Update()
    {
        base.Update();
        widget.isVisible = ShowWidget;
    }




}
