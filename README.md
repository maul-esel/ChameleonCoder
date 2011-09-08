Note: more information can be found in the help file: ```Documentation\Documentation.chm```.

## Introduction
This program called 'ChameleonCoder' (CC) is intended to be an IDE for every programming or scripting language.
It makes it possible to interoperate between different languages easily, providing a lot of other features useful for programmers.
Current version: 0.0.0.1 alpha 2

## System Requirements
To run CC, the .NET Framework v4 is required. The Framework v4 itself is supported begining with XP SP3.
Other implementations such as MONO do not work as CC requires WPF.

## Credits & Related
* CC is licensed under GPL which can be found in license.txt.
* CC makes use of the great ***AvalonEdit*** control. It is licensed under GPL, too. [more...](http://www.icsharpcode.net/OpenSource/SD/)
* CC also uses the ***Odyssey WPF Controls*** by Thomas Gerber. They're licensed under the Microsoft Public License. [more...](http://odyssey.codeplex.com/)

## Features
### realized
* rich extensibility model
* parsing of resource files and displaying them, including hierarchy
* 7 included resource types
	* links, which allow referencing another resource without copying it
	* files
    * code files
    * libraries
    * projects
    * tasks
	* groups
* user-defined metadata for a resource
* possibility to add custom resource types
* a breadcrumb control for easy navigation
* multiple language support
* creation, moving, copying and deleting of resources

### planned
* support for coding languages using LanguageModules, which can allow
	* compilation of resources,
	* execution of resources,
	* syntax highlighting,
	* intellisense,
	* ... and a lot more
* of course rich editing component using the AvalonEdit control
* RichContent model for describing the content of a resource
* Generating documentation from RichContent, maybe exporting to HTML
* 2 different file types including possibility to convert between them
* backing up *.ccp files on startup, possibility to extract / restore / delete backups
* creation of empty files of both types

## Extensibility model
It is planned to support different models to extend CC.
There are 4 types of plugins:

* the most important one: ***LanguageModules***, which add language-specific support such as a compiler, intellisense, ...
* ***services*** which can be opened by the user and perform a specific, language-independant action.
* ***templates*** which create a new resource and apply a common template on it, for example by setting its properties or adding metadata.
* ***component factories*** which are used to provide custom resource types and custom RichContent members.

To add an extension to CC, simply place it in the ```Components\``` folder. Then run CC, go to the plugins dialog and click "install".
Select the file containing the plugin. Select the plugins you want to install and click "install selected".

## Resources model
Resources are described by XML files. A resource can have several child resource. For more information see the help file.
You can interact with these resources in a lot of different ways such as copying, moving, editing or compiling them.

## RichContent model
Each resource can have 'RichContent'. RichContent describes what parts a resource includes.
These members can have child members: An example would be the classes in a namespace, the methods in a class, the parameters in a method, ...

## Resource files
There are 2 types of resource files:

* files with the extension *.ccr are simply XML files containing the resource markup. All related files are stored outside.
Editing without CC is possible, and the files are smaller.

* files with the extension *.ccp are zip-like files. They contain the resource markup, but every related file will be stored inside, too.
Editing without CC is only possible using an archiver such as 7zip, and the files are bigger.
But there will be the possibility to backup files every time opened, and you have to carry just 1 file with you.

