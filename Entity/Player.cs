using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using MonoFarming.Util;
using MonoFarming.Audio;
using MonoFarming.Entity.UI;
using MonoFarming.Scene.Scenes;
using MonoFarming.Entity.InventoryUtil;
using MonoFarming.Animations.Animators;

namespace MonoFarming.Entity {
    public class Player {

        private Sprite Farmer; //farmer sprite
        private byte facing = 2; //the way the farmer is facing / 1=right / 2=down / 3=left / 4=up
        private int farmerPosX; //x position
        private int farmerPosY; //y position
        public static bool freeze = false; //check freeze
        public static bool canAction = false; //check if can action
        public static bool isInterior = false; //check if indoors
        private bool isHoldingTool = true; //check tool hold
        public static bool isUsingTool = false; //check tool usage

        private Vector2 farmerVelocity; //farmer velocity
        private float farmerSpeed; //speed
        private float farmerWalkSpeed; //walk speed
        private float farmerSpeedSprint; //sprint speed

        private Rectangle farmerHitbox; //farmer hitbox
        public List<Rectangle> xAxisCollisionTiles = new List<Rectangle>(); //adjacent tiles horizontally
        public List<Rectangle> yAxisCollisionTiles = new List<Rectangle>(); //adjacent tiles vertically
        public List<Rectangle> adjacentRectangles = new List<Rectangle>(); //adjacent tiles to the player

        private PlayerAnimator animator; //player animator

        public Inventory Inventory; //inventory
        public Item CurrentItem; //current item
        private Texture2D tools; //tool's texture
        //tools
        private Item Pickaxe; 
        private Item Axe;
        private Item Hoe;
        private Item Scythe;
        public static Tool.ToolType CurrentTool;

        public Player() {

            //set the player position to current map's default spawning position
            this.farmerPosX = Overworld.currentMap.defaultX;
            this.farmerPosY = Overworld.currentMap.defaultY;

            //create farmer sprite
            var farmerTexture = Main.contentManager.Load<Texture2D>("Sprites\\Farmer");
            var farmerPosition = new Vector2((this.farmerPosX * Main.targetTileSize) - 4, (this.farmerPosY * Main.targetTileSize) - 8);

            this.Farmer = new Sprite(farmerTexture, farmerPosition);

            //set farmer rectangle and hitbox
            this.Farmer.Rectangle = new Rectangle(0, 0, 24, 24);
            this.farmerHitbox = new Rectangle((int)this.Farmer.Position.X, (int)this.Farmer.Position.Y, this.Farmer.Rectangle.Width, this.Farmer.Rectangle.Height);

            //set farmer velocity and speed
            this.farmerVelocity = new Vector2(0, 0);
            this.farmerWalkSpeed = 1f;
            this.farmerSpeedSprint = 5f;
            this.farmerSpeed = this.farmerWalkSpeed;

            //initialize player animator
            this.animator = new PlayerAnimator();

            //initialize inventory
            this.Inventory = new Inventory();

            //create tools
            this.tools = Main.contentManager.Load<Texture2D>("Sprites\\Tools");

            this.Pickaxe = new Tool(1, this.tools, this.Inventory.InventoryItemCells[0].Sprite.Position);
            this.Pickaxe.SetToolProperties((this.Pickaxe as Tool).Name, (this.Pickaxe as Tool).Description, (this.Pickaxe as Tool).canBreak);

            this.Axe = new Tool(0, this.tools, this.Inventory.InventoryItemCells[1].Sprite.Position);
            this.Axe.SetToolProperties((this.Axe as Tool).Name, (this.Axe as Tool).Description, (this.Axe as Tool).canBreak);

            this.Hoe = new Tool(2, this.tools, this.Inventory.InventoryItemCells[2].Sprite.Position);
            this.Hoe.SetToolProperties((this.Hoe as Tool).Name, (this.Hoe as Tool).Description, (this.Hoe as Tool).canBreak);

            this.Scythe = new Tool(3, this.tools, this.Inventory.InventoryItemCells[3].Sprite.Position);
            this.Scythe.SetToolProperties((this.Scythe as Tool).Name, (this.Scythe as Tool).Description, (this.Scythe as Tool).canBreak);

            //add tools to inventory
            this.Inventory.InventoryItemCells[0].AddItem(this.Pickaxe);
            this.Inventory.InventoryItemCells[1].AddItem(this.Axe);
            this.Inventory.InventoryItemCells[2].AddItem(this.Hoe);
            this.Inventory.InventoryItemCells[3].AddItem(this.Scythe);
            this.Inventory.InventoryItems.Add(0, this.Inventory.InventoryItemCells[0]);
            this.Inventory.InventoryItems.Add(1, this.Inventory.InventoryItemCells[1]);
            this.Inventory.InventoryItems.Add(2, this.Inventory.InventoryItemCells[2]);
            this.Inventory.InventoryItems.Add(3, this.Inventory.InventoryItemCells[3]);
            this.Inventory.availableCells -= 4;

            //set the current tool as pickaxe
            Player.CurrentTool = Tool.ToolType.Pickaxe;
        }

