using BaseX;
using System;
using System.Collections.Generic;

namespace FrooxEngine
{
    public class MengerSponge : MeshXShape
    {
        public int Subdivisions;

        private static readonly int[] cubeTriangles = new int[]
        {
            0, 2, 1, 0, 3, 2, 2, 3, 6, 6, 3, 7, 0, 7, 3, 0, 4, 7,
            6, 5, 2, 5, 1, 2, 1, 5, 0, 5, 4, 0, 4, 5, 7, 5, 6, 7,
        };

        public MengerSponge(MeshX mesh, int subdivisions) : base(mesh)
        {
            Subdivisions = subdivisions;
            mesh.Clear();
            GenerateSponge(mesh, Subdivisions, float3.Zero, 1);
        }

        private void GenerateSponge(MeshX mesh, int level, float3 center, float size)
        {
            if (level == 0)
            {
                GenerateCube(mesh, center, size);
            }
            else
            {
                float newSize = size / 3;
                int newLevel = level - 1;

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int z = -1; z <= 1; z++)
                        {
                            if (x == 0 && y == 0 || x == 0 && z == 0 || y == 0 && z == 0)
                                continue;

                            float3 newCenter = center + new float3(x * newSize, y * newSize, z * newSize);
                            GenerateSponge(mesh, newLevel, newCenter, newSize);
                        }
                    }
                }
            }
        }

        private void GenerateCube(MeshX mesh, float3 center, float size)
        {
            float halfSize = size / 2;
            var cubeVertices = new float3[]
            {
                center + new float3(-halfSize, -halfSize, -halfSize),
                center + new float3(halfSize, -halfSize, -halfSize),
                center + new float3(halfSize, -halfSize, halfSize),
                center + new float3(-halfSize, -halfSize, halfSize),
                center + new float3(-halfSize, halfSize, -halfSize),
                center + new float3(halfSize, halfSize, -halfSize),
                center + new float3(halfSize, halfSize, halfSize),
                center + new float3(-halfSize, halfSize, halfSize),
            };

            for (int i = 0; i < cubeTriangles.Length; i += 3)
            {
                int vertexCount = mesh.VertexCount;
                mesh.SetVertexCount(vertexCount + 3);

                mesh.SetVertex(vertexCount, cubeVertices[cubeTriangles[i]]);
                mesh.SetVertex(vertexCount + 1, cubeVertices[cubeTriangles[i + 1]]);
                mesh.SetVertex(vertexCount + 2, cubeVertices[cubeTriangles[i + 2]]);

                float3 normal = MathX.Cross(
                    cubeVertices[cubeTriangles[i + 1]] - cubeVertices[cubeTriangles[i]],
                    cubeVertices[cubeTriangles[i + 2]] - cubeVertices[cubeTriangles[i]]).Normalized;
                mesh.SetNormal(vertexCount, normal);
                mesh.SetNormal(vertexCount + 1, normal);
                mesh.SetNormal(vertexCount + 2, normal);

                mesh.AddTriangle(vertexCount, vertexCount + 1, vertexCount + 2);
            }
        }

        public override void Update()
        {
            Mesh.RecalculateNormals(AllTriangles);
            Mesh.RecalculateTangents(AllTriangles);
        }
    }
}