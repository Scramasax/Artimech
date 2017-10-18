# Artimech

Artimech is an open source visual scripting system that creates c# finite state machine code.  
The state machine pattern encourages code distribution and modal logic.  
In other words, the aforementioned design pattern causes classes to be shorter and less 'if than' logic.

1)  Getting Started.
To watch Artimech in action, load the Test.scene in the Scenes directory.  Go to Windows->Artimech and 
place the spawned window.  In the "Game Object" selection box select Cube.  Run the game.  The highlighted box is the active code.

Right-click state window in the body of the state box and select "Edit Script".  The linked editor at this point should be called.

2) Creating your own state machine.
Select a game object in the "Game Object" selector box.  Click File->Create. This will generate a state machine, 
link it to the selected object, and populated the state machine with a "start state".  Be patient as Unity3d needs 
to refresh itself.  At the end of this process, a state should be seen in the Artimech state editor window.

Right, Click on empty space to get an Add State menu.  
There are three choices: Empty, Subscribe and Publish.  
The "Empty" state contains no special logic but is ready to be populated.  
"Subscribe" menu entry copies and renames a template with a subscription system attached.  
"Publish" will publish a message and its own self-using an EventRouter.

3) Right-click on the body of the state.  "Add Condition" menu will popup.  That and "Edit Script" which launches the Unity3d linked script editor.  Add Conditional->Empty Conditional will create an empty conditional with no special code.  Add Conditional->Subscribe Conditional creates conditional code that subscribes to the EventRouter.  
