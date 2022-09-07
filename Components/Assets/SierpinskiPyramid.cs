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
        private readonly STetrahedron sTetrahedron = new STetrahedron();
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
    }
}
