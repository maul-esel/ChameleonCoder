using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.RichContent.Implementations
{
    public class MethodMember : FunctionMember
    {
        public override bool ValidateParent(IContentMember parent)
        {
            if (parent is ClassMember)
                return true;
            return false;
        }
    }
}
