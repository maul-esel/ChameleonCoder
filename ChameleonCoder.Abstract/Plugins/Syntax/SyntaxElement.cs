﻿namespace ChameleonCoder.Plugins.Syntax
{
    /// <summary>
    /// an enumeration to describe elements in a coding language's syntax
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(true)]
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

        ReturnValue,

        Field,

        Property,

        Parameter,

        Variable,

        ExplicitVariableDeclaration,

        TypedVariableDeclaration,


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
