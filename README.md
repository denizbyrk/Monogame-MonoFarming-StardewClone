# MonoFarming - Stardew Valley Clone

A Stardew Valley Clone that has been created using C# and Monogame Framework.

## Features

- **Farmer**
  - The player itself. 

- **NPC system**
  - You can interact with NPC's. Currently there is only one villager in the farm, but you can add more if you want.
    
- **NPC Dialogues**
  - NPC's can have their own dialogues.
  
- **Maps and map transitions**
  - 5 different areas (Farm, Lake, Forest, Mountain[W.I.P.], and Farm House) are present in the game.
  - 4 of them are exterior areas, the only interior area being the Farm House.
  - You begin at the Farm area, Lake is on east, Forest on south, Mountain on north, and Farm House is located at top right corner of the Farm.
 
- **Camera**
  - Smooth camera system that follows the farmer.
  - Press Z to zoom in, X to zoom out, and C to reset zoom.
    
- **Map objects (stone, wood, grass etc.)**
  - Map objects are randomly spawned each time you restart the game.
   
- **Object interactions**
  - You can destroy the objects with tools.

- **Item drops**
  - You can drop resources by breaking map objects with your tools.
     
- **Inventory system**
  - Store your tools and picked up items at your inventory. Default stack size is 10. You can move items between slots by left clicking and dragging, and you can grab just one item from the stack by right clicking.
   
- **Energy bar and energy depletion**
  - You will deplete your energy by 2 when using tools, you can check your energy level by hovering your mouse over the energy bar.
    
- **Tools and tool usage**
  - Left click a tool to use it on map objects.
  - Use axe for woods, pickaxe for stones, scythe for grass, and hoe for tilling(W.I.P.).
  
- **Item info and details**
  - You can display the name and details of items and tools when you hover them.
    
- **Hour-Date-Season system**
  - Although there is no visual or gameplay changes in the game 
- **Day-Night cycle**
  - After the hour is 19.00, the sundawn begins. The sunrise begins after 04.00.
    
- **Sunny/Rainy weater**
  - Each night at 00.00, there is %25 chance that the weather will be rainy for the rest of the day.

- **Animations**
  - Farmer animations, Villager animations, Rain animations, and particle animations are implemented. 
  
- **Debug mode**
  - Press CTRL + P to toggle debug mode.
  - You can see the hitbox of the player, adjacent interactable tiles, and collidable tiles(collision is not implemented) while in debug mode.
  - While in debug mode, press CTRL + M to toggle rain.
  - While in debug mode, press CTRL + N to toggle time speed.
 
- **Changing Screen Size**
  - Press F1 to change window size.
  - After pressing F1, the screen size would go larger, if the current size is 1280x720, it would go up to 1920x1080. If the next window size is larger than your monitor's display size, it goes back to the smallest, which is 640x360. 
  - There are 6 possible window dimensions, which are: 640x360, 1280x720, 1920x1080, 2560x1440, 3200x1800, 3840x2160. The window size won't go larger than your monitor's display settings (i.e. if your display is set at 1920x1080, you would have only 3 options.). 
  - Press F2 to go fullscreen mode.
  - At the time being, the only perfect window size is 1280x720. While changing the screen size feature is almost completed, there are still some bugs.
    - Mouse position and tile indicator does not appear on their correct positions. This can be fixed by setting the window size to 1280x720.
    - Energy bar is only at it's correct position while the window size is 1280x720.  

### Unimplemented features

- Collisions
- Dropping items from inventory
- Growing/Harvesting
- Storage/Crafting
- Shopping/Trading
- Combat
- Quests
- Charater customization
  
**Since I am focused on my career based projects, I don't know if I'll ever implement these. Feel free to fork and contrubuting by implementing these features.**

## Screenshots 

After starting the game, you spawn at Farm Map, in front of your Farm House.

<img src="https://github.com/user-attachments/assets/15cae492-020c-4fc1-a59f-582db2571eea" width="1000">

