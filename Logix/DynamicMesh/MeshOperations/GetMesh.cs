using System;
using BaseX;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.LogiX.Audio;
using FrooxEngine.UIX;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
/// credit
/// faloan
/// 
namespace FrooxEngine.LogiX.Assets
{
    [Category(new string[] { "LogiX/Mesh/Operations" })]
    public class GetMesh : LogixOperator<Mesh>
    {
        public readonly Input<Slot> Root;

        public override Mesh Content
        {
            get
            {
                return Root.Evaluate().GetComponent<Mesh>();
            }
        }

    }
}
