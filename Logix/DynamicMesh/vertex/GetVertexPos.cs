using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine.Logix.Math
{
    [Category(new string[] { "LogiX/Mesh/Vertex" })]
    public class GetVertexPos : LogixOperator<float3>
    {
        public readonly Input<IAssetProvider<Mesh>> Mesh;
        public readonly Input<int> Index;

        public override float3 Content
        {
            get
            {
                var mesh = Mesh.Evaluate();
                if(mesh?.Asset == null)
                {
                    return float3.Zero;
                }

                return mesh.Asset.Data.GetVertex(Index.Evaluate()).Position;
            }
        }
    }
}