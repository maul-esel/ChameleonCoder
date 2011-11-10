namespace ChameleonCoder.Plugins.Syntax
{
    /// <summary>
    /// an enumeration to describe elements in a coding language's syntax
    /// </summary>
    public enum SyntaxElement
    {
        None,

        Array,

        Function,

        LineEnd,

        Method,

        Constant,        

        ScriptingObject,

        ExtendedSignature,


        Assignment,

        AltAssignment,

        ParamDefaultValueAssignment,


        StringDelimiter,

        AltStringDelimiter,


        VariableDelimiterBegin,

        VariableDelimiterEnd,


        Comment,

        MultiLineComment,

        MultiLineCommentBegin,

        MultiLineCommentEnd,


        Class,

        ClassAccessor,

        ClassField,

        ClassInheritance,

        MultipleClassInheritance,

        ClassMethod,

        ClassProperty,


        Struct,

        StructAccessor,

        StructInheritance,

        MultipleStructInheritance,

        StructMethod,


        Interface,

        InterfaceAccessor,

        InterfaceInheritance,

        MultipleInterfaceInheritance,

        InterfaceImplementation,

        MultipleInterfaceImplementation,

        InterfaceMethod,


        PropertyAccessor,

        FieldAccessor,


        Constructor,

        Destructor,


        Operator,

        CustomOperator
    }
}
