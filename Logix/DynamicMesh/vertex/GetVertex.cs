using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;
// Rad was here
namespace FrooxEngine.Logix.Math
{
    [Category(new string[] { "LogiX/Mesh/Vertex" })]
    public class GetVertex : LogixOperator<Vertex>
    {
        public readonly Input<IAssetProvider<Mesh>> Mesh;
        public readonly Input<int> Index;

        public override Vertex Content
        {
            get
            {
                var mesh = Mesh.Evaluate();
                if(mesh?.Asset == null)
                {
                    return new Vertex();
                }

                return mesh.Asset.Data.GetVertex(Index.Evaluate());
            }
        }

    }
}