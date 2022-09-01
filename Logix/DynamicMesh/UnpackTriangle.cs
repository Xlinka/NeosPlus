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
        public readonly Output<float3> Point0Pos;
        public readonly Output<float2> Point0UV;
        public readonly Output<color> Point0Col;
        public readonly Output<float3> Point0Norm;
        public readonly Output<float4> Point0Tan;

        public readonly Output<float3> Point1Pos;
        public readonly Output<float2> Point1UV;
        public readonly Output<color> Point1Col;
        public readonly Output<float3> Point1Norm;
        public readonly Output<float4> Point1Tan;

        public readonly Output<float3> Point2Pos;
        public readonly Output<float2> Point2UV;
        public readonly Output<color> Point2Col;
        public readonly Output<float3> Point2Norm;
        public readonly Output<float4> Point2Tan;

        protected override void OnEvaluate()
        {
            var tri = Triangle.Evaluate();
            Point0Pos.Value = tri.Vertex0.Position;
            Point1Pos.Value = tri.Vertex1.Position;
            Point2Pos.Value = tri.Vertex2.Position;

            Point0UV.Value = tri.Vertex0.UV0;//tri.InterpolateUV0(new float3(1, 0, 0));
            Point1UV.Value = tri.Vertex1.UV0;//tri.InterpolateUV0(new float3(0, 1, 0));
            Point2UV.Value = tri.Vertex2.UV0;//tri.InterpolateUV0(new float3(0, 0, 1));

            Point0Col.Value = tri.Vertex0.Color;
            Point1Col.Value = tri.Vertex1.Color;
            Point2Col.Value = tri.Vertex2.Color;

            Point0Norm.Value = tri.Vertex0.Normal;
            Point1Norm.Value = tri.Vertex1.Normal;
            Point2Norm.Value = tri.Vertex2.Normal;

            Point0Tan.Value = tri.Vertex0.Tangent;
            Point1Tan.Value = tri.Vertex1.Tangent;
            Point2Tan.Value = tri.Vertex2.Tangent;

        }

        protected override void NotifyOutputsOfChange()
        {
            ((IOutputElement)Point0Pos).NotifyChange();
            ((IOutputElement)Point0UV).NotifyChange();
            ((IOutputElement)Point0Col).NotifyChange();
            ((IOutputElement)Point0Norm).NotifyChange();
            ((IOutputElement)Point0Tan).NotifyChange();


            ((IOutputElement)Point1Pos).NotifyChange();
            ((IOutputElement)Point1UV).NotifyChange();
            ((IOutputElement)Point1Col).NotifyChange();
            ((IOutputElement)Point1Norm).NotifyChange();
            ((IOutputElement)Point1Tan).NotifyChange();

            ((IOutputElement)Point2Pos).NotifyChange();
            ((IOutputElement)Point2UV).NotifyChange();
            ((IOutputElement)Point2Col).NotifyChange();
            ((IOutputElement)Point2Norm).NotifyChange();
            ((IOutputElement)Point2Tan).NotifyChange();
        }
    }
}