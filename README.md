# ChatAppServer

Simple Chat server, written in c# using SignalR.
<br/>
features:
<br/>
Direct Messages.
<br/>
Reconnection mechanism, using a message queue, with a timeout of 15 seconds.
<br/>
Persistent db.
<br/>
JWT Authentication, using an http header instead of cookies, to maintain authorization outside of a browser's environment (like a mobile app).
<br/>
Game Invitation - a blazor app project is hosted at the wwwroot folder, when a player accepts an invite, the game is initialized.
                    the game code is here: https://github.com/tsotso123/MyBlazorGameEngine
  
