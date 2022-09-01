using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine.Logix.Math
{
    [Category(new string[] { "LogiX/Mesh" })]
    public class GetVertex : LogixOperator<float3>
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
                    return new float3();
                }

                return mesh.Asset.Data.GetVertex(Index.Evaluate()).Position;
            }
        }
    }
}