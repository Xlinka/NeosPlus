using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine.Logix.Collections.Objs;
namespace FrooxEngine.Logix.Collections.Delegates
{
    public delegate void ArrayXDataChange<T>(ArrayX<T> sender, int index, int length);

    public delegate void ArrayXLengthChange<T>(ArrayX<T> sender, int index, int length);

}
