using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrooxEngine;
using FrooxEngine.Logix.Collections.Delegates;
namespace FrooxEngine.Logix.Collections.Objs
{
    public interface ArrayX<T>: IList<T>, ICustomInspector
    {
        event ArrayXDataChange<T> DataWritten;

        event ArrayXLengthChange<T> DataShortened;

        event ArrayXDataChange<T> DataInsert;

        void AppendArray(ArrayX<T> source);

        void Copy(ArrayX<T> source);
        string ToString();
        ICollectionsObj<T> GetObj(int index);
        
        void Remove(int index, int count);

        int XAdd(T data);

    }
}
