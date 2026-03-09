using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    public static class Cylinder
    {
        public static Mesh Generate(float length, float diameter, int radialSegments, bool capped = true)
        {
            float radius = diameter * 0.5f;
            int seg = Mathf.Max(3, radialSegments);
            float half = length * 0.5f;

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            // --- Rings ---
            for (int i = 0; i < seg; i++)
            {
                float a = (i / (float)seg) * Mathf.PI * 2f;
                float x = Mathf.Cos(a) * radius;
                float y = Mathf.Sin(a) * radius;

                verts.Add(new Vector3(x, y, -half)); // bottom ring
                verts.Add(new Vector3(x, y, half)); // top ring
            }

            // --- Sides ---
            for (int i = 0; i < seg; i++)
            {
                int i0 = i * 2;
                int i1 = ((i + 1) % seg) * 2;

                int b0 = i0;
                int t0 = i0 + 1;
                int b1 = i1;
                int t1 = i1 + 1;

                tris.Add(b0); tris.Add(t0); tris.Add(t1);
                tris.Add(b0); tris.Add(t1); tris.Add(b1);
            }

            // --- Caps ---
            if (capped)
            {
                int bottomCenter = verts.Count;
                verts.Add(new Vector3(0, 0, -half));

                int topCenter = verts.Count;
                verts.Add(new Vector3(0, 0, half));

                // bottom cap
                for (int i = 0; i < seg; i++)
                {
                    int i0 = i * 2;
                    int i1 = ((i + 1) % seg) * 2;

                    tris.Add(bottomCenter);
                    tris.Add(i1);
                    tris.Add(i0);
                }

                // top cap
                for (int i = 0; i < seg; i++)
                {
                    int i0 = i * 2 + 1;
                    int i1 = ((i + 1) % seg) * 2 + 1;

                    tris.Add(topCenter);
                    tris.Add(i0);
                    tris.Add(i1);
                }
            }

            Mesh m = new Mesh();
            m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            m.SetVertices(verts);
            m.SetTriangles(tris, 0);
            m.RecalculateNormals();
            m.RecalculateBounds();

            return m;
        }
    }
}