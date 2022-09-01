using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;

namespace FrooxEngine.LogiX.Operators
{
    [Category(new string[] { "LogiX/Mesh" })]
    public class UnpackBoneBindings : LogixNode
    {
        public readonly Input<BoneBinding> BoneBind;

        public readonly Output<int> boneIndex0;
        public readonly Output<int> boneIndex1;
        public readonly Output<int> boneIndex2;
        public readonly Output<int> boneIndex3;
        public readonly Output<float> weight0;
        public readonly Output<float> weight1;
        public readonly Output<float> weight2;
        public readonly Output<float> weight3;

        protected override void OnEvaluate()
        {
            var bone = BoneBind.Evaluate();
            boneIndex0.Value = bone.boneIndex0;
            boneIndex1.Value = bone.boneIndex1;
            boneIndex2.Value = bone.boneIndex2;
            boneIndex3.Value = bone.boneIndex3;
            weight0.Value = bone.weight0;
            weight1.Value = bone.weight1;
            weight2.Value = bone.weight2;
            weight3.Value = bone.weight3;
        }

        protected override void NotifyOutputsOfChange()
        {
            ((IOutputElement)boneIndex0).NotifyChange();
            ((IOutputElement)boneIndex1).NotifyChange();
            ((IOutputElement)boneIndex2).NotifyChange();
            ((IOutputElement)boneIndex3).NotifyChange();
            ((IOutputElement)weight0).NotifyChange();
            ((IOutputElement)weight1).NotifyChange();
            ((IOutputElement)weight2).NotifyChange();
            ((IOutputElement)weight3).NotifyChange();
        }
    }
}