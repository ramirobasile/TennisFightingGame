TennisFightingGame
==================

A free fighting game with none of the fighting and all of the tennis.


License
-------

TennisFightingGame is free software licensed under the GPL-3.0. For more 
information see LICENSE.txt.


Building from source and running the game
-----------------------------------------

1. Install .NET core 3.1 or above

2. Get the content files[1] (email me)

3. Run the following command on te Game/ folder:

   $ dotnet publish -c Release -r linux-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained
   
   You can replace linux-x64 with linux-x86 to build a 32-bit version or
   with windows-x64 and windows-x86 to build a Windows version.

Troubleshooting and more information about the game can be found in
MANUAL.pdf which sits in the same directory this file is.


Contact
-------

You can shoot me an email at ramirobasile1@gmail.com if you have any
questions, need help with getting the game running, or just want to
chat.


[1]: The content files right now are mostly placeholders not authored by
     me, so distributing them under the correct license is kind of a
     hassle.
