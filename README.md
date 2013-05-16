# Project File Synchronizer

Project File Synchronizer is a command line tool intended to aid in the multi-project 
setup common to a MonoGame environment. 

Each platform being targed needs a separate project, however as you are developing it 
becomes cumbersome and tedious to constantly be manually editing project files. Or 
cluttered to throw everything into a single folder.

## Overview

We will use one of the platform project files as the 'base project'. Form it, all 
project and content files will be sync'd to the other projects using links\references. 
This means that all of the source and assets will be located within the base
project's folder.

This project is pretty new. I've found that Windows 8 provides the fastest build
to deploy turn around time and has the nicest remote debugging support. For these
reasons I made my base project a Windows 8 project. For now, this is a requirement.

## Project Structure

A specific directory structure and naming scheme is expected. You will need to create
each platform project yourself using whatever IDE you prefer.

<pre>
|- Base
   |- Base.csproj
   |- file.cs
   |- SomeDirectory
      |- subfoldersarefine.cs
   |- Content
       |- asset.png
       |- etc
|- Base.Windows
    |- Base.Windows.csproj
|- Base.Android
    |- Base.Android.csproj
|- Base.WindowsPhone
    |- Base.WindowsPhone.csproj
|- Base.iOS
	|- Base.iOS.csproj
</pre>

## Adding a file

To add a file, always add it to the Base project. Then execute this sync tool

## Removing a file

Just delete it from the Base project. Right now the sync tool will not remove files. 
This has been less of an issue as they show up as missing files and it's extremely
easy to just delete the reference in the IDE.

## References

This sync tool will not sync references