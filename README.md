# R&D App
 A minigame for children made to distract them during surgery.

 ![alt text](https://github.com/vandim199/RnD-App/blob/main/Screenshot.png?raw=true)

 # How to play?
 Download the .apk file from the releases tab.
 Enable unknown sources on your Android phone.
 Install the game by opening the downloaded file and following the instructions.

 After entering the game start dragging with a finger from top to bottom to charge up the slime jump like a slingshot.
 Collect coins to increase your score.
 Be careful not to fall to the bottom.

 # Design choices
My goal was to create a mobile game which requires little input but is very engaging to keep a child distracted during an operation.
Knowing that I narrowed my choices of game genres. For example a first person shooter, although it is engaging, it requires a lot of input and multiple fingers to play and the device needs to be held.
Another limitation is a game which uses levels or progress isn't a good choice, because it's a game played on a non-personal device so that would mean losing progress and other kids starting from the level it was left at. This further narrowed my choices to an endless game using scores like in the days of the arcades.
At first I wanted to make a minecart which is traveling on rails and the player would need to tap on blocks (ores, etc.) to destroy them and pickup the materials to make rails. Then drag the rails from the inventory to the ground in front of the minecart so that it doesn't go off-rails. It would be a constant race of breaking blocks to create a path, then making rails and making a safe path for the minecart. 
Unfortunately this was a complex idea and would take me more than a week to make. So I decided to reuse the scrolling blocks to make an endless jumping game, where you jump by slingshotting yourself in the desired direction by swiping on the screen.
I made it so that you can create and edit levels using a tilemap editing tool called "Tiled", which stores the presets in a grid and then the game chooses random presets to make the endless level.
Finally I wanted some assets to make the game kid friendly and colorful so I drew some pixel art to use in the game.