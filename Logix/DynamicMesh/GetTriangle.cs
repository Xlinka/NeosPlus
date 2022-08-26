using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine.Logix.Math
{
    [Category(new string[] { "LogiX/Mesh" })]
    public class GetTriangle : LogixOperator<Triangle>
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<int> Index;
        public readonly Input<int> Submesh;

        public override Triangle Content
        {
            get
            {
                var mesh = DynamicMesh.Evaluate();
                var i = Index.Evaluate();
                var s = Submesh.Evaluate();
                return mesh.Mesh.GetTriangle(i, s);
            }
        }
    }
}