### Dialogues

<img src="https://github.com/user-attachments/assets/dd16d61f-2547-4262-b4e7-54698574613b" width="270">
<img src="https://github.com/user-attachments/assets/05cb7ef7-0cb0-4d22-bbee-3a353455c3b9" width="270">
<img src="https://github.com/user-attachments/assets/510a9503-48e3-4226-b87f-a58ba1d8dcde" width="270">

### Maps

<img src="https://github.com/user-attachments/assets/e008a270-8a51-44b0-9a24-c6b72b5ea352" width="400">
<img src="https://github.com/user-attachments/assets/3ba8f450-306f-420e-8a48-0c674127167d" width="400">
<img src="https://github.com/user-attachments/assets/8ee18f8e-e11f-47c4-a887-664473de546b" width="400">
<img src="https://github.com/user-attachments/assets/b4268b81-46a7-4e9f-8808-cdc2d3c936d4" width="400">

### Breaking Objects

**Might take some time to load**

<img src="https://github.com/user-attachments/assets/db201940-2f6b-4ffb-b9c0-b3be96e9a4c7" alt="Loading..." width="500">

### Inventory

<img src="https://github.com/user-attachments/assets/146e59a8-4e6b-4c12-9f1d-5a56e47359e3" width="400">
<img src="https://github.com/user-attachments/assets/1ab9555a-9c3b-47d9-91f6-735c10869aee" width="400">

### Tooltips

**Tooltips in main inventory**

<img src="https://github.com/user-attachments/assets/54a68eea-c2a1-4a25-bdd1-7f09a84a9fde" width="500">
<img src="https://github.com/user-attachments/assets/6eba976e-7a89-408c-bf32-0d2364f87bf6" width="500">
<img src="https://github.com/user-attachments/assets/a3fcc0ac-39dc-442d-8783-abf998d8a0a3" width="500">

**Tooltips in whole inventory**

<img src="https://github.com/user-attachments/assets/35d43b85-4682-4445-9a77-04ffe439f38b" width="500">
<img src="https://github.com/user-attachments/assets/960c3386-fbf5-455d-aa6c-5d6e94d3ed60" width="500">

### Rain

<img src="https://github.com/user-attachments/assets/2d641fa8-b836-4810-8672-d5d36f43b1b0" width="1000">

### Day-Night Cycle

<img src="https://github.com/user-attachments/assets/6cdac97a-4cf3-4993-b1de-ae7e27ada80d" width="400">
<img src="https://github.com/user-attachments/assets/7b7c8c4e-5bb3-4dfc-9853-426d77b28135" width="400">
<img src="https://github.com/user-attachments/assets/c05df342-dec0-4364-8932-491c52767082" width="400">
<img src="https://github.com/user-attachments/assets/b829141a-6cff-47ba-a5e0-66f68cb2076f" width="400">

### Energy Usage

<img src="https://github.com/user-attachments/assets/d78d1015-1346-4cb8-8a7d-93b090816aa3" width="100">
<img src="https://github.com/user-attachments/assets/aee7005c-81e1-4bcf-b946-f5bd922053d8" width="100">
<img src="https://github.com/user-attachments/assets/b67f8326-4dea-4082-beb1-58c7cdaae15e" width="100">
<img src="https://github.com/user-attachments/assets/8352a3fd-533c-485a-996b-5b376e22faf5" width="100">

### Debug Mode

<img src="https://github.com/user-attachments/assets/84ac4f43-6754-4a1e-84a4-94ab73715e0d" width="1000">

## Installation

The following are the instructions for running the code:

- Download the project as a ZIP file or clone it using GitHub Desktop.
- Open the project in Visual Studio or VS Code.
- Make sure you have C# installed.
  - If you are using Visual Studio, Install .NET desktop development.
  - If you are using VS Code, run the following command:
    
  ```
  code --install-extension ms-dotnettools.csharp
  ```
  
  - To verify installation run the following command:
    
  ```
  dotnet --version
  ```
  
