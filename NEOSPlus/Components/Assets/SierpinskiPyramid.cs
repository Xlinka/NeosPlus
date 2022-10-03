using BaseX;
using System.Collections.Generic;

namespace FrooxEngine
{
    // https://answers.unity.com/questions/1477363/infinite-vector-3-generate-method-for-sierpinski-t.html
    public class SierpinskiPyramid : MeshXShape
    {
        public int Subdivisions;

        public SierpinskiPyramid(MeshX mesh, int subdivisions) : base(mesh)
        {
            Subdivisions = subdivisions;
            var sTet = new STetrahedron().SubdivideFirst(subdivisions);
            mesh.Clear();
            mesh.SetVertexCount(sTet.centers.Count * 12);

            float s = sTet.Size;
            int i = 0;
            foreach (var c in sTet.centers)
            {
                var v0 = c + new float3(0, s, 0);
                var v1 = c + new float3(-STetrahedron.s2_3 * s, -STetrahedron.f1_3 * s, -STetrahedron.s2_9 * s);
                var v2 = c + new float3(STetrahedron.s2_3 * s, -STetrahedron.f1_3 * s, -STetrahedron.s2_9 * s);
                var v3 = c + new float3(0, -STetrahedron.f1_3 * s, STetrahedron.s8_9 * s);

                var n = MathX.Cross(v2 - v0, v1 - v0).Normalized;
                mesh.SetNormal(i, n);
                mesh.SetNormal(i + 1, n);
                mesh.SetNormal(i + 2, n);
                mesh.AddTriangle(i, i + 1, i + 2);
                mesh.SetVertex(i++, v0);
                mesh.SetVertex(i++, v2);
                mesh.SetVertex(i++, v1);

                n = MathX.Cross(v1 - v0, v3 - v0).Normalized;
                mesh.SetNormal(i, n);
                mesh.SetNormal(i + 1, n);
                mesh.SetNormal(i + 2, n);
                mesh.AddTriangle(i, i + 1, i + 2);
                mesh.SetVertex(i++, v0);
                mesh.SetVertex(i++, v1);
                mesh.SetVertex(i++, v3);

                n = MathX.Cross(v3 - v0, v2 - v0).Normalized;
                mesh.SetNormal(i, n);
                mesh.SetNormal(i + 1, n);
                mesh.SetNormal(i + 2, n);
                mesh.AddTriangle(i, i + 1, i + 2);
                mesh.SetVertex(i++, v0);
                mesh.SetVertex(i++, v3);
                mesh.SetVertex(i++, v2);

                n = float3.Down;
                mesh.SetNormal(i, n);
                mesh.SetNormal(i + 1, n);
                mesh.SetNormal(i + 2, n);
                mesh.AddTriangle(i, i + 1, i + 2);
                mesh.SetVertex(i++, v1);
                mesh.SetVertex(i++, v2);
                mesh.SetVertex(i++, v3);
            }

            // The following have no effects
            // mesh.RecalculateNormals();
            // mesh.RecalculateTangents();
            // mesh.GetMergedDoubles();
        }

        public override void Update()
        {
            Mesh.RecalculateNormals(AllTriangles);
            Mesh.RecalculateTangents(AllTriangles);
        }

        public class STetrahedron
        {
            public static float s8_9 = MathX.Sqrt(8f / 9f);
            public static float s2_9 = MathX.Sqrt(2f / 9f);
            public static float s2_3 = MathX.Sqrt(2f / 3f);
            public const float f1_3 = 1f / 3f;
            public float Size = 1;
            public List<float3> centers = new List<float3>();

            private STetrahedron Subdivide()
            {
                var result = new STetrahedron();
                float s = result.Size = Size * 0.5f;
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

            public STetrahedron SubdivideFirst(int aCount)
            {
                var res = this;
                Size = aCount;
                for (int i = 0; i < aCount; i++)
                    res = res.Subdivide();
                return res;
            }
        }
    }
}