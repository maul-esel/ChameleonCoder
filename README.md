Note: more information can be found in the help file: ```Documentation\Documentation.chm```.

## Introduction
This program called 'ChameleonCoder' (CC) is intended to be an IDE for every programming or scripting language.
It makes it possible to interoperate between different languages easily, providing a lot of other features useful for programmers.
Current version: 0.0.0.1 alpha (not yet functional)

## System Requirements
To run CC, the .NET Framework v4 is required. The Framework v4 itself is supported begining with XP SP3.
Other implementations such as MONO do not work as CC requires WPF.

## Credits & Related
* CC is licensed under GPL which can be found in license.txt.
* CC makes use of the great ***AvalonEdit*** control. It is licensed under GPL, too. [more...](http://www.icsharpcode.net/OpenSource/SD/)
* CC also uses the ***Windows Ribbon Control for WPF October*** by Microsoft. The license can be found in ribbon_license.txt.


## Features
### realized
* rich extensibility model
* parsing of resource files and displaying them, including hierarchy
* 6 included resource types
	* links, which allow referencing another resource without copying it
	* files
    * code files
    * libraries
    * projects
    * tasks
* user-defined metadata for a resource
* possibility to add custom resource types

### planned
* creation, moving, copying and deleting of resources
* super-easy importing of resources
* packaging and unpackaging of resource into archives, with the possibility to include the corresponding data files
* support for coding languages using LanguageModules, which can allow
	* compilation of resources,
	* execution of resources,
	* syntax highlighting,
	* intellisense,
	* ... and a lot more
* multiple language support
* of course rich editing component using the AvalonEdit control
* RichContent model for describing the content of a resource
* Generating documentation from RichContent, maybe exporting to HTML
* maybe a "breadcrumb" control for easy navigation

## Extensibility model
It is planned to support different models to extend CC.
Basically, there are two types: ***plugins***, which can communicate with CC and influence it in A LOT of different ways, and ***static components***, which can't communicate with CC.

Speaking of ***plugins***, there are 2 types:

* the most important one: ***LanguageModules***, which add language-specific support such as a compiler, intellisense, ...
* ***services*** which can be opened by the user and perform a specific, language-independant action.

When it comes to ***static components***, the following 2 types are planned:

* ***ResourceTypes*** that enable support for custom resource types, such as *IniSection* or *XmlNode*.
* ***RichContentMembers*** which enable support for custom RichContent types. They often correspond to a custom ResourceType.

To add an extension to CC, simply place it in the ```Components\``` folder.

## Resources model
Resources are described by XML files in the ```Data\``` folder. A resource can have several child resource. For more information see the help file.
You can interact with these resources in a lot of different ways such as copying, moving, editing or compiling them.

## RichContent model
Each resource can have 'RichContent'. RichContent describes what parts a resource includes.
An example would be the functions in a class, including its parameters.

RichContent is described using XML and the ```<content>``` key in a resource. Multiple hierarchy levels will be supported,
and as mentioned above, it could be used for auto-generated documentation, including export to HTML, or other purposes.

