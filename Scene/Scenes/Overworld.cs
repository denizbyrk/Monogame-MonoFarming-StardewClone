using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using MonoFarming.Map;
using MonoFarming.Util;
using MonoFarming.Audio;
using MonoFarming.Entity;
using MonoFarming.Map.Maps;
using MonoFarming.Map.Weather;
using MonoFarming.Entity.UI;
using MonoFarming.Entity.InventoryUtil;

namespace MonoFarming.Scene.Scenes {
    public class Overworld : SceneManager {

        //maps and camera
        private FarmMap farmMap;
        private LakeMap lakeMap;
        private ForestMap forestMap;
        private MountainMap mountainMap;
        private HouseMap houseMap;
        public static MapLoader currentMap;
        public static Camera Camera;
        public static bool cameraActivate = true;
        private Matrix camMatrix;

        //player
        private Player Player;

        //tile cursor
        private Texture2D tileCursor;

        //HUD
        private Hud Hud;

        public static bool mapChanged; //check if the map has changed
        public static bool displayHud = true; //check if the hud is being displayed
        public static bool freezeTime = false; //freeze/unfreeze time
        public static bool deactivateCamControls = false; //activate/deactivate camera
        public static bool doTransition = false; //do screen transitions while changing maps
        public static bool isDialogShown = false; //check if a dialog is being shown
        
        //create a "Multiply" blend state
        public static BlendState multiply = new BlendState() {

            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero
        };

        //create the night overlay
        private Rectangle Night;
        public static int nightAlpha = 255;
        public static Color nightColor = new Color(255, 255, nightAlpha);
        public static bool isNight = false;

        //a black gradient, drawn when inventory is opened
        private Rectangle gradient;

        //rain
        private Rain RainDrops;
        private Rectangle Rain;
        private Color rainColor = new Color(122, 137, 255);
        public static bool isRaining = false;

        //day states
        public enum DayState {

            Morning,
            Noon,
            Evening,
            Midnight
        }

        public static DayState currentDayState;

        public Overworld() {

            //load item and sound data
            ItemData.Data = JsonHelper.ReadItemData("Content\\Data\\ItemData\\ItemData.json");
            ToolData.Data = JsonHelper.ReadToolData("Content\\Data\\ToolData\\ToolData.json");
            SoundEffectAudio.Load();
            AmbienceAudio.Load();

            //initialize maps
            this.farmMap = new FarmMap();
            this.lakeMap = new LakeMap();
            this.forestMap = new ForestMap();
            this.mountainMap = new MountainMap();
            this.houseMap = new HouseMap();

            Overworld.currentMap = this.farmMap; //set the spawning map to farm map

            this.Player = new Player(); //initialize player

            //initialize the night overlay rectangle, relative to the current screen size
            this.Night = new Rectangle(0, 0, Main.graphics.PreferredBackBufferWidth, Main.graphics.PreferredBackBufferHeight);

            //initialize the raindrops and rain overlay rectangle, relative to the current screen size
            this.RainDrops = new Rain();
            this.Rain = new Rectangle(0, 0, Main.graphics.PreferredBackBufferWidth, Main.graphics.PreferredBackBufferHeight);

            Overworld.Camera = new Camera(); //initialize camera

            this.Hud = new Hud(); //initialize HUD
        }

        public override void LoadContent() {

            this.tileCursor = Main.contentManager.Load<Texture2D>("Sprites\\Tile Cursor");
            this.gradient = new Rectangle(0, 0, Main.screenDimensions[Main.currentScreenSize, 0], Main.screenDimensions[Main.currentScreenSize, 1]);
        }

        //method for updating camera
        private void UpdateCamera(GameTime dt) {

            //update the cam matrix
            this.camMatrix = Overworld.Camera.getTransformation();

            if (Overworld.deactivateCamControls == false) Overworld.Camera.CameraControl();
        }

        //method for map transition
        private void MapTransition(MapLoader newMap) {

            Player.canAction = false; //prevent the player from performing actions
            Player.freeze = true; //freeze the player
            Overworld.doTransition = true; //start the transition

            //if the black screen's transparency is 0, go to next map
            if (Helper.goToNextMap) {

                Overworld.Camera.isInstant = true; //instead of camera slowly going to the players position, make it instant
                Overworld.currentMap = newMap; //set the new map
                Overworld.mapChanged = true; 
                this.Player.CheckMapChange(); //check if the player has changed map
                Overworld.Camera.UpdateZoom(); //update the zoom level of the camera, relative to the current map's size

                //check if the new map is an interior area, and set the player's isInterior attribute accordingly
                if (Overworld.currentMap.isInterior == true) {

                    Player.isInterior = true;

                } else {

                    Player.isInterior = false;
                }
            }
        }

