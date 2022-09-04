using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class GetSubmesh : LogixOperator<Submesh>
    {
        public readonly Input<IAssetProvider<Mesh>> DynamicMesh;
        public readonly Input<int> Index;

        public override Submesh Content => DynamicMesh.Evaluate()?.Asset?.Data?.GetSubmesh(Index.Evaluate());
    }

    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class SubmeshMerge : LogixNode
    {
        public readonly Input<Submesh> SubmeshFrom;
        public readonly Input<Submesh> SubmeshTo;
        public readonly Impulse OK;

        [ImpulseTarget]
        public void Process()
        {
            var a = SubmeshFrom.Evaluate();
            var b = SubmeshTo.Evaluate();
            b.Append(a);
            OK.Trigger();
        }
        //public override Mesh Content => Submesh.Evaluate()?.Topology//.Asset?.Data?.GetSubmesh(Index.Evaluate());
    }
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class RemoveSubmesh : LogixNode
    {
        public readonly Input<Submesh> SubMesh;

        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var sub = SubMesh.Evaluate();
                if (sub == null)
                {
                    Failed.Trigger();
                    return;
                }
                sub.Mesh.RemoveSubmesh(sub);
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }

    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class AddSubmesh : LogixNode
    {
        public readonly Input<DynamicMesh> DynamicMesh;
        public readonly Input<SubmeshTopology> Topology;

        public readonly Output<Submesh> Submesh;
        public readonly Output<int> SubMeshIndex;

        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var mesh = DynamicMesh.Evaluate();
                var topology = Topology.Evaluate();

                if (mesh?.Mesh == null)
                {
                    Failed.Trigger();
                    return;
                }
                var ret = mesh.Mesh.AddSubmesh(topology);
                Submesh.Value = ret;
                SubMeshIndex.Value = ret.Index;
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}