using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    public interface IResolvable
    {
        ResourceBase Resolve();
    }
}