        //method for moving camera towards the player
        private void MoveCameraTowardsPlayer(GameTime dt) {

            Overworld.Camera.MoveToward(new Vector2(this.Farmer.Position.X, this.Farmer.Position.Y), (float)dt.ElapsedGameTime.TotalMilliseconds, 0.07f);
        }

        //check of map change and set the player's position accordingly
        public void CheckMapChange() {

            this.Farmer.Position.X = Overworld.currentMap.defaultX * Main.targetTileSize - 4;
            this.Farmer.Position.Y = Overworld.currentMap.defaultY * Main.targetTileSize - 8;

            Overworld.mapChanged = false;
        }

        private void CheckCollision(Rectangle collision) {

            if (this.farmerVelocity.X < 0 && Helper.CheckRectangleCollisionRight(this.farmerHitbox, collision, this.farmerVelocity) == true) {

                this.farmerVelocity.X = MathHelper.Clamp(this.farmerVelocity.X, 0, this.farmerSpeed);
            }

            if (this.farmerVelocity.Y < 0 && Helper.CheckRectangleCollisionTop(this.farmerHitbox, collision, this.farmerVelocity) == true) {


            }

            if (this.farmerVelocity.Y > 0 && Helper.CheckRectangleCollisionBottom(this.farmerHitbox, collision, this.farmerVelocity) == true) {


            }
        }

        private void HandleCollisions() {

            foreach (var c in Overworld.currentMap.collisionTiles) {

                this.CheckCollision(c.Key.tileBounds);
            }
        }

