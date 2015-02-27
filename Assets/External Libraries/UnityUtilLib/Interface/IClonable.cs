using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityUtilLib.Interface
{
    public interface IClonable<T>
    {
        T Clone();
    }
}
