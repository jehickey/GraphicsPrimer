using System.Globalization;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected Renderer render;
    protected MeshFilter filter;
    public Material material;
    public Color color = Color.white;

    public bool isVisible = true;

    public bool isAnimating = false;
    public Actor Target;
    public bool doRegenerate;

    //movement
    protected Vector3 posLast = Vector3.zero;     //last known position
    protected Vector3 posDelta = Vector3.zero;    //how much recent movement?

    //autoscale - mostly used when appearing or disappearing
    public float Scale = 1f;
    public float scaleTime = .25f;
    [SerializeField]
    private float targetScale;
    [SerializeField]
    protected float currentScale;
    public bool isScaling;
    public bool useScaling=true;
    private float scaleThreshold = .001f;

    private float startTime;
    private float elapsedTime;
    private int frameCount;

    protected virtual void Start()
    {
        render = GetComponent<Renderer>();
        if (!render) render = gameObject.AddComponent<MeshRenderer>();
        filter = GetComponent<MeshFilter>();
        if (!filter) filter = gameObject.AddComponent<MeshFilter>();
        if (!material)
        {
            material = new Material(Shader.Find("Unlit/Color"));
            material.color = Color.white;
        }
        render.sharedMaterial = material;
        posLast = transform.position;

        //currentScale = 0;
        //targetScale = Scale;
        startTime = Time.time;
        frameCount = 0;
    }

    protected virtual void Update()
    {
        elapsedTime = Time.time - startTime;
        if (doRegenerate) Generate();
        posDelta = transform.position - posLast;
        posLast = transform.position;
        UpdateColor();
        UpdateAutoScale();
    }

    protected virtual void LateUpdate()
    {
        frameCount++;
    }

    protected virtual void Generate()
    {
        doRegenerate = false;
    }

    public static T Create<T>(Transform setparent = null, Vector3 pos = new Vector3()) where T : Component
    {
        var obj = new GameObject(typeof(T).Name);
        obj.transform.parent = setparent;
        obj.transform.localPosition = pos;
        return obj.AddComponent<T>();
    }

    public void Show()
    {
        isVisible = true;
    }

    public void Hide()
    {
        isVisible = false;
    }

    private void UpdateColor()
    {
        if (!material) return;
        material.color = color;
    }

    private void UpdateAutoScale()
    {
        targetScale = isVisible ? Scale : 0;

        //don't let Scale or targetScale go out of range
        if (Scale < 0) Scale = 0;
        targetScale = Mathf.Clamp(targetScale, 0, Scale);
        //is it currently a different size than it should be?
        if (currentScale != targetScale)
        {
            if (scaleTime > 0 && useScaling)
            {
                // Base linear speed (same as your earlier growthRate magnitude)
                float baseScaleSpeed = Scale / scaleTime;
                // Normalized distance from target (0..1)
                float scaleFactor = Mathf.Abs(currentScale - targetScale) / Scale;
                // Curve multiplier (fast far away, slow near target)
                float curve = scaleFactor;// * scaleFactor;   //adjustable curve
                float scaleSpeed = baseScaleSpeed * curve;
                // Move currentScale toward targetScale
                currentScale = Mathf.MoveTowards(currentScale, targetScale, scaleSpeed * Time.deltaTime);
                // snap if close enough
                if (Mathf.Abs(currentScale - targetScale) < scaleThreshold) currentScale = targetScale;

            }
            else
            {
                currentScale = targetScale;
            }
        }
        //enforce currentScale within bounds
        currentScale = Mathf.Clamp(currentScale, 0, Scale);
        if (currentScale != targetScale) isScaling = true;
        transform.localScale = Vector3.one * currentScale;
    }




}