        private void Movement(GameTime dt) {

            if (Player.isUsingTool == true) {

                Player.freeze = true;

            } else if (Overworld.doTransition == false && Player.isUsingTool == false) {

                Player.freeze = false;
            }

            this.farmerSpeed = Input.IsKeyHold(Microsoft.Xna.Framework.Input.Keys.LeftShift) ? this.farmerSpeed = this.farmerSpeedSprint : this.farmerWalkSpeed;

            farmerVelocity = Vector2.Zero;

            if (Player.freeze == false) {

                //foreach (var r in Overworld.currentMap.collisionTiles) {

                //    if (this.farmerHitbox.Intersects(r.Key.tileBounds)) {

                //        if (this.facing == 1) this.Farmer.Position.X -= this.farmerSpeed;
                //        else if (this.facing == 3) this.Farmer.Position.X += this.farmerSpeed;

                //        if (this.facing == 4) this.Farmer.Position.Y += this.farmerSpeed;
                //        else if (this.facing == 2) this.Farmer.Position.Y -= this.farmerSpeed;
                //    }
                //}

                if (Input.IsKeyHold(Microsoft.Xna.Framework.Input.Keys.D)) {

                    this.facing = 1;
                    this.farmerVelocity.X = this.farmerSpeed;
                }
                if (Input.IsKeyHold(Microsoft.Xna.Framework.Input.Keys.A)) {

                    this.facing = 3;
                    this.farmerVelocity.X = -this.farmerSpeed;
                }

                if (Input.IsKeyHold(Microsoft.Xna.Framework.Input.Keys.W)) {

                    this.facing = 4;
                    this.farmerVelocity.Y = -this.farmerSpeed;
                }
                if (Input.IsKeyHold(Microsoft.Xna.Framework.Input.Keys.S)) {

                    this.facing = 2;
                    this.farmerVelocity.Y = this.farmerSpeed;
                }

                if (this.farmerVelocity.X != 0 && this.farmerVelocity.Y != 0) {
                
                    this.farmerVelocity.X /= (float)Math.Sqrt(this.farmerSpeed * 2);
                    this.farmerVelocity.Y /= (float)Math.Sqrt(this.farmerSpeed * 2);
                }

                if (this.farmerVelocity.X > 0 && this.farmerVelocity.Y < 0 || this.farmerVelocity.X > 0 && this.farmerVelocity.Y > 0) this.facing = 1;
                else if (this.farmerVelocity.X < 0 && this.farmerVelocity.Y < 0 || this.farmerVelocity.X < 0 && this.farmerVelocity.Y > 0) this.facing = 3;

            } else {

                this.farmerVelocity = Vector2.Zero;
            }

            this.Farmer.Position.X += this.farmerVelocity.X;
            this.Farmer.Position.Y += this.farmerVelocity.Y;

            this.farmerPosX = (int)Math.Ceiling(this.Farmer.Position.X / Main.targetTileSize);
            this.farmerPosY = (int)Math.Ceiling(this.Farmer.Position.Y / Main.targetTileSize);

            this.farmerHitbox = new Rectangle((int)this.Farmer.Position.X + 7, (int)this.Farmer.Position.Y + 15, Main.targetTileSize - 4, Main.targetTileSize - 6);
        }

        private void Animate() {

            if (Player.freeze == true) this.animator.SetAnimation("Idle", this.facing);

            if (Player.isUsingTool == true) {

                this.animator.SetAnimation("UseTool", this.facing);
                return;

            } else if (this.farmerVelocity == Vector2.Zero && Player.isUsingTool == false) {

                this.animator.SetAnimation("Idle", this.facing);
                return;

            } else if (this.farmerVelocity != Vector2.Zero && Player.isUsingTool == false) {

                this.animator.SetAnimation("Run", this.facing);
                return;
            }
        }

