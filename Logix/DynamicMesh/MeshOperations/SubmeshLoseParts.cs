using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

using System.Collections.Generic;
/// should crawl over the mesh to find all connected triangles
/// maybe should return it as a mesh
/// 

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class SubmeshLoseParts : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<int> Submesh;
        public readonly Input<int> Vertex;
        public readonly Impulse OK;
        public readonly Impulse Failed;

        public readonly Output<MeshX> fuckidfk;

        private  bool[] visited;
        private List<Triangle>[] nodes;
        [ImpulseTarget]
        public void Process()
        {
            var mesh = DynamicMesh.Evaluate();
            var sub = Submesh.Evaluate();
            var vert = Vertex.Evaluate();

            if (mesh?.Mesh == null && sub > mesh.Mesh.SubmeshCount)
            {
                Failed.Trigger();
                return;

            }
            var m = mesh.Mesh.GetSubmesh(sub).Mesh;

            //this thing is cursed and very crap 

            nodes = new List<Triangle>[m.VertexCount];
            visited = new bool[m.VertexCount];
            
            for (int i = 0; i < m.TotalTriangleCount; i++)
            {
                var tri = m.GetTriangle(i);
                nodes[tri.Vertex0.Index].Add(tri);
                nodes[tri.Vertex1.Index].Add(tri);
                nodes[tri.Vertex2.Index].Add(tri);
            }
            
            getneighbor(vert);

            mesh.Mesh.AddSubmesh(SubmeshTopology.Triangles);
            var a = mesh.Mesh.GetSubmesh(mesh.Mesh.SubmeshCount).Mesh;
            for (int i = 0; i < m.TotalTriangleCount; i++)
            {
                if (visited[i])
                {
                    var t = m.GetTriangle(i);
                    a.AddTriangle(t.Vertex0, t.Vertex1, t.Vertex2);
                }
            }
            fuckidfk.Value = a;
            //a.AddTriangle();
            //shitty way to clear the data
            nodes = new List<Triangle>[0];
            visited = new bool[0];
            OK.Trigger();
        }
        private void getneighbor(int index)
        {
            if (!visited[index])
            {
                visited[index] = true;
                nodes[index].ForEach(node =>
                {
                    getneighbor(node.Vertex0Index);
                    getneighbor(node.Vertex1Index);
                    getneighbor(node.Vertex2Index);
                });
            }
        }
    }
}