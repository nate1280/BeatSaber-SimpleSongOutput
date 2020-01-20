Simple Song Output v1.0.1 by nate1280
----------------------------------------------------------------------------------
Light weight option for streamers who are having issues with HTTPStatus

Outputs song title and more to a text file (that you can include in your
broadcast scene) to show viewers the song you're playing.

The output string is dynamic, and can be changed via its config file (if
you're familiar with Versus, it uses the same kind of system)

Default output is:

%songname%%songsubname: - % by %levelauthorname% on %difficulty%

Available options:

%time%				- The current time, formatted hh:mm
%longdate%			- The current time, formatted hh:mm:ss
%date%				- The current date, formatted yyyy/MM/dd
%lf%				- A linefeed
%songname%			- The song name
%songsubname%		- Secondary song name, not all have this
%songauthorname%	- The author of the song
%levelauthorname%	- The mapper
%difficulty%		- The difficulty being played
%songbpm%			- Beats per minute of the song
%notejumpspeed%		- NJS of the map
%notescount%		- Number of notes in the map
%bombscount%		- Number of bombs in the map
%obstaclescount%	- Number of obstacles in the map

All fields can also be optionally prefixed by adding text after a colon
as an example, %songsubname: - %, this will prefix songsubname with
a " - " if its not empty.

To change this, simply modify the settings file.

You can also output a song's cover into a thumbnail, this is by default turned
off and can be enabled in the settings.  Also, this thumbnail is auto resized
(by default) to 256x256 pixels, and is adjustable via the settings file
manually (a restart of beat saber is required after changing it)

You can also override the location, and filenames for the output, the defaults
get placed in UserData\SimpleSongOutput\, you can specify a full path to put
them elsewhere on your computer.

Another feature is using a template file instead of the format string.  You can
look at the template example to see how this can be used, it mimics how HTTPStatus
shows songs with the default setup.  With this you can create an HTML page to use
as a browser source, must use an empty and regular template for this to use
auto-refresh (as can be seen in the example).

Default config:

{
  "enabled": true,
  "saveThumbnail": false
  "textFilename": "simplesongoutput.txt",
  "thumbnailFilename": "simplesongoutput.jpg",
  "songTemplate": "",
  "songTemplateEmpty": "",
  "songFormat": "%songname%%songsubname: - % by %levelauthorname% on %difficulty%",
  "thumbnailSize": 256
}
