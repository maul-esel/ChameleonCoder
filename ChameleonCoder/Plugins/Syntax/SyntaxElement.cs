namespace ChameleonCoder.Plugins.Syntax
{
    /// <summary>
    /// an enumeration to describe elements in a coding language's syntax
    /// </summary>
    public enum SyntaxElement
    {
        LineEnd,

        Assignment,

        AltAssignment,

        ParamDefaultValueAssignment,

        StringDelimiter,

        AltStringDelimiter,

        VariableDelimiterBegin,

        VariableDelimiterEnd,

        Comment,

        MultiLineCommentBegin,

        MultiLineCommentEnd,

        Class,

        Struct,

        Interface,

        Function,

        ClassProperty,

        ClassField,

        PropertyAccessor,

        FieldAccessor,

        ClassAccessor,

        StructAccessor,

        InterfaceAccessor,

        Constructor,

        Destructor,

        ClassInheritance,

        StructInheritance,

        InterfaceInheritance
    }
}
