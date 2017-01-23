RadioSpy
--------

You play the part of an intelligence agent working to disrupt a plot to assinate *The Leader*. You must listen to different conversations to try and unravel the mystery. There will be clues and redherrings that you must decipher to arrest the correct person(s) involved. 

Setup
-----

To run the game from source, you first must install:

Unity - https://unity3d.com
Twine (v2) - https://twinery.org/
eSpeak - http://espeak.sourceforge.net/

Then the following steps must be taken:

1. Run `python copy_twine.py` from the root directory
2. Run `python Scripts/processVoices.py` 
3. Open Unity and click Assets > Generate > Generate Conversations

Modifying the game
------------------

The game currently only supports one day and choosing who to arrest at the end.

The game is driven by a bunch of Twines files that are automatically converted using a tool called eSpeak into audio files. 

The game generates conversations based on the script in the Twine file. It generates filler audio to keep the 
conversations in sync. 

Weirdly Twine won't let you save your stories anywhere else than in the Documents/Twine/Stories. To modify the ones inside this repository, we recommend making a hard link between the two places. To do this in Windows, you would run the following command from within the TwineSrc/ folder. 

```
mklink /H C:\Path\To\Documents\Twine\Stories\storyname.html storyname.html
```

Note this will only work if this repository is cloned onto the same disk as your documents folder. If this isn't the case, I still haven't found a good fix for this. 

Credits
-------

Iulian Arcus - Programming, Audio
Sophie Collard - Writing
Thomas Kiley - Programming and writing
Richard Lai - Writing


Contributions
-------------

Thanks to NAL for the pixel font we used:
The font file in this archive was created using Fontstruct the free, online
font-building tool.
This font was created by “NAL”.
This font has a homepage where this archive and other versions may be found:
http://fontstruct.com/fontstructions/show/619715


