using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
    public static class Icosphere
    {
        private const int MaxSubdivisions = 5;

        public static Mesh Generate(int subdivisions)
        {
            if (subdivisions > MaxSubdivisions) subdivisions = MaxSubdivisions;
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            CreateIcosahedron(verts, tris);

            Dictionary<long, int> midpointCache = new Dictionary<long, int>();

            for (int i = 0; i < subdivisions; i++)
                Subdivide(verts, tris, midpointCache);

            // Normalize to unit sphere
            for (int i = 0; i < verts.Count; i++)
                verts[i] = verts[i].normalized;

            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        //base icosahedron
        private static void CreateIcosahedron(List<Vector3> verts, List<int> tris)
        {
            verts.Clear();
            tris.Clear();

            float t = (1f + Mathf.Sqrt(5f)) * 0.5f;

            verts.Add(new Vector3(-1, t, 0));
            verts.Add(new Vector3(1, t, 0));
            verts.Add(new Vector3(-1, -t, 0));
            verts.Add(new Vector3(1, -t, 0));

            verts.Add(new Vector3(0, -1, t));
            verts.Add(new Vector3(0, 1, t));
            verts.Add(new Vector3(0, -1, -t));
            verts.Add(new Vector3(0, 1, -t));

            verts.Add(new Vector3(t, 0, -1));
            verts.Add(new Vector3(t, 0, 1));
            verts.Add(new Vector3(-t, 0, -1));
            verts.Add(new Vector3(-t, 0, 1));

            // Normalize base vertices
            for (int i = 0; i < verts.Count; i++)
                verts[i] = verts[i].normalized;

            int[] faces = {
                0,11,5,  0,5,1,  0,1,7,  0,7,10, 0,10,11,
                1,5,9,   5,11,4, 11,10,2, 10,7,6, 7,1,8,
                3,9,4,   3,4,2,  3,2,6,  3,6,8,  3,8,9,
                4,9,5,   2,4,11, 6,2,10, 8,6,7,  9,8,1
            };

            tris.AddRange(faces);
        }

        // Subdivision
        private static void Subdivide(
            List<Vector3> verts,
            List<int> tris,
            Dictionary<long, int> midpointCache)
        {
            midpointCache.Clear();
            List<int> newTris = new List<int>(tris.Count * 4);

            for (int i = 0; i < tris.Count; i += 3)
            {
                int v1 = tris[i];
                int v2 = tris[i + 1];
                int v3 = tris[i + 2];

                int a = GetMidpoint(v1, v2, verts, midpointCache);
                int b = GetMidpoint(v2, v3, verts, midpointCache);
                int c = GetMidpoint(v3, v1, verts, midpointCache);

                newTris.AddRange(new int[] {
                    v1, a, c,
                    v2, b, a,
                    v3, c, b,
                    a, b, c
                });
            }

            tris.Clear();
            tris.AddRange(newTris);
        }

        //midpoint helper
        private static int GetMidpoint(
            int i1, int i2,
            List<Vector3> verts,
            Dictionary<long, int> cache)
        {
            long key = i1 < i2
                ? ((long)i1 << 32) + i2
                : ((long)i2 << 32) + i1;

            if (cache.TryGetValue(key, out int cached))
                return cached;

            Vector3 midpoint = (verts[i1] + verts[i2]) * 0.5f;
            int newIndex = verts.Count;
            verts.Add(midpoint);

            cache[key] = newIndex;
            return newIndex;
        }
    }
}