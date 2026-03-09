using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class OrientationWidget : Actor
{
    public float arrowWidth = .1f;
    public float arrowLength = 1f;
    public int arrowSegments = 16;
    public float headWidthFactor = 2f;
    public float headLengthFactor = .25f;

    public float rescaleMax = 2f;                   //maximum scale (if vector = 1)
    public float rescaleMin = .5f;                  //minimum - default scale
    public float rescaleFactor = 1f;                //how much to scale sizes based on movement
    public float rescaleAttack = .5f;               //maximum magnitude increase
    public float rescaleDecay = .25f;               //magnitude decay per second
    public Vector3 rescaleVector = Vector3.zero;   //controls animated scaling factors
    public Vector3 rescaleDeformation = Vector3.zero;   //controls how arrows are rescaled

    private ArrowActor up;
    private ArrowActor right;
    private ArrowActor forward;

    protected override void Start()
    {
        base.Start();
        up = Actor.Create<ArrowActor>();
        right = Actor.Create<ArrowActor>();
        forward = Actor.Create<ArrowActor>();

        foreach (var dir in new[] { up, right, forward })
        {
            dir.transform.parent = transform;
            dir.transform.localPosition = Vector3.zero;
        }

        up.Direction(Vector3.up);
        up.color = Color.green;
        right.Direction(Vector3.right);
        right.color = Color.red;
        forward.Direction(Vector3.forward);
        forward.color = Color.blue;

    }

    protected override void Update()
    {
        if (doRegenerate)
        {
            up.doRegenerate = true;
            right.doRegenerate = true;
            forward.doRegenerate = true;
        }
        base.Update();
        updateArrowProperties();
        updateArrowScaling();
        applyArrowScaling();
    }

    private void updateArrowProperties()
    {
        foreach (var dir in new[] { up, right, forward })
        {
            dir.width = arrowWidth;
            dir.length = arrowLength;
            dir.segments = arrowSegments;
            dir.headWidthFactor = headWidthFactor;
            dir.headLengthFactor = headLengthFactor;
        }
    }

    //rescale arrows based on recent movement
    private void updateArrowScaling()
    {
        //should be rewritten so rescalevector is limited to 1 (not normalized)

        // apply decay (max shrink per second)
        rescaleVector -= rescaleVector * rescaleDecay * Time.deltaTime;

        //movement contribution (unsigned per-axis)
        Vector3 growth = new Vector3(
            Mathf.Abs(posDelta.x),
            Mathf.Abs(posDelta.y),
            Mathf.Abs(posDelta.z)
        ) * rescaleFactor;

        // apply growth limit (max growth per second)
        growth.x = Mathf.Min(growth.x, rescaleAttack * Time.deltaTime);
        growth.y = Mathf.Min(growth.y, rescaleAttack * Time.deltaTime);
        growth.z = Mathf.Min(growth.z, rescaleAttack * Time.deltaTime);

        // update rescaleVector
        rescaleVector += growth;

        // clamp to allowed range
        rescaleVector.x = Mathf.Clamp(rescaleVector.x, rescaleMin, rescaleMax);
        rescaleVector.y = Mathf.Clamp(rescaleVector.y, rescaleMin, rescaleMax);
        rescaleVector.z = Mathf.Clamp(rescaleVector.z, rescaleMin, rescaleMax);
    }


    private void applyArrowScaling()
    {
        //ideally arrows this hand scaling for themselves
        up.transform.localScale = Vector3.one + rescaleDeformation * rescaleVector.y * currentScale;
        right.transform.localScale = Vector3.one + rescaleDeformation * rescaleVector.x * currentScale;
        forward.transform.localScale = Vector3.one + rescaleDeformation * rescaleVector.z * currentScale;
    }
}
