using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine.LogiX.Operators
{
    [Category(new string[] { "LogiX/Mesh" })]
    public class UnpackTriangle : LogixNode
    {
        public readonly Input<Triangle> Triangle;
        public readonly Output<float3> Point0;
        public readonly Output<float2> UV0;
        public readonly Output<float3> Point1;
        public readonly Output<float2> UV1;
        public readonly Output<float3> Point2;
        public readonly Output<float2> UV2;

        protected override void OnEvaluate()
        {
            var tri = Triangle.EvaluateRaw();
            Point0.Value = tri.Vertex0.Position;
            Point1.Value = tri.Vertex1.Position;
            Point2.Value = tri.Vertex2.Position;

            UV0.Value = tri.InterpolateUV0(new float3(1, 0, 0));
            UV1.Value = tri.InterpolateUV0(new float3(0, 1, 0));
            UV2.Value = tri.InterpolateUV0(new float3(0, 0, 1));
        }

        protected override void NotifyOutputsOfChange()
        {
            ((IOutputElement)Point0).NotifyChange();
            ((IOutputElement)UV0).NotifyChange();
            ((IOutputElement)Point1).NotifyChange();
            ((IOutputElement)UV1).NotifyChange();
            ((IOutputElement)Point2).NotifyChange();
            ((IOutputElement)UV2).NotifyChange();
        }
    }
}