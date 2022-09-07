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
    [Category("LogiX/Assets")]
    [NodeName("GetIAssetProvider")]

    //Make a group for the generic types of assets
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
    public class GetIAssetProvider<A> : LogixNode where A : class, IAsset
    {
        //Inputs
        public readonly Input<Slot> slot;

        //Outputs
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        public readonly Output<IAssetProvider<A>> AssetProviderOut;

        [ImpulseTarget]
        public void Run()
        {
            Slot target = slot.Evaluate();

            if (target != null)
            {

                //Get the first  available IAssetProvider component on the slot
                var AssetComponent = target.GetComponent<IAssetProvider<A>>();
                AssetProviderOut.Value = AssetComponent;

                //Check if it found an IAssetprovider and trigger OnDone if yes otherwise trigger an OnFail
                if (AssetComponent != null) OnDone.Trigger();
                else OnFail.Trigger();
            }
            else

            {
                OnFail.Trigger(); //Trigger an impulse if no slot was found failing
            }
            AssetProviderOut.Value = null; //Set the output value to null again
        }
    }
}