        //method for changing map
        private void ChangeMap(GameTime dt) {

            Vector2 playerPos;

            //iterate through all transition tiles in the current map
            foreach (Vector2 v in Overworld.currentMap.transitionTiles) {

                //get player position
                playerPos = this.Player.getPlayerPosition();

                //check if the player stands on a transition tile
                if (v == playerPos) {

                    //farm map transitions
                    if (Overworld.currentMap == this.farmMap) {

                        if (v == Overworld.currentMap.transitionTiles[0] || v == Overworld.currentMap.transitionTiles[1] || v == Overworld.currentMap.transitionTiles[2]) {

                            this.MapTransition(this.lakeMap);
                        } else if (v == Overworld.currentMap.transitionTiles[3] || v == Overworld.currentMap.transitionTiles[4]) {

                            this.MapTransition(this.mountainMap);
                        } else if (v == Overworld.currentMap.transitionTiles[5] || v == Overworld.currentMap.transitionTiles[6] || v == Overworld.currentMap.transitionTiles[7] || v == Overworld.currentMap.transitionTiles[8]) {

                            this.MapTransition(this.forestMap);
                        } else if (v == Overworld.currentMap.transitionTiles[9]) {

                            this.MapTransition(this.houseMap);
                        }

                        break;

                    //lake map transitions
                    } else if (Overworld.currentMap == this.lakeMap) {

                        this.farmMap.setSpawningX(79);
                        this.farmMap.setSpawningY(12);
                        this.MapTransition(this.farmMap);

                        break;

                    //mountain map transitions
                    } else if (Overworld.currentMap == this.mountainMap) {

                        this.farmMap.setSpawningX(79);
                        this.farmMap.setSpawningY(12);
                        this.MapTransition(this.farmMap);

                        break;
                    
                    //forest map transitions
                    } else if (Overworld.currentMap == this.forestMap) {

                        this.farmMap.setSpawningX(42);
                        this.farmMap.setSpawningY(44);
                        this.MapTransition(this.farmMap);

                        break;

                    //house map transitions
                    } else if (Overworld.currentMap == this.houseMap) {

                        this.farmMap.setSpawningX(63);
                        this.farmMap.setSpawningY(13);
                        this.MapTransition(this.farmMap);

                        break;
                    }
                }
            }
        }

        //method for drawing interactable entity indicator (the indicator above the NPC at farm)
        private void DrawInteractableEntityIndicator() {

            //iterate through the adjacent tiles around the player
            foreach (Rectangle r in this.Player.adjacentRectangles) {

                //get the entity position
                Rectangle entityRect = new Rectangle((int)Math.Ceiling(this.farmMap.getVillagerPosition().X), (int)Math.Ceiling(this.farmMap.getVillagerPosition().Y), Main.targetTileSize, Main.targetTileSize);

                //check if their rectangles intersect
                if (r.Intersects(entityRect) == true) {

                    this.farmMap.DrawIndicator(true);

                    if ((Input.getRelativeMouseRectangle().Intersects(entityRect) == true && (Input.IsLeftClickDown() == true || Input.IsRightClickDown()) && this.farmMap.FarmVillager.Dialog.drawDialog == false) && this.Player.Inventory.invetoryOpened == false) {

                        this.farmMap.FarmVillager.Dialog.currentDialog = 0;
                        this.farmMap.FarmVillager.Dialog.drawDialog = true;
                    }

                    break;

                } else {

                    this.farmMap.DrawIndicator(false);
                }
            }
        }

        //method for movement of dropped items
        private void DroppedItemMovement(GameTime dt) {

            //check if the current map has at least 1 dropped item
            if (Overworld.currentMap.DroppedItems.Count > 0) {

                //iterate through all dropped items
                for (int x = 0; x < Overworld.currentMap.DroppedItems.Count; x++) {

                    ItemDrop i = Overworld.currentMap.DroppedItems[x];

                    i.Update(dt);

                    //check if the item could be picked up, and the player's inventory is not full
                    if (i.canPickUp == true && this.Player.Inventory.isFull == false) {

                        //calculate the distance between item and player
                        float dist = i.CalculateDistance(Player.getPlayerPositionInPixels(), i.dropPosition);

                        //if the distance is smaller than the player's pick up range, move the item towards the player
                        if (dist < this.Player.Inventory.pickupRange) {

                            i.MoveTowards(i, new Rectangle(Player.getPlayerHitbox().X, Player.getPlayerHitbox().Y, Player.getPlayerHitbox().Width / 2, Player.getPlayerHitbox().Height / 2));
                        }

                        this.Player.CheckItemPickup(i);
                    }
                }
            }
        }

        //method for particles
        private void ParticleManager(GameTime dt) {

            //check if there are any particles drawn
            for (int i = 0; i < Overworld.currentMap.Particles.Count; i++) {

                Particle p = Overworld.currentMap.Particles[i];

                p.Update(dt);

                //if the particle completes it's animation, remove it
                if (p.dispose == true) {

                    Overworld.currentMap.Particles.Remove(p);
                }
            }
        }

        //method for toggling inventory
        private void CheckInventoryToggle() {

            System.Diagnostics.Debug.WriteLine(Player.freeze);

            if (this.Player.Inventory.invetoryOpened == true) {

                Player.freeze = true; //freeze the player if the inventory is open
                Overworld.freezeTime = true; //freeze the time if the inventory is open
                Overworld.deactivateCamControls = true; //deactivate camera controls while inventory is open

            } else if (this.Player.Inventory.invetoryOpened == false && Overworld.doTransition == false) {

                //check if a dialog is being shown
                if (Overworld.isDialogShown == false) {

                    Player.freeze = false;
                    Overworld.freezeTime = false;
                    Overworld.deactivateCamControls = false;
                }
            }
        }

        //update method
        public override void Update(GameTime dt) {

            this.UpdateCamera(dt); //update camera

            this.CheckInventoryToggle(); //update inventory toggle

            this.Player.Update(dt); //update player

            this.DrawInteractableEntityIndicator(); //update entity indicator

            this.DroppedItemMovement(dt); //update dropped items

            this.ParticleManager(dt); //update particles

            this.ChangeMap(dt); //update map changes

            this.Hud.Update(dt); //update HUD

            if (Overworld.isRaining == true && Player.isInterior == false) this.RainDrops.Update(dt); //update rain

            Overworld.currentMap.UpdateMap(dt); //update map

            this.PlayAmbienceAudio(); //play ambience audio
        }

        //draw method
        public override void Draw(SpriteBatch b, GameTime dt) {

            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, this.camMatrix);

            //draw the current map
            Overworld.currentMap.DrawMap(b);

            //draw dropped items
            this.DrawDroppedItems(b);

            //draw particles
            this.DrawParticle(b);

            //draw the player and the NPC on farm map, and check their positions and swap their rendering order accordingly
            if ((this.Player.getPlayerPositionInPixels().Y) < this.farmMap.FarmVillager.Sprite.Position.Y) {

                this.Player.Draw(b);
                this.farmMap.DrawNPC(b);

            } else {

                this.farmMap.DrawNPC(b);
                this.Player.Draw(b);
            }

            //draw the map objects that should be drawn in front of the player
            Overworld.currentMap.DrawMapFront(b);

            if (Main.debugMode == true) {

                Helper.DrawGrid(b);
            }

            b.End();

            //draw rain
            if (Overworld.isRaining == true) {

                //check if the player is indoors
                if (Player.isInterior == false) {

                    b.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, this.camMatrix);                    
                    this.RainDrops.DrawRain(b); //draw rain drops
                    b.End();
                }

                //begin the sprite batch by setting the blend mode to multiply
                b.Begin(SpriteSortMode.Deferred, Overworld.multiply);

                b.Draw(Main.pixel, this.Rain, this.rainColor); //draw the rain overlay

                b.End();
            }

