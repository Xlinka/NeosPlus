using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using CloudX;
using CloudX.Shared;
using BaseX;
namespace NEOSPlus.Shaders
{
    internal class ShaderInjection
    {
        async Task RegisterShader(Uri uri)
        {
            // This is the important bit of the code
            string signature = AssetUtil.ExtractSignature(uri);
            // Check if the shader is already in the database
            var shaderExists = await Engine.Current.LocalDB.ReadVariableAsync(signature, false);
            if (!shaderExists)
            {
                // If the shader isn't in the database yet add it
                await Engine.Current.LocalDB.WriteVariableAsync(signature, true);
            }
        }

        private void AppendShader()
        {
            // Put all your shader urls in this array
            List<Uri> shaders = new List<Uri>();
            List<Task> tasks = new List<Task>();

            foreach (var shader in shaders)
            {
                tasks.Add(RegisterShader(shader));
            }
            Task.WaitAll(tasks.ToArray());
        }
         
    }
}
