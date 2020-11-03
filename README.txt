TennisFightingGame
==================

A free fighting game with none of the fighting and all of the tennis.

Note: Information on license, contributing and building are found at the 
      bottom.


Running the game
----------------

Currently releases are self-contained, so no dependancies are needed
(this might change in the future).

* Windows users can run the game simply by executing
  TennisFightingGame.exe

* Linux users have to make TennisFightingGame executable and then run
  it:

    $ chmod +x TennisFightingGame && ./TennisFightingGame

Note: Upon starting the game, you might find the default bidings
      confusing, so be sure to read both "Controls" and "How to change
      controls" sections.
      The default keybindings accomodate two players in a single
      keyboard by binding WASD, R, T, Y, U, Return for Player 1 and
      Arrow keys, N, M, Comma, Period, Backspace for Player 2.


Further information on troubleshooting, input devices and the game itself can be
found in MANUAL.pdf, sitting on the same directory this file is.


License
-------

TennisFightingGame is licensed under the GPL-3.0. For more information see 
LICENSE.txt.


Contributing
------------

Currently not accepting contributions.

Excepto de mis bros, que los dirijo al HACKING.txt.


Building from source
--------------------

Run the following command on Game/:

$ dotnet publish -c Release -r win-x64 /p:PublishReadyToRun=false /p:TieredCompilation=false --self-contained

Note that content files are currently not tracked because, since many 
placeholders are not original creations, they amount to a mess of licenses 
not worth the hassle of going through.


Support
-------

Shoot me an email at ramirobasile1@gmail.com.