            //draw night
            if (Overworld.isNight == true) {

                //begin the sprite batch by setting the blend mode to multiply
                b.Begin(SpriteSortMode.Deferred, Overworld.multiply);

                b.Draw(Main.pixel, this.Night, Overworld.nightColor); //draw the night overlay

                b.End();
            }

            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, this.camMatrix);

            //draw tile cursor
            if (this.Player.Inventory.invetoryOpened == false) Helper.DrawTileCursor(b, this.tileCursor);

            //draw screen transition
            if (Overworld.doTransition == true) Helper.BlackScreenTransition(b, dt);

            b.End();

            //draw HUD
            if (Overworld.displayHud == true) {

                b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

                //if the inventory is open draw the black gradient
                if (this.Player.Inventory.invetoryOpened == true) b.Draw(Main.pixel, this.gradient, Color.Black * 0.4f);

                this.Hud.Draw(b); //draw HUD

                //draw dialog
                if (this.farmMap.FarmVillager.Dialog.drawDialog == true && this.Player.Inventory.invetoryOpened == false) {

                    this.farmMap.FarmVillager.DisplayDialog(b);

                //draw inventory when inventory is opened
                } else if (this.Player.Inventory.invetoryOpened == true) {

                    this.Player.Inventory.DrawWholeInventory(b);

                    this.Player.Inventory.DrawItemCountWholeInventory(b);

                    this.Player.Inventory.DrawWholeInventoryItemDescriptions(b);

                    this.Player.Inventory.DrawPickedUpItem(b);

                //draw inventory when inventory is closed
                } else {

                    this.Player.Inventory.DrawMainInventory(b);

                    this.Player.Inventory.DrawItemCountMainInventory(b);

                    this.Player.Inventory.DrawMainInventoryItemDescriptions(b);
                }

                b.End();
            }
        }

        //method for drawing dropped items
        private void DrawDroppedItems(SpriteBatch b) {

            //check if there is at least one dropped item
            if (Overworld.currentMap.DroppedItems.Count > 0) {

                //iterate through all dropped items and draw
                foreach (ItemDrop i in Overworld.currentMap.DroppedItems) {

                    b.Draw(i.itemTexture, i.dropPosition, new Rectangle(i.itemID * Main.targetTileSize, 0, Main.targetTileSize, Main.targetTileSize), Color.White, 0f, Vector2.Zero, 0.85f, SpriteEffects.None, 0f);
                }
            }
        }

        //method for drawing particles
        private void DrawParticle(SpriteBatch b) {

            if (Overworld.currentMap.Particles.Count > 0) {

                foreach (Particle p in Overworld.currentMap.Particles) {

                    p.Draw(b);
                }
            }
        }

        //method for playing ambience audio
        public void PlayAmbienceAudio() {

            //check for rain
            if (Overworld.isRaining == true) {

                //stop other sounds
                AmbienceAudio.AmbienceSoundInstances["Day"].Stop();
                AmbienceAudio.AmbienceSoundInstances["Night"].Stop();

                //check if the player is indoors or not, and change rain pitch accordingly
                if (Overworld.mapChanged == true) {

                    if (Player.isInterior == true) {

                        AmbienceAudio.AmbienceSoundInstances["Rain"].Stop();
                        AmbienceAudio.AmbienceSoundInstances["Rain"].Pitch = 1f;

                    } else {

                        AmbienceAudio.AmbienceSoundInstances["Rain"].Pitch = -1f;
                    }
                }

                //play rain audio
                AmbienceAudio.AmbienceSoundInstances["Rain"].Play();

            //check if it's noon
            } else if (Overworld.currentDayState == DayState.Noon) {

                //stop other sounds
                AmbienceAudio.AmbienceSoundInstances["Rain"].Stop();
                AmbienceAudio.AmbienceSoundInstances["Night"].Stop();

                //play day audio
                if (Player.isInterior == false) AmbienceAudio.AmbienceSoundInstances["Day"].Play();
                else AmbienceAudio.AmbienceSoundInstances["Day"].Stop();

            //check if it's night or evening
            } else if (Overworld.currentDayState == DayState.Evening || Overworld.currentDayState == DayState.Midnight) {

                //stop other sounds
                AmbienceAudio.AmbienceSoundInstances["Day"].Stop();
                AmbienceAudio.AmbienceSoundInstances["Rain"].Stop();

                //play night audio
                if (Player.isInterior == false) AmbienceAudio.AmbienceSoundInstances["Night"].Play();
                else AmbienceAudio.AmbienceSoundInstances["Night"].Stop();
            }
        }
    }
}