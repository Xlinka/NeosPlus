using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine.LogiX.Operators
{
    [Category(new string[] { "LogiX/Mesh/Vertex" })]
    public class UnpackVertex : LogixNode
    {
        public readonly Input<Vertex> Vertex;

        public readonly Output<float3> Position;
        public readonly Output<float3> Normal;
        public readonly Output<float3> Tangent;
        public readonly Output<float4> Tangent4;
        public readonly Output<color> Color;
        public readonly Output<BoneBinding> BoneBinding;
        public readonly Output<float2> UV0;
        public readonly Output<float2> UV1;
        public readonly Output<float2> UV2;
        public readonly Output<float2> UV3;
        public readonly Output<int> Index;

        protected override void OnEvaluate()
        {
            var vert = Vertex.Evaluate();
            var mesh = vert.Mesh;
            Position.Value = vert.Position;
            Normal.Value = (mesh.HasNormals) ? vert.Normal : default;
            Tangent.Value = (mesh.HasTangents) ? vert.Tangent : default;
            Color.Value = (mesh.HasColors) ? vert.Color : default;
            BoneBinding.Value = (mesh.HasBoneBindings) ? vert.BoneBinding : default;
            Tangent4.Value = (mesh.HasTangents) ? vert.Tangent4 : default;
            UV0.Value = (mesh.HasUV0s) ? vert.UV0 : default;
            UV1.Value = (mesh.HasUV1s) ? vert.UV1 : default;
            UV2.Value = (mesh.HasUV2s) ? vert.UV2 : default;
            UV3.Value = (mesh.HasUV3s) ? vert.UV3 : default;
            Index.Value = vert.Index;
        }

        protected override void NotifyOutputsOfChange()
        {
            ((IOutputElement)Position).NotifyChange();
            ((IOutputElement)Normal).NotifyChange();
            ((IOutputElement)Tangent).NotifyChange();
            ((IOutputElement)Tangent4).NotifyChange();
            ((IOutputElement)Color).NotifyChange();
            ((IOutputElement)BoneBinding).NotifyChange();
            ((IOutputElement)UV0).NotifyChange();
            ((IOutputElement)UV1).NotifyChange();
            ((IOutputElement)UV2).NotifyChange();
            ((IOutputElement)UV3).NotifyChange();
        }
    }
}