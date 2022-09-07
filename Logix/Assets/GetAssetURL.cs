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
    [NodeName("GetAssetURL")]

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
    public class GetAssetURL<A> : LogixNode where A : class, IAsset
    {
        //Inputs
        public readonly Input<IAssetProvider<A>> Provider;

        //Outputs
        public readonly Impulse OnDone;
        public readonly Impulse OnFail;
        public readonly Output<Uri> Url;

        [ImpulseTarget]
        public void Run()
        //Run when it gets an impulse
        {
            var Asset = Provider.Evaluate();

            if (Asset != null)
            {
                IField URL = Asset.TryGetField("URL"); //Try to get the field on the Asset called "URL"
                Uri result;
                Url.Value = Uri.TryCreate(URL.ToString(), UriKind.Absolute, out result) ? result : (Uri)null; //tries to convert the Ifield to a Uri variable, fail will result in null
                OnDone.Trigger();   //incase the code above hasn't thrown multiple exeptions this should fire
            }
            else
            {
                OnFail.Trigger();   //when it tries its best but it don't succeed
            }

            Url.Value = null; //Set value back to null after the action
        }

        protected override void NotifyOutputsOfChange()
        {
            ((IOutputElement)Url).NotifyChange();
        }
    }
}
