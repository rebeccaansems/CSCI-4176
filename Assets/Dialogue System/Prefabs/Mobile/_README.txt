/*

*** To use link.xml, move it into the root Assets folder. ***

The link.xml file in this folder allows you to build Dialogue System
projects with Stripping Level set to "Strip assemblies" or "Strip ByteCode".
The Dialogue System uses the System DLLs, so it doesn't support
"Use micro mscorlib" stripping.

If you don't use the link.xml file, Dialogue System functionality that
uses reflection, such as the sequencer, will not work.


If your game doesn't use networking, you can remove the requirement for 
full network permissions in your builds by deleting these two files:

- Scripts/Supplemental/Sequencer Commands/AudioWWW.cs 
- Scripts/Supplemental/Utility/LuaNetworkCommands.cs

*/