Note: more information can be found in the help file: ```Documentation\Documentation.chm```.

## Introduction
This program called 'ChameleonCoder' (CC) is intended to be an IDE for every programming or scripting language.
It makes it possible to interoperate between different languages easily, providing a lot of other features useful for programmers.
Current version: 0.0.0.1 alpha 2

## System Requirements
To run CC, the .NET Framework v4 is required. The Framework v4 itself is supported begining with XP SP3.
Other implementations such as MONO do ***not*** work as CC requires WPF.

## Credits & Related
* CC is licensed under GPL which can be found in license.txt.
* CC makes use of the great ***AvalonEdit*** control. It is licensed under GPL, too. [more...](http://www.icsharpcode.net/OpenSource/SD/)
* CC also uses the ***Odyssey WPF Controls*** by Thomas Gerber. They're licensed under the Microsoft Public License. [more...](http://odyssey.codeplex.com/)

## Features
### realized
* rich extensibility model
* parsing of resource files and displaying them, including hierarchy
* 6 included resource types
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
* referencing a resource in another place

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
* creation of empty files, management of existing files
	* creation and modification of references

## Extensibility model
It is planned to support different models to extend CC.
There are 4 types of plugins:

* the most important one: ***LanguageModules***, which add language-specific support such as a compiler, intellisense, ...
* ***services*** which can be opened by the user and perform a specific, language-independant action.
* ***templates*** which create a new resource and apply a common template on it, for example by setting its properties or adding metadata.
* ***component factories*** which are used to provide custom resource types and custom RichContent members.

To add an extension to CC, simply place it in the ```Components\``` folder. Then run CC, go to the plugins dialog and click "install".
Select the file containing the plugin. Select the plugins you want to install and click "install selected". Then restart CC.

## Command line
CC supports several command line switches:

***NOTE:*** All of these must be the first parameter you pass.

* when you pass the path to a *.ccr file to CC, it will be opened
* ```--install_ext``` installs the *.ccr file extension
* ```--uninstall_ext``` unistalls the *.ccr file extension
* ```--install_com``` will install COM support in a future version
* ```--uninstall_com``` will uninstall COM support in a future version
* ```--install_full``` will install both the file extension and COM support in a future version (now it only affects the file extension)
* ```--uninstall_full``` will uninstall both the file extension and COM support in a future version (now it only affects the file extension)
* when you pass ```--config``` and as next parameter a file, CC will use this file as configuration file instead of writing to your AppData.
This option can also be passed ***after*** the others.

## Resources model
Resources are described by XML files. A resource can have several child resource. For more information see the help file.
You can interact with these resources in a lot of different ways such as copying, moving, editing or compiling them.

## RichContent model
Each resource can have 'RichContent'. RichContent describes what parts a resource includes.
These members can have child members: An example would be the classes in a namespace, the methods in a class, the parameters in a method, ...