- Install Monogame
  - If you are using Visual Studio you can install Monogame through Extensions window.
  - If you are using VS Code, you can run the following command to install Monogame Templates:
    
  ```
  dotnet new --install MonoGame.Templates.CSharp
  ```

- Run the code

**Important**

You may encounter some problems regarding font rendering. Make sure you have Press Start 2P font installed, or the game may not run. If you cannot install it for any reason, you can change the font manually.

To change the font, open the project, navigate throught "Content -> Font -> Font.spritefont". Then, find the font name field and change it to any font you have installed on your computer.
```
<FontName>Press Start 2P</FontName>
```
Change the "Press Start 2P" to font of your choice.

Don't forget to edit "DescriptionFont.spritefont" file aswell.

## Controls and How to Use

- Use WASD to move around.
- Press L-Shift to sprint at high speeds for faster traveling.
- Press E to toggle inventory.
- Left-click to use tools and selecting items.
- Right-click to select one item from stack.
- Left/Right-click to interact with NPC's and reading dialogues.
- Scroll to change the selected item in the while inventory is not opened.
- Hover over items for item details.
- Press Z to zoom in, X to zoom out, C to reset zoom.
- CTRL + P for debug mode. While in debug mode:
  - Press CTRL + M to toggle rain.
  - Press CTRL + N to toggle time speed. 

## Code

Here is the brief explanation for what the classes are responsible for.

While some classes have detailed comments about what the codes are responsible for, I didn't had the time to add comments to all of the code. But some of the more important stuff are commented for better explanation.

- **Main.cs:** The code starts up from here. It responsible for loading data, updating, and drawing. You can change the screen size and running speed from here.

- **Scene**
  - **SceneManager.cs:** Base class for scenes.

  - **Scenes** 
    - **Splash.cs:** The splash screen of the game. The game does have a splash state, but it is unaccessible without changing the code. To access splash state do the following:
      - At Main.cs class, set the currentScene to new Splash(), and set the currentGameScene to Scenes.Splash, and run the program again. Currently, these are located on lines 72 and 73 on Main.cs class.
       
    - **Overworld.cs:** The state that the actual gameplay is taking part. It binds together the player, all the maps, UI's, NPC's,
    - **Menu.cs:** There is no main menu implemented in the game. 

- **Util**
  - **Camera.cs:** Camera that follows the player. At the 107th line of the Player.cs class, the method for moving the camera towards the player can be found. You change the move percantage to different values to change the smoothness of the camera. The value is 0.07f by default. 
  - **Helper.cs:** Responsible for drawing screen transitions, drawing lines, drawing outlines, drawing text outlines, drawing grids(visible in debug mode), drawing mouse cursor, getting the tiles that the mouse is on, drawing tile indicator, and checking collisions(Not implemented in the gameplay).
  - **Input.cs:** Responsible for mouse and keyboard detection. The project used to have keyboard inputs for debugging.
  - **JsonHelper.cs:** Class for reading item and tool data.
  - **Sprite.cs:** Class that stores the data for rendering sprites, including texture, position, rotation, scale, and effects.

