using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    public static class Cone
    {
        public static Mesh Generate(float height, float baseWidth, int radialSegments)
        {
            float radius = baseWidth * 0.5f;
            int seg = Mathf.Max(3, radialSegments);

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            // --- Base ring ---
            for (int i = 0; i < seg; i++)
            {
                float a = (i / (float)seg) * Mathf.PI * 2f;
                float x = Mathf.Cos(a) * radius;
                float y = Mathf.Sin(a) * radius;

                verts.Add(new Vector3(x, y, 0f)); // base ring
            }

            // Tip vertex
            int tipIndex = verts.Count;
            verts.Add(new Vector3(0f, 0f, height));

            // Base center
            int baseCenter = verts.Count;
            verts.Add(new Vector3(0f, 0f, 0f));

            // --- Side triangles ---
            for (int i = 0; i < seg; i++)
            {
                int i0 = i;
                int i1 = (i + 1) % seg;

                tris.Add(i0);
                tris.Add(i1);
                tris.Add(tipIndex);
            }

            // --- Base cap ---
            for (int i = 0; i < seg; i++)
            {
                int i0 = i;
                int i1 = (i + 1) % seg;

                tris.Add(baseCenter);
                tris.Add(i1);
                tris.Add(i0);
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