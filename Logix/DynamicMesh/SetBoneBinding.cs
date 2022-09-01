using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;

namespace FrooxEngine
{
    [Category(new string[] { "LogiX/Mesh/" })]
    public class SetBoneBinding : LogixNode
    {
        public readonly Input<BoneBinding> BoneBind;

        public readonly Input<int> boneIndex0;
        public readonly Input<int> boneIndex1;
        public readonly Input<int> boneIndex2;
        public readonly Input<int> boneIndex3;
        public readonly Input<float> weight0;
        public readonly Input<float> weight1;
        public readonly Input<float> weight2;
        public readonly Input<float> weight3;


        public readonly Impulse OK;
        public readonly Impulse Failed;

        [ImpulseTarget]
        public void Process()
        {
            try
            {
                var bone = BoneBind.Evaluate();
                if(boneIndex0.IsConnected){
                    bone.boneIndex0 = boneIndex0.Evaluate();
                }
                if(boneIndex1.IsConnected){
                    bone.boneIndex1 = boneIndex1.Evaluate();
                }
                if(boneIndex2.IsConnected){
                    bone.boneIndex2 = boneIndex2.Evaluate();
                }
                if(boneIndex3.IsConnected){
                    bone.boneIndex3 = boneIndex3.Evaluate();
                }
                if(weight0.IsConnected){
                    bone.weight0 = weight0.Evaluate();
                }
                if(weight1.IsConnected){
                    bone.weight1 = weight1.Evaluate();
                }
                if(weight2.IsConnected){
                    bone.weight2 = weight2.Evaluate();
                }
                if (weight3.IsConnected) { 
                    bone.weight3 = weight3.Evaluate();
                }
                OK.Trigger();
            }
            catch
            {
                Failed.Trigger();
            }
        }
    }
}