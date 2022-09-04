using BaseX;
using FrooxEngine.LogiX;
using System.Linq;

/// should crawl over the mesh to find all connected triangles
/// maybe should return it as a mesh
/// 

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class SubmeshLooseParts : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<int> Submesh;
        public readonly Input<bool> RemoveOld;

        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                var sub = Submesh.Evaluate();
                if (mesh?.Mesh == null && sub > mesh.Mesh.SubmeshCount)
                {
                    Failed.Trigger();
                    return;
                }
                var m = mesh.Mesh.GetSubmesh(sub);
                if (!(m is TriangleSubmesh tm))
                {
                    Failed.Trigger();
                    return;
                }
                var trySubAsoos = new int[tm.IndicieCount / 3];
                var current = 0;
                for (var i = 0; i < tm.IndicieCount / 3; i++)
                {
                    GetNeighbor(ref trySubAsoos, tm, i, ref current, true);
                }
                for (var i = 0; i < current; i++)
                {
                    var newSubMesh = (TriangleSubmesh)mesh.Mesh.AddSubmesh(SubmeshTopology.Triangles);
                    for (var t = 0; t < tm.IndicieCount / 3; t++)
                    {
                        var target = trySubAsoos[t];
                        if ((target - 1) != i) continue;
                        var e = tm.GetTriangle(t);
                        newSubMesh.AddTriangle(e.Vertex0Index, e.Vertex1Index, e.Vertex2Index);
                    }
                }
                if (RemoveOld.Evaluate(true)) m.Mesh.RemoveSubmesh(sub);
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
        private static void GetNeighbor(ref int[] trySubAccess, TriangleSubmesh m, int index, ref int currentIndex, bool isNSub = false)
        {
            if (trySubAccess[index] != 0)
            {
                return;
            }
            if (isNSub)
            {
                currentIndex++;
            }
            trySubAccess[index] = currentIndex;
            var e = m.GetTriangle(index);
            foreach (var t in m.Mesh.Triangles.Where(x =>
                            x.Submesh == m
                            &&
                            (
                                  x.Vertex0Index == e.Vertex0Index ||
                                  x.Vertex0Index == e.Vertex1Index ||
                                  x.Vertex0Index == e.Vertex2Index ||
                                  x.Vertex1Index == e.Vertex0Index ||
                                  x.Vertex1Index == e.Vertex1Index ||
                                  x.Vertex1Index == e.Vertex2Index ||
                                  x.Vertex2Index == e.Vertex0Index ||
                                  x.Vertex2Index == e.Vertex1Index ||
                                  x.Vertex2Index == e.Vertex2Index
                            ))) GetNeighbor(ref trySubAccess, m, t.Index, ref currentIndex);
        }
    }
}