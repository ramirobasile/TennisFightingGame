TennisFightingGame
==================

A free fighting game with none of the fighting and all of the tennis.


License
-------

TennisFightingGame is licensed under the GPL-3.0. For more information see
LICENSE.txt.


Contributing
------------

Currently not accepting merge requsts.

Excepto de mis bros, que los dirijo al HACKING.txt.


Support
-------

* Email me at ramirobasile1@gmail.com


Running the game
----------------

Currently releases are self-contained, so no dependancies are needed (this
might change in the future).

* Windows users can run the game simply by executing TennisFightingGame.exe
* Linux users have to make TennisFightingGame executable and then run it:
  $ chmod +x TennisFightingGame && ./TennisFightingGame

Note: Upon starting the game, you might find default bidings confusing, so
      be sure to read both "Controls" and "How to change controls" sections.
      The default keybindings accomodate two players in a single keyboard by
      binding WASD, R, T, Y, U, Return for Player 1 and Arrow keys,
      N, M, Comma, Period, Backspace for Player 2.

Controls
--------

Users input "actions" to their characters, with each action being bound to
a certain key or button, depending on the input method.

List of actions and their uses:

* Up: Used for moving up and down menues, and, in-match, for motion inputs.
* Down: Used for moving up and down menues, and, in-match, for motion inputs.
* Left: Used to move left. Quickly tapping twice will result in a dash and
        holding it afterwards will result in running for as long as it's held
* Right: Used to move right. Quickly tapping twice will result in a dash and
         holding it afterwards will result in running for as long as it's held
* Jump: Used to perform jump
* Light: Used to throw light attacks
* Medium: Used to throw medium attacks
* Heavy: Used to throw heavy attacks
* Start: Used to end character selecion once all are ready and pause in-match


How to change bindings
----------------------

You'll find a file named TennisFightingGame.ini in the same directory the
executable is, which contains many other settings besides bindings.

The section "[PX InputMethod]", where X is the corresponding player index,
determines which kind of input device a player uses. Currently, 0 is for
keyboard and 1 is for XInput devices. Below, a section titled "[Controls]"
for each player contains a binding for each of the possible actions, such
as Left, Right, Jump, etc.

* Keyboard keys are represented by numbers correspoding to each key in
  C#'s Keys enum. To bind a certain key to a player's action, say,
  the spacebar to Player 1's Jump, find "Jump=" under the section titled
  "[P1 Controls]" and then look up the key code for spacebar in the C#
  reference (link to it lies in a comment within the configuration file),
  32 in this case, and assign it to jump by writing it like this: "Jump=32".

* Controller buttons work the same, except the corresponding number for
  each button are based on the XNA's Buttons enum (link also found in a
  comment within the configuration file).


Using up non-XInput controllers
-------------------------------

x360ce.