- **Map**
  - **Tile.cs:** The tile class that contains the properties for a single tile.
  - **MapLoader.cs:** Base class of all maps. Responsible for loading, updating, and drawing the maps. Contains the data for currents map's name, width and height (in tiles), boundaries, interactable tiles, tillable tiles, particles, dropped items, transitions, and map objects. Loads the map layer by reading the CSV file. The maps are rendered layer-by-layer.
  - **MapObject.cs:** Base class of all map objects. Interactable and collidable objects that spawn on the map.
  - **DayTime.cs:** Class for managing day state, hour, day, and season. Also handles the day-night cycle.
  - **Light.cs:** Implemented but unused because not working as intended. You can still create a light object in one of the maps and test it by yourself. My goal was to merge the night overlay with light texture, and then apply the multiply effect to make the light appear flawless. This probably can be obtained by using RenderTarget2D, but I couldn't get it to work. You can try to implement it by yourself.
  - **AnimatedTile.cs:** Unimplemeted.
    
  - **Maps**
    - **FarmMap.cs:** Class for loading the Farm Map. It is the only map that has an NPC object. 
    - **HouseMap.cs:** Class for loading the House Map. It is the only interior map available.
    - **LakeMap.cs:** Class for loading the Lake Map.
    - **ForestMap.cs:** Class for loading the Forest Map.
    - **MountainMap.cs:** Class for loading the Mountain Map (W.I.P.).
   
  - **Map Objects**
    - **Fiber.cs:** Subclass of MapObject. Defines the properites for Fiber object.
    - **Stone.cs:** Subclass of MapObject. Defines the properites for Stone object.
    - **Wood.cs:** Subclass of MapObject. Defines the properites for Wood object.

  - **Weather**
    - **Rain.cs:** Contains a list of Raindrops and renders the rain. Rain density and rain speed values can be changed.
    - **Raindrop.cs:** Class for creating a single raindrop. Each raindrop has a random speed, random lifetime, and random opacity.

- **Entity**
  - **Player.cs:** The Player class, responsible for managing movement, animations, logics, inventory of the player, tools, tool usages, and player hitboxes. There are some incomplete methods for collision checking, you can ignore or complete them. 
  - **Energy.cs:** Class for managing the energy. The player's current energy level is being stored here. Max energy can be changed.
  - **ItemDrop.cs:** Class for managing the behaviour of dropped items. Each dropped item moves in a random direction, and if the player is close enough, they are being pulled towards the player and collected.
  - **NPC.cs:** Base class for NPC's.
  - **Villager.cs:** The only NPC in the game, appearing in Farm Map. It has it's own dialogues. You can add/delete/change it's dialogues.
  - **Particle.cs:** Class for managing the particles that appear when a map object is destroyed.

  - **UI**
    - **Inventory.cs:** Class for managing the inventory. MainInventory is the inventory that appears at the bottom of the screen, and WholeInventory is the inventory that appears when the inventory opens. It contains the List for all Inventory Cells.
    - **Hud.cs:** Class for updating and drawing the date, energy bar, and picked up items bar (the bar that appears at the bottom left corner of the screen).   
    - **ItemDescriptions.cs:** Class for drawing item details. Each item has different name and description size. The class handles the size of the bar according to length of the strings.
    - **Button.cs:** Class for updating and drawing buttons.
    - **Dialog.cs:** Class for dialogue boxes.
    - **Pickup.cs:** Class for managing the length of the picked up item bar (the bar that appears at the bottom left corner of the screen).
    - **EnergyBar.cs:** Class for a single bar of energy.
    - **EntityIndicator.cs:** The indicator that appears on top an NPC when you get close to them. This can also be used to indicate things like chests, furnaces, and other items, which are not implemented in this game.
   
  - **Inventory Util**
    - **InventoryCell.cs:** Class for a single inventory cells. Stores the item that is in the cell, the count of the item, and the max item count of the cell.
    - **Item.cs:** Class for an item that is in an inventory cells. It loads the item by getting it's ID, and stores the sprite, name, description, type, and other properties of the item.
    - **ItemData.cs:** Class for storing item data (They are being loaded in Main.cs).
    - **Tool.cs:** Class for loading tools. It load's the tool by getting the ID of the tool, and stores the sprite, name, description, type, and other properties of the tool.
    - **ToolData.cs:** Class for storing tool data (they are being loaded in Main.cs).

- **Animations**
  - **Animation.cs:** Animation manager class.
    
  - **Animators**
    - **PlayerAnimator.cs:** Class for managing player animations.
    - **ParticleAnimator.cs:** Class for managing particle animatons.

- **Audio**
  - **SoundEffectAudio.cs:** Class for loading and playing the sound effects.  
  - **AmbienceAudio.cs:** Class for loading and playing the ambience sound effects.
