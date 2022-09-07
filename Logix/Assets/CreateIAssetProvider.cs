//MIT License

//Copyright (c) 2022 Beaned

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;

namespace FrooxEngine.LogiX.ProgramFlow
{
    [Category(new string[] { "LogiX/Assets" })]
    [GenericTypes(new Type[]
{
    typeof(Texture2D),
    typeof(Texture3D),
    typeof(VideoTexture),
    typeof(AudioClip),
    typeof(ITexture2D),
    typeof(Mesh),
    typeof(LocaleResource)
})]

    public class CreateAssetLoader<A> : LogixNode where A : class, IAsset

    {
        //Inputs
        public readonly Input<IAssetProvider<A>> Provider;
        public readonly Input<Slot> slot;

        //Outputs
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;

        [ImpulseTarget]

        public void Run()
        //runs when it gets an impulse
        {
            Slot target = slot.Evaluate();
            var Asset = Provider.EvaluateRaw();

            if (target != null & Asset != null)
            {
                //Attatches a new AssetLoader with the given Audioclip
                target.AttachComponent<AssetLoader<A>>().Asset.Value = Asset.ReferenceID;

                OnDone.Trigger();
            }
            else
            {
                //Trigger when no slot was found
                OnFail.Trigger();
            }
        }
    }
}
