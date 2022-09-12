using BaseX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrooxEngine
{
    // https://answers.unity.com/questions/1477363/infinite-vector-3-generate-method-for-sierpinski-t.html
    public class SierpinskiPyramid : MeshXShape
    {
        public int Subdivisions;
        public SierpinskiPyramid(MeshX mesh, int subdivisions) : base(mesh)
        {
            var sMesh = sTetrahedron.CreateMesh(subdivisions);
            mesh.SetVertexCount(sMesh.VertexCapacity);
            for (int i = 0; i < sMesh.VertexCapacity; i++)
            {
                mesh.SetVertex(i, sMesh.GetVertex(i).Position);
                mesh.SetNormal(i, sMesh.GetNormal(i));
                mesh.AddTriangle(sMesh.GetTriangle(i));
            }

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
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
