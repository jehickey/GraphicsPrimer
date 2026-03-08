using UnityEngine;

public class Actor : MonoBehaviour
{
    protected Renderer render;
    protected MeshFilter filter;
    public Material material;

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
    }

    protected virtual void Update()
    {
    }

    public static T Create<T>(Vector3 pos) where T : Component
    {
        var obj = new GameObject(typeof(T).Name);
        obj.transform.position = pos;
        return obj.AddComponent<T>();
    }

}
