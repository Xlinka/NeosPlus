using BaseX;
using System;

namespace FrooxEngine
{
    public class MobiusStrip : MeshXShape
    {
        public int Sides;
        public int Resolution;
        public float Width;

        public MobiusStrip(MeshX mesh, int sides, int resolution, float width) : base(mesh)
        {
            Sides = sides;
            Resolution = resolution;
            Width = width;

            mesh.Clear();
            mesh.SetVertexCount(sides * resolution * 4);

            int index = 0;
            for (int side = 0; side < sides; side++)
            {
                for (int res = 0; res < resolution; res++)
                {
                    float t0 = (float)side / sides;
                    float t1 = (float)(side + 1) / sides;
                    float s0 = (float)res / resolution;
                    float s1 = (float)(res + 1) / resolution;

                    var v0 = GetMobiusPoint(t0, s0, width);
                    var v1 = GetMobiusPoint(t1, s0, width);
                    var v2 = GetMobiusPoint(t1, s1, width);
                    var v3 = GetMobiusPoint(t0, s1, width);

                    mesh.SetVertex(index, v0);
                    mesh.SetVertex(index + 1, v1);
                    mesh.SetVertex(index + 2, v2);
                    mesh.SetVertex(index + 3, v3);

                    mesh.AddQuadAsTriangles(index, index + 1, index + 2, index + 3);
                    index += 4;
                }
            }
        }

        public override void Update()
        {
        }

        private float3 GetMobiusPoint(float t, float s, float width)
        {
            float angle = 2 * MathX.PI * t;
            float radius = 1.0f + s * width - (width / 2);
            float3 point = new float3(MathX.Cos(angle) * radius, MathX.Sin(angle) * s * (width / 2), MathX.Sin(angle) * radius);

            return point;
        }
    }
}