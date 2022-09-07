using BaseX;
using System.Collections.Generic;

namespace FrooxEngine
{
    public class STetrahedron
    {
        private readonly static float s8_9 = MathX.Sqrt(8f / 9f);
        private readonly static float s2_9 = MathX.Sqrt(2f / 9f);
        private readonly static float s2_3 = MathX.Sqrt(2f / 3f);
        private readonly static float f1_3 = 1f / 3f;
        private List<float3> centers = new List<float3>();
        public int subdivisions = 1;
        private STetrahedron Subdivide()
        {
            var result = new STetrahedron();
            float s = result.subdivisions = (int)(subdivisions * 0.5f);
            if (centers.Count == 0)
                centers.Add(float3.Zero);
            foreach (var c in centers)
            {
                result.centers.Add(c + new float3(0, s, 0));
                result.centers.Add(c + new float3(-s2_3 * s, -f1_3 * s, -s2_9 * s));
                result.centers.Add(c + new float3(s2_3 * s, -f1_3 * s, -s2_9 * s));
                result.centers.Add(c + new float3(0, -f1_3 * s, s8_9 * s));
            }
            return result;
        }

        private STetrahedron Subdivide(int aCount)
        {
            var res = this;
            for (int i = 0; i < aCount; i++)
                res = res.Subdivide();
            return res;
        }

        public MeshX CreateMesh(int subdivisions = 1)
        {
            this.subdivisions = subdivisions;
            var meshX = new MeshX();
            meshX.SetVertexCount(centers.Count * 12);
            float s = subdivisions;
            int i = 0;
            foreach (var c in centers)
            {
                var v0 = c + new float3(0, s, 0);
                var v1 = c + new float3(-s2_3 * s, -f1_3 * s, -s2_9 * s);
                var v2 = c + new float3(s2_3 * s, -f1_3 * s, -s2_9 * s);
                var v3 = c + new float3(0, -f1_3 * s, s8_9 * s);

                float3 res = MathX.Cross(v2 - v0, v1 - v0).Normalized;
                meshX.SetNormal(i, res);
                meshX.SetNormal(i + 1, res);
                meshX.SetNormal(i + 2, res);
                meshX.SetVertex(i++, v0);
                meshX.SetVertex(i++, v2);               
                meshX.SetVertex(i++, v1);

                res = MathX.Cross(v1 - v0, v3 - v0).Normalized;
                meshX.SetNormal(i, res);
                meshX.SetNormal(i + 1, res);
                meshX.SetNormal(i + 2, res);
                meshX.SetVertex(i++, v0);
                meshX.SetVertex(i++, v1);
                meshX.SetVertex(i++, v3);

                res = MathX.Cross(v3 - v0, v2 - v0).Normalized;
                meshX.SetNormal(i, res);
                meshX.SetNormal(i + 1, res);
                meshX.SetNormal(i + 2, res);
                meshX.SetVertex(i++, v0);
                meshX.SetVertex(i++, v3);
                meshX.SetVertex(i++, v2);

                res = float3.Down;
                meshX.SetNormal(i, res);
                meshX.SetNormal(i + 1, res);
                meshX.SetNormal(i + 2, res);
                meshX.SetVertex(i++, v1);
                meshX.SetVertex(i++, v2);
                meshX.SetVertex(i++, v3);
            }

            int[] triangles = new int[meshX.VertexCapacity];
            for (int n = 0; n < triangles.Length; n++)
                meshX.AddTriangle(n);
            
            return meshX;
        }
    }
}