        private void getAdjacentTiles() {

            this.adjacentRectangles.Clear();
            this.xAxisCollisionTiles.Clear();
            this.yAxisCollisionTiles.Clear();

            Vector2 PlayerPosition = new Vector2(this.farmerPosX * Main.targetTileSize, this.farmerPosY * Main.targetTileSize);

            this.adjacentRectangles.Add(new Rectangle(this.farmerPosX * Main.targetTileSize, this.farmerPosY * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));

            this.adjacentRectangles.Add(new Rectangle((this.farmerPosX + 1) * Main.targetTileSize, this.farmerPosY * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.adjacentRectangles.Add(new Rectangle((this.farmerPosX - 1) * Main.targetTileSize, this.farmerPosY * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.xAxisCollisionTiles.Add(new Rectangle((this.farmerPosX + 1) * Main.targetTileSize, this.farmerPosY * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.xAxisCollisionTiles.Add(new Rectangle((this.farmerPosX - 1) * Main.targetTileSize, this.farmerPosY * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));

            this.adjacentRectangles.Add(new Rectangle(this.farmerPosX * Main.targetTileSize, (this.farmerPosY + 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.adjacentRectangles.Add(new Rectangle(this.farmerPosX * Main.targetTileSize, (this.farmerPosY - 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.yAxisCollisionTiles.Add(new Rectangle(this.farmerPosX * Main.targetTileSize, (this.farmerPosY + 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.yAxisCollisionTiles.Add(new Rectangle(this.farmerPosX * Main.targetTileSize, (this.farmerPosY - 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));

            this.adjacentRectangles.Add(new Rectangle((this.farmerPosX + 1) * Main.targetTileSize, (this.farmerPosY - 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.adjacentRectangles.Add(new Rectangle((this.farmerPosX - 1) * Main.targetTileSize, (this.farmerPosY + 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.adjacentRectangles.Add(new Rectangle((this.farmerPosX - 1) * Main.targetTileSize, (this.farmerPosY - 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));
            this.adjacentRectangles.Add(new Rectangle((this.farmerPosX + 1) * Main.targetTileSize, (this.farmerPosY + 1) * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize));

            foreach (Rectangle r in this.adjacentRectangles) {

                if (Helper.currentTilePosition == new Vector2(r.X, r.Y)) {

                    Player.canAction = true;
                    Helper.tileCursorAlpha = 1f;
                    Helper.tileCursorColor = Color.White;

                    break;

                } else {

                    Player.canAction = false;
                    Helper.tileCursorAlpha = 1f;
                    Helper.tileCursorColor = new Color(255, 60, 90);
                }
            }
        }

        private void CheckHoldingTool() {

            if (!this.CurrentItem.Type.Equals(null) && this.CurrentItem.Type.Equals("Tool")) {

                this.isHoldingTool = true;

                switch (this.CurrentItem.Name) {

                    case "Pickaxe":

                        Player.CurrentTool = Tool.ToolType.Pickaxe;
                        break;

                    case "Axe":

                        Player.CurrentTool = Tool.ToolType.Axe;
                        break;

                    case "Hoe":

                        Player.CurrentTool = Tool.ToolType.Hoe;
                        break;

                    case "Scythe":

                        Player.CurrentTool = Tool.ToolType.Scythe;
                        break;

                    default:

                        Player.CurrentTool = Tool.ToolType.None;
                        break;
                }

            } else {

                this.isHoldingTool = false;
            }
        }

        private void UseTools() {

            if (Player.canAction && this.isHoldingTool == true) {

                foreach (var tillableTile in Overworld.currentMap.tillableTiles.ToList()) {

                    if (Player.CurrentTool == Tool.ToolType.Hoe) {

                        if (Helper.currentTilePosition == new Vector2(tillableTile.Key.tilePosition.X * Main.targetTileSize, tillableTile.Key.tilePosition.Y * Main.targetTileSize)) {

                            if (Input.IsLeftClickDown()) {

                                Player.isUsingTool = true;

                                Overworld.currentMap.interactableTiles[tillableTile.Key] = 1316;

                                SoundEffectAudio.SoundEffectInstances["UseHoe"].Stop();
                                SoundEffectAudio.SoundEffectInstances["UseHoe"].Pitch = (float)Main.random.Next(0, 5) / 10f;
                                SoundEffectAudio.SoundEffectInstances["UseHoe"].Play();

                                //if (Overworld.currentMap.hoedTiles.ContainsKey(tillableTile.Key) == false) {

                                //    Overworld.currentMap.hoedTiles.Add(tillableTile.Key, 1316);

                                //    SoundEffectAudio.SoundEffectInstances["UseHoe"].Stop();
                                //    SoundEffectAudio.SoundEffectInstances["UseHoe"].Pitch = (float)Main.random.Next(0, 5) / 10f;
                                //    SoundEffectAudio.SoundEffectInstances["UseHoe"].Play();
                                //}

                                Energy.decreaseEnergy = true;

                                break;
                            }
                        }
                    }
                }

                foreach (var tile in Overworld.currentMap.interactableTiles) {

                    if (Helper.currentTilePosition == new Vector2(tile.Key.tilePosition.X * Main.targetTileSize, tile.Key.tilePosition.Y * Main.targetTileSize)) {

                        if (Input.IsLeftClickDown() && Energy.playerEnergy > 0 && Player.CurrentTool != Tool.ToolType.None) {

                            bool objectDestroyed = false;
                            Player.isUsingTool = true;

                            if (Player.CurrentTool == Tool.ToolType.Pickaxe) {

                                if (tile.Key.objectOnTile.name.Equals("Stone")) {

                                    Overworld.currentMap.destroyInteractableObjects(tile, tile.Key.objectOnTile);
                                    objectDestroyed = true;

                                    SoundEffectAudio.SoundEffectInstances["BreakStone1"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["BreakStone2"].Stop();

                                    int randomSound = Main.random.Next(2);

                                    if (randomSound == 0) {

                                        SoundEffectAudio.SoundEffectInstances["BreakStone1"].Play();

                                    } else {

                                        SoundEffectAudio.SoundEffectInstances["BreakStone2"].Play();
                                    }
                                }
                            }

                            if (Player.CurrentTool == Tool.ToolType.Axe) {

                                if (tile.Key.objectOnTile.name.Equals("Wood")) {

                                    Overworld.currentMap.destroyInteractableObjects(tile, tile.Key.objectOnTile);
                                    objectDestroyed = true;

                                    SoundEffectAudio.SoundEffectInstances["UseAxe"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["UseAxe"].Pitch = (float)Main.random.Next(-2, 5) / 10f;
                                    SoundEffectAudio.SoundEffectInstances["UseAxe"].Play();
                                }
                            }

                            if (Player.CurrentTool == Tool.ToolType.Scythe) {

                                if (tile.Key.objectOnTile.name.Equals("Fiber")) {

                                    Overworld.currentMap.destroyInteractableObjects(tile, tile.Key.objectOnTile);
                                    objectDestroyed = true;

                                    SoundEffectAudio.SoundEffectInstances["Cut"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["Cut"].Pitch = (float)Main.random.Next(0, 5) / 10f;
                                    SoundEffectAudio.SoundEffectInstances["Cut"].Play();
                                }
                            }

                            if (objectDestroyed == true) {

                                Vector2 particlePos = new Vector2(tile.Key.tilePosition.X, tile.Key.tilePosition.Y);
                                Particle p = new Particle(particlePos);
                                p.particleID = tile.Key.objectOnTile.id;
                                Overworld.currentMap.Particles.Add(p);
                            }

                            if (Player.CurrentTool != Tool.ToolType.Scythe) Energy.decreaseEnergy = true;

                            break;
                        }
                    }
                }
            }
        }

        public void CheckItemPickup(ItemDrop i) {

            if (i.itemDestroyed == true) {

                SoundEffectAudio.SoundEffectInstances["Pickup"].Pitch = (float)Main.random.NextDouble();
                SoundEffectAudio.SoundEffectInstances["Pickup"].Stop();
                SoundEffectAudio.SoundEffectInstances["Pickup"].Play();

                Item item = new Item();

                item.ID = ItemData.Data[i.itemID].ID;
                item.Name = ItemData.Data[i.itemID].Name;
                item.Description = ItemData.Data[i.itemID].Description;
                item.isStackable = ItemData.Data[i.itemID].isStackable;
                item.Type = ItemData.Data[i.itemID].Type;

                if (this.Inventory.isFull == false) {

                    for (int j = 0; j < this.Inventory.InventoryItemCells.Count; j++) {

                        item.Sprite = new Sprite(i.itemTexture, new Vector2(this.Inventory.InventoryItemCells[j].Sprite.Position.X + 22, this.Inventory.InventoryItemCells[j].Sprite.Position.Y + 24));
                        item.Sprite.Origin = new Vector2(8, 8);
                        item.Sprite.Rectangle = new Rectangle((item.ID - 1) * 16, 0, 16, 16);
                        item.Sprite.Scale = 3f;

                        if (this.Inventory.InventoryItemCells[j].hasItem == true) {

                            if (this.Inventory.InventoryItemCells[j].Item.ID == item.ID) {

                                if (this.Inventory.InventoryItemCells[j].itemCount < this.Inventory.InventoryItemCells[j].maxItemCount) {

                                    this.Inventory.InventoryItemCells[j].itemCount++;
                               
                                } else {

                                    continue;
                                }

                                break;

                            } else {

                                continue;
                            }
                        }

                        if (this.Inventory.InventoryItemCells[j].hasItem == false) {

                            if (item.isStackable == true) {

                                this.Inventory.InventoryItemCells[this.Inventory.InventoryItemCells[j].CellID].Item = item;

                                if (this.Inventory.InventoryItems.ContainsKey(j)) {

                                    this.Inventory.InventoryItems[j].Item = item;

                                } else {

                                    this.Inventory.InventoryItems.Add(this.Inventory.InventoryItemCells[j].CellID, this.Inventory.InventoryItemCells[j]); /////
                                }

                                this.Inventory.InventoryItemCells[j].itemCount = 1;
                                this.Inventory.InventoryItemCells[j].hasItem = true;
                                this.Inventory.availableCells--;

                                if (this.Inventory.availableCells < 0) this.Inventory.isFull = true;

                                System.Diagnostics.Debug.WriteLine(item.Name + " has been added to " + this.Inventory.InventoryItemCells[j].CellID);

                                break;
                            }
                        }
                    }

                    Pickup p = new Pickup(item.Sprite, item.Name);
                    Hud.pickupBars.Add(p);
                }
            }
        }

        public void Update(GameTime dt) {

            this.HandleCollisions();
            if (Player.freeze == false) {

                this.Movement(dt);
            } else {

                this.animator.SetAnimation("Idle", this.facing);
                SoundEffectAudio.SoundEffectInstances["Step"].Stop();
            }
            this.getAdjacentTiles();

            this.MoveCameraTowardsPlayer(dt);

            this.Inventory.UpdateInventory(dt);
            this.CurrentItem = this.Inventory.GetCurrentItem();
            this.CheckHoldingTool();

            if (this.Inventory.invetoryOpened == false) this.UseTools();

            this.Farmer.Rectangle = this.animator.Animate(dt, 24, 24);
            this.Animate();

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L)) {

                this.ResetPlayerPosition();
            }
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.Farmer.Texture, this.Farmer.Position, this.Farmer.Rectangle, this.Farmer.Hue, this.Farmer.Rotation, this.Farmer.Origin, this.Farmer.Scale, this.Farmer.Effect, this.Farmer.Depth);

            if (Main.debugMode == true) {

                foreach (Rectangle r in this.xAxisCollisionTiles) {

                    b.Draw(Main.pixel, r, Color.Red * 0.6f);
                }

                foreach (Rectangle r in this.yAxisCollisionTiles) {

                    b.Draw(Main.pixel, r, Color.Red * 0.6f);
                }

                foreach (Rectangle r in this.adjacentRectangles) {

                    b.Draw(Main.pixel, r, Color.Pink * 0.6f);
                }

                Helper.DrawOutlineRectangle(b, (int)this.farmerHitbox.X, (int)this.farmerHitbox.Y, this.farmerHitbox.Width, this.farmerHitbox.Height, 1, Color.DarkBlue);
            }
        }

        private void ResetPlayerPosition() {

            this.farmerPosX = Overworld.currentMap.defaultX;
            this.farmerPosY = Overworld.currentMap.defaultY;

            this.Farmer.Position.X = (this.farmerPosX * Main.targetTileSize) - 4;
            this.Farmer.Position.Y = (this.farmerPosY * Main.targetTileSize) - 6;
        }

        public void setPlayerX(int x) => this.farmerPosX = x;

        public void setPlayerY(int y) => this.farmerPosY = y;

        public int getPlayerX() => this.farmerPosX;

        public int getPlayerY() => this.farmerPosY;

        public Vector2 getPlayerPosition() => new Vector2(this.farmerPosX, this.farmerPosY);

        public Vector2 getPlayerPositionInPixels() => this.Farmer.Position;

        public Rectangle getPlayerHitbox() => this.farmerHitbox;
    }
}