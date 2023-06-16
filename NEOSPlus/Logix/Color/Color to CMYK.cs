using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Color;

[Category(new string[] { "LogiX/Color" })]
[NodeName("Color To CMYK")]
public class ColorToCMYK : LogixNode
{
    public readonly Input<color> Color;
    public readonly Output<float> C;
    public readonly Output<float> M;
    public readonly Output<float> Y;
    public readonly Output<float> K;

    protected override void OnEvaluate()
    {
        color c = Color.Evaluate();

        float rPrime = c.r;
        float gPrime = c.g;
        float bPrime = c.b;

        float k = 1 - Math.Max(rPrime, Math.Max(gPrime, bPrime));

        float cPrime = (1 - rPrime - k) / (1 - k);
        float mPrime = (1 - gPrime - k) / (1 - k);
        float yPrime = (1 - bPrime - k) / (1 - k);

        C.Value = (float)Math.Round(cPrime * 100f);
        M.Value = (float)Math.Round(mPrime * 100f);
        Y.Value = (float)Math.Round(yPrime * 100f);
        K.Value = (float)Math.Round(k * 100f);
    }
}
