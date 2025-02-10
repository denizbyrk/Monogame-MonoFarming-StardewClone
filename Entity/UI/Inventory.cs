using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;
using MonoFarming.Util;
using MonoFarming.Audio;
using MonoFarming.Entity.InventoryUtil;

namespace MonoFarming.Entity.UI {
    public class Inventory {

        private Sprite MainInventory;
        private Sprite WholeInventory;
        private Sprite PlayerPanel;
        private Sprite PlayerImage;
        private Sprite Indicator;
        private Sprite InventoryTab;
        private Sprite OptionsTab;
        private Texture2D textureMap;
        public Button backButton;
        public Button exitGameButton;

        public List<InventoryCell> InventoryItemCells = new List<InventoryCell>();
        public List<InventoryCell> InventoryAccessoryCells = new List<InventoryCell>();

        public Dictionary<int, InventoryCell> InventoryItems = new Dictionary<int, InventoryCell>();
        private ItemDescription itemDesc = new ItemDescription(Vector2.Zero, "", "", "");
        private Item NullItem = new Item();
        public Item pickedUpItem = new Item();
        public bool isHoldingItem = false;
        public int pickedUpItemCount = 0;
        private float scale = 1f;

        public bool invetoryOpened = false;
        public float pickupRange = 35f;
        public short currentCell = 0;
        public short availableCells = 35;
        public int currentTab = 1;
        public bool isFull = false;

        public Inventory() {

            this.textureMap = Main.contentManager.Load<Texture2D>("Sprites\\Inventory");

            Vector2 mainINVpos = new Vector2((float)Main.screenDimensions[Main.currentScreenSize, 0] / 2 - 468 / 2, (float)Main.screenDimensions[Main.currentScreenSize, 1] - 100);
            this.MainInventory = new Sprite(this.textureMap, mainINVpos);
            this.MainInventory.Rectangle = new Rectangle(0, 435, 468, 81);
            this.MainInventory.Scale = this.scale;

            Vector2 invBGpos = new Vector2((float)Main.screenDimensions[Main.currentScreenSize, 0] / 2 - this.textureMap.Width / 2, (float)Main.screenDimensions[Main.currentScreenSize, 1] / 2 - 128);
            this.WholeInventory = new Sprite(this.textureMap, invBGpos);
            this.WholeInventory.Rectangle = new Rectangle(0, 0, 640, 256);
            this.WholeInventory.Scale = this.scale;

            for (int i = 0; i < 9; i++) {

                Sprite invBar;

                Vector2 barPos = new Vector2(this.WholeInventory.Position.X + 181 + (48 * i), this.WholeInventory.Position.Y + 24);
                invBar = new Sprite(this.textureMap, barPos);
                invBar.Rectangle = new Rectangle(0, 297, 48, 48);
                invBar.Scale = this.scale;

                InventoryCell cell = new InventoryCell(invBar);
                cell.CellID = i;

                this.InventoryItemCells.Add(cell);
            }

            for (int j = 0; j < 3; j++) {

                for (int i = 0; i < 9; i++) {

                    Sprite invBar;

                    Vector2 barPos = new Vector2(this.WholeInventory.Position.X + 181 + (48 * i), this.WholeInventory.Position.Y + 91 + (48 * j));

                    invBar = new Sprite(this.textureMap, barPos);
                    invBar.Rectangle = new Rectangle(0, 297, 48, 48);
                    invBar.Scale = this.scale;

                    InventoryCell cell = new InventoryCell(invBar);
                    cell.CellID = i + (9 * (j + 1));

                    this.InventoryItemCells.Add(cell);
                }
            }

            Vector2 playerPanelPos = new Vector2(this.WholeInventory.Position.X + 75, this.WholeInventory.Position.Y + 40);
            this.PlayerPanel = new Sprite(this.textureMap, playerPanelPos);
            this.PlayerPanel.Rectangle = new Rectangle(218, 256, 103, 179);
            this.PlayerPanel.Scale = this.scale;

            for (int i = 0; i < 4; i++) {

                Sprite invBar;

                Vector2 barPos = new Vector2(this.WholeInventory.Position.X + 24, this.WholeInventory.Position.Y + 24 + (54 * i));

                invBar = new Sprite(this.textureMap, barPos);
                invBar.Rectangle = new Rectangle(0, 297, 48, 48);
                invBar.Scale = this.scale;

                InventoryCell cell = new InventoryCell(invBar);
                cell.CellID = i;

                this.InventoryAccessoryCells.Add(cell);
            }

            Vector2 playerImagePos = new Vector2(this.WholeInventory.Position.X + 96, this.WholeInventory.Position.Y + 85);
            this.PlayerImage = new Sprite(this.textureMap, playerImagePos);
            this.PlayerImage.Rectangle = new Rectangle(321, 256, 60, 88);
            this.PlayerImage.Scale = this.scale;

            if (this.invetoryOpened == false) {

                for (int i = 0; i < 9; i++) {

                    this.InventoryItemCells[i].Sprite.Position = new Vector2(this.MainInventory.Position.X + 18 + (48 * i), this.MainInventory.Position.Y + 15);
                }
            } else if (this.invetoryOpened == true) {

                for (int i = 0; i < 9; i++) {

                    this.InventoryItemCells[i].Sprite.Position = new Vector2(this.WholeInventory.Position.X + 181 + (48 * i), this.WholeInventory.Position.Y + 24);
                }
            }

            this.Indicator = new Sprite(this.textureMap, new Vector2(this.InventoryItemCells.First().GetRectangle().X + 24, this.InventoryItemCells.First().GetRectangle().Y + 24));
            this.Indicator.Origin = new Vector2(24, 24);
            this.Indicator.Rectangle = new Rectangle(48, 297, 48, 48);
            this.Indicator.Scale = 1.125f;

            this.backButton = new Button(new Vector2(this.WholeInventory.Rectangle.Width * 1.5f - 24, this.WholeInventory.Position.Y - 28), 3f, 3f, 1.5f);
            this.backButton.AddText("X", new Color(255, 65, 65));

            this.InventoryTab = new Sprite(this.textureMap, new Vector2(this.WholeInventory.Position.X + 70, this.WholeInventory.Position.Y - 41));
            this.InventoryTab.Rectangle = new Rectangle(0, 516, 218, 44);

            this.OptionsTab = new Sprite(this.textureMap, new Vector2(this.WholeInventory.Position.X + this.textureMap.Width - 218 - 70, this.WholeInventory.Position.Y - 41));
            this.OptionsTab.Rectangle = new Rectangle(218, 516, 218, 44);

            this.exitGameButton = new Button(new Vector2(this.WholeInventory.Rectangle.Width, this.WholeInventory.Rectangle.Height + 96), 8f, 6f, 1.25f);
            this.exitGameButton.AddText("Exit", new Color(255, 65, 65));

            this.NullItem.Name = "";
            this.NullItem.Description = "";
            this.NullItem.Type = "";
            this.NullItem.isStackable = false;
        }

        public int GetEmptyCell() {

            int emptyCell = 0;

            return emptyCell;
        }

        public void MoveIndicator() {

            this.Indicator.Position.X = (this.InventoryItemCells.First().GetRectangle().X + (48 * this.currentCell)) + 24;
        }

        public void MoveIndicator(short cell) {

            this.currentCell += cell;
            this.currentCell = this.currentCell < 0 ? this.currentCell = 8 : this.currentCell > 8 ? this.currentCell = 0 : this.currentCell;

            this.Indicator.Position.X = (this.InventoryItemCells.First().GetRectangle().X + (48 * this.currentCell)) + 24;

            System.Diagnostics.Debug.WriteLine(this.currentCell);

            SoundEffectAudio.SoundEffectInstances["Swap"].Stop();

            if (SoundEffectAudio.SoundEffectInstances["Swap"].State == SoundState.Stopped) SoundEffectAudio.SoundEffectInstances["Swap"].Play();
        }

        private void InventoryIndicatorMovement() {

            if (this.invetoryOpened == false) {

                if (Input.currentScroll > Input.prevScroll) {

                    this.MoveIndicator(-1);

                } else if (Input.currentScroll < Input.prevScroll) {

                    this.MoveIndicator(1);

                } else {

                    foreach (InventoryCell i in this.InventoryItemCells) {

                        if (i.CellID < 9) {

                            for (sbyte j = 1; j <= 9; j++) {

                                Microsoft.Xna.Framework.Input.Keys pressedKey = (Microsoft.Xna.Framework.Input.Keys)((int)Microsoft.Xna.Framework.Input.Keys.D0 + j);

                                if (Input.IsKeyDown(pressedKey)) {

                                    this.currentCell = (short)(j - 1);
                                    this.MoveIndicator();

                                    SoundEffectAudio.SoundEffectInstances["Swap"].Stop();

                                    if (SoundEffectAudio.SoundEffectInstances["Swap"].State == SoundState.Stopped) SoundEffectAudio.SoundEffectInstances["Swap"].Play();

                                    break;
                                }
                            }

                            if (Input.getMouseRectangle().Intersects(i.GetRectangle())) {

                                if (Input.IsLeftClickDown()) {

                                    this.currentCell = (sbyte)i.CellID;
                                    this.MoveIndicator();

                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Pitch = 0.2f;
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Play();

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ToggleInventory(GameTime dt) {

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E)) {

                this.invetoryOpened = !this.invetoryOpened;

                if (this.invetoryOpened == true) {

                    SoundEffectAudio.SoundEffectInstances["CloseMenu"].Stop();
                    SoundEffectAudio.SoundEffectInstances["OpenMenu"].Play();

                } else if (this.invetoryOpened == false) {

                    SoundEffectAudio.SoundEffectInstances["OpenMenu"].Stop();
                    SoundEffectAudio.SoundEffectInstances["CloseMenu"].Play();
                }
            }

            if (this.invetoryOpened == true) {

                this.backButton.Update(dt);

                if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) || this.backButton.doAction == true) {

                    this.invetoryOpened = false;

                    SoundEffectAudio.SoundEffectInstances["OpenMenu"].Stop();
                    SoundEffectAudio.SoundEffectInstances["CloseMenu"].Play();
                }

                if (this.currentTab == 2) {

                    this.exitGameButton.Update(dt);

                    if (this.exitGameButton.doAction == true) {

                        Main.exitGame = true;
                    }
                }
            }
        }

        public void UpdateInventory(GameTime dt) {

            this.ToggleInventory(dt);
            this.InventoryActions();
            this.UpdatePickedUpItem();
            
            if (this.invetoryOpened == false) {

                for (int i = 0; i < 9; i++) {

                    this.InventoryItemCells[i].Sprite.Position = new Vector2(this.MainInventory.Position.X + 18 + (48 * i), this.MainInventory.Position.Y + 15);
                }

            } else if (this.invetoryOpened == true && this.currentTab == 1) {

                for (int i = 0; i < 9; i++) {

                    this.InventoryItemCells[i].Sprite.Position = new Vector2(this.WholeInventory.Position.X + 181 + (48 * i), this.WholeInventory.Position.Y + 24);
                }

                this.InventoryTab.Rectangle = new Rectangle(0, 516, 218, 44);
                this.OptionsTab.Rectangle = new Rectangle(218, 516, 218, 44);

            } else if (this.invetoryOpened == true && this.currentTab == 2) {

                this.InventoryTab.Rectangle = new Rectangle(0, 560, 218, 44);
                this.OptionsTab.Rectangle = new Rectangle(218, 560, 218, 44);
            }
        }

        public Item GetCurrentItem() {

            if (this.invetoryOpened == false) {

                if (this.InventoryItemCells[this.currentCell].hasItem == true && this.InventoryItemCells[this.currentCell] != null) {

                    return this.InventoryItems[this.currentCell].Item;
                }

                return this.NullItem;
            }

            return this.NullItem;
        }

        private void UpdatePickedUpItem() {

            if (this.isHoldingItem == true) {

                this.pickedUpItem.Sprite.Position = new Vector2(Input.getMousePosition().X + 24, Input.getMousePosition().Y + 24);
            }
        }

        private void InventoryActions() {

            if (this.invetoryOpened == false) {

                this.InventoryIndicatorMovement();

                foreach (InventoryCell i in this.InventoryItemCells) {

                    if (i.CellID < 9) {

                        if (Input.getMouseRectangle().Intersects(i.GetRectangle())) {

                            i.Sprite.Rectangle = i.highlightRectangle;

                            if (i.hasItem == true) {

                                i.Item.Sprite.Scale += 0.15f;
                                i.Item.Sprite.Scale = MathHelper.Clamp(i.Item.Sprite.Scale, 3f, 3.75f);

                                i.Item.displayItemDescription = true;

                                Vector2 itemDescPos = new Vector2(Input.getMouseX() + 24, Input.getMouseY() + 24);
                                itemDescPos.Y = MathHelper.Clamp(itemDescPos.Y, 0, Main.screenDimensions[Main.currentScreenSize, 1] - Main.defaultFont.MeasureString(i.Item.Name).Y - Main.defaultFont.MeasureString(i.Item.Description).Y - 64);

                                this.itemDesc = new ItemDescription(itemDescPos, i.Item.Name, i.Item.Type, i.Item.Description);
                            }

                            if (i.soundPlayed == false) {

                                SoundEffectAudio.SoundEffectInstances["Cell"].Stop();
                                SoundEffectAudio.SoundEffectInstances["Cell"].Play();
                            }
                            i.soundPlayed = true;

                        } else {

                            i.Sprite.Rectangle = i.defaultRectangle;

                            if (i.hasItem == true) {

                                
                                i.Item.Sprite.Scale -= 0.15f;
                                i.Item.Sprite.Scale = MathHelper.Clamp(i.Item.Sprite.Scale, 3f, 3.75f);
                                i.Item.displayItemDescription = false;
                            }

                            i.soundPlayed = false;
                        }

                    } else {

                        break;
                    }
                }

            } else if (this.invetoryOpened == true && this.currentTab == 1) {

                foreach (InventoryCell i in this.InventoryItemCells) {

                    if (Input.getMouseRectangle().Intersects(i.GetRectangle())) {

                        i.Sprite.Rectangle = i.highlightRectangle;

                        if (i.hasItem == true && i.Item != null) {

                            i.Item.Sprite.Scale += 0.125f;
                            i.Item.Sprite.Scale = MathHelper.Clamp(i.Item.Sprite.Scale, 3f, 3.5f);

                            i.Item.displayItemDescription = true;

                            this.itemDesc = new ItemDescription(new Vector2(Input.getMouseX() + 24, Input.getMouseY() + 24), i.Item.Name, i.Item.Type, i.Item.Description);

                            if (Input.IsLeftClickDown() == true) {

                                if (this.isHoldingItem == true && i.Item.Name == this.pickedUpItem.Name && i.itemCount < i.maxItemCount) {

                                    i.itemCount += this.pickedUpItemCount;

                                    //this.pickedUpItem = this.NullItem;
                                    this.isHoldingItem = false;
                                    this.pickedUpItemCount = 0;

                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Pitch = 0.2f;
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Play();

                                    break;

                                } else if (this.isHoldingItem == true && i.hasItem == true) {

                                    Item temp = new Item();
                                    temp.ID = this.pickedUpItem.ID;
                                    temp.Name = this.pickedUpItem.Name;
                                    temp.Description = this.pickedUpItem.Description;
                                    temp.Sprite = this.pickedUpItem.Sprite;
                                    temp.CanBreak = this.pickedUpItem.CanBreak;
                                    temp.isStackable = this.pickedUpItem.isStackable;
                                    temp.Type = this.pickedUpItem.Type;
                                    int tempItemCount = this.pickedUpItemCount;

                                    this.pickedUpItem = i.Item;
                                    this.pickedUpItemCount = i.itemCount;
                                    i.Item = temp;
                                    i.itemCount = tempItemCount;

                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Pitch = 0.2f;
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Play();

                                    break;

                                } else {

                                    this.isHoldingItem = true;
                                    this.pickedUpItem = i.Item;
                                    this.pickedUpItemCount = i.itemCount;

                                    i.itemCount = 0;
                                    i.Item = this.NullItem;
                                    i.hasItem = false;
                                    this.InventoryItems[i.CellID].Item = this.NullItem;

                                    this.availableCells++;

                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Pitch = -0.2f;
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Play();

                                    break;
                                }

                            }
                            
                            if (Input.IsRightClickDown()) {

                                if (this.isHoldingItem == false && i.itemCount > 1) {

                                    i.itemCount -= 1;

                                    Item temp = new Item();
                                    temp.ID = i.Item.ID;
                                    temp.Name = i.Item.Name;
                                    temp.Description = i.Item.Description;
                                    temp.Sprite = i.Item.Sprite;
                                    temp.CanBreak = i.Item.CanBreak;
                                    temp.isStackable = i.Item.isStackable;
                                    temp.Type = i.Item.Type;

                                    this.isHoldingItem = true;
                                    this.pickedUpItem = temp;
                                    this.isHoldingItem = true;
                                    this.pickedUpItemCount = 1;

                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Stop();
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Pitch = 0.2f;
                                    SoundEffectAudio.SoundEffectInstances["StoneStep"].Play();

                                    break;

                                }
                            }
                        }

                        else if (i.hasItem == false && this.isHoldingItem) {

                            if (Input.IsLeftClickDown()) {

                                i.Item = this.pickedUpItem;
                                i.hasItem = true;
                                i.itemCount = this.pickedUpItemCount;

                                //this.pickedUpItem = this.NullItem;
                                this.isHoldingItem = false;
                                this.pickedUpItemCount = 0;

                                this.availableCells--;

                                if (this.InventoryItems.ContainsKey(i.CellID)) {

                                    this.InventoryItems[i.CellID].Item = this.pickedUpItem;

                                } else {

                                    this.InventoryItems.Add(i.CellID, i);
                                    this.InventoryItems[i.CellID].Item = this.pickedUpItem;
                                }

                                SoundEffectAudio.SoundEffectInstances["StoneStep"].Stop();
                                SoundEffectAudio.SoundEffectInstances["StoneStep"].Pitch = 0.2f;
                                SoundEffectAudio.SoundEffectInstances["StoneStep"].Play();

                                break;
                            }
                        }

                        if (i.soundPlayed == false) {

                            SoundEffectAudio.SoundEffectInstances["Cell"].Stop();
                            SoundEffectAudio.SoundEffectInstances["Cell"].Play();
                        }
                        i.soundPlayed = true;

                    } else {

                        i.Sprite.Rectangle = i.defaultRectangle;
                        i.Item.displayItemDescription = false;

                        if (i.hasItem == true && i.Item.Sprite != null) {

                            i.Item.Sprite.Scale -= 0.125f;
                            i.Item.Sprite.Scale = MathHelper.Clamp(i.Item.Sprite.Scale, 3f, 3.5f);
                        }

                        i.soundPlayed = false;
                    }
                }

                foreach (InventoryCell i in this.InventoryAccessoryCells) {

                    if (Input.getMouseRectangle().Intersects(i.GetRectangle())) {

                        i.Sprite.Rectangle = i.highlightRectangle;

                        if (i.soundPlayed == false) {

                            SoundEffectAudio.SoundEffectInstances["Cell"].Stop();
                            SoundEffectAudio.SoundEffectInstances["Cell"].Play();
                        }
                        i.soundPlayed = true;

                    } else {

                        i.Sprite.Rectangle = i.defaultRectangle;

                        i.soundPlayed = false;
                    }
                }

            }
            
            if (this.invetoryOpened == true) {

                if (this.currentTab == 1 && Input.getMouseRectangle().Intersects(this.getOptionsTabRectangle())) {

                    if (Input.IsLeftClickDown() == true) {

                        this.currentTab = 2;
                        SoundEffectAudio.SoundEffectInstances["ChangeTab"].Stop();
                        SoundEffectAudio.SoundEffectInstances["ChangeTab"].Play();
                    }

                } else if (this.currentTab == 2 && Input.getMouseRectangle().Intersects(this.getInventoryTabRectangle())) {

                    if (Input.IsLeftClickDown() == true) {

                        this.currentTab = 1;
                        SoundEffectAudio.SoundEffectInstances["ChangeTab"].Stop();
                        SoundEffectAudio.SoundEffectInstances["ChangeTab"].Play();
                    }
                }
            }
        }

        public void DrawMainInventory(SpriteBatch b) {

            b.Draw(this.MainInventory.Texture, this.MainInventory.Position, this.MainInventory.Rectangle, this.MainInventory.Hue,
                    this.MainInventory.Rotation, this.MainInventory.Origin, this.MainInventory.Scale, this.MainInventory.Effect, this.MainInventory.Depth);

            foreach (InventoryCell i in this.InventoryItemCells) {

                if (i.CellID < 9) {

                    b.Draw(i.Sprite.Texture, i.Sprite.Position, i.Sprite.Rectangle, i.Sprite.Hue, i.Sprite.Rotation, i.Sprite.Origin, i.Sprite.Scale, i.Sprite.Effect, i.Sprite.Depth);

                    if (i.hasItem == true) b.Draw(i.Item.Sprite.Texture, new Vector2(i.Sprite.Position.X + 24, i.Sprite.Position.Y + 24), i.Item.Sprite.Rectangle, i.Item.Sprite.Hue, i.Item.Sprite.Rotation, i.Item.Sprite.Origin, i.Item.Sprite.Scale, i.Item.Sprite.Effect, i.Item.Sprite.Depth);
                }
            }

            b.Draw(this.Indicator.Texture, this.Indicator.Position, this.Indicator.Rectangle, this.Indicator.Hue,
                    this.Indicator.Rotation, this.Indicator.Origin, this.Indicator.Scale, this.Indicator.Effect, this.Indicator.Depth);
        }

        public void DrawWholeInventory(SpriteBatch b) {

            b.Draw(this.WholeInventory.Texture, this.WholeInventory.Position, this.WholeInventory.Rectangle, this.WholeInventory.Hue,
                    this.WholeInventory.Rotation, this.WholeInventory.Origin, this.WholeInventory.Scale, this.WholeInventory.Effect, this.WholeInventory.Depth);

            b.Draw(this.InventoryTab.Texture, this.InventoryTab.Position, this.InventoryTab.Rectangle, this.InventoryTab.Hue,
                    this.InventoryTab.Rotation, this.InventoryTab.Origin, this.InventoryTab.Scale, this.InventoryTab.Effect, this.InventoryTab.Depth);

            b.Draw(this.OptionsTab.Texture, this.OptionsTab.Position, this.OptionsTab.Rectangle, this.OptionsTab.Hue,
                    this.OptionsTab.Rotation, this.OptionsTab.Origin, this.OptionsTab.Scale, this.OptionsTab.Effect, this.OptionsTab.Depth);

            if (this.currentTab == 1) {

                foreach (InventoryCell i in this.InventoryAccessoryCells) {

                    b.Draw(i.Sprite.Texture, i.Sprite.Position, i.Sprite.Rectangle, i.Sprite.Hue, i.Sprite.Rotation, i.Sprite.Origin, i.Sprite.Scale, i.Sprite.Effect, i.Sprite.Depth);
                }

                foreach (InventoryCell i in this.InventoryItemCells) {

                    b.Draw(i.Sprite.Texture, i.Sprite.Position, i.Sprite.Rectangle, i.Sprite.Hue, i.Sprite.Rotation, i.Sprite.Origin, i.Sprite.Scale, i.Sprite.Effect, i.Sprite.Depth);

                    if (i.hasItem == true && i != null && i.Item != null && i.Item.Sprite != null) {

                        b.Draw(i.Item.Sprite.Texture, new Vector2(i.Sprite.Position.X + 24, i.Sprite.Position.Y + 24), i.Item.Sprite.Rectangle, i.Item.Sprite.Hue, i.Item.Sprite.Rotation, i.Item.Sprite.Origin, i.Item.Sprite.Scale, i.Item.Sprite.Effect, i.Item.Sprite.Depth);
                    }
                }

                b.Draw(this.PlayerPanel.Texture, this.PlayerPanel.Position, this.PlayerPanel.Rectangle, this.PlayerPanel.Hue,
                        this.PlayerPanel.Rotation, this.PlayerPanel.Origin, this.PlayerPanel.Scale, this.PlayerPanel.Effect, this.PlayerPanel.Depth);

                b.Draw(this.PlayerImage.Texture, this.PlayerImage.Position, this.PlayerImage.Rectangle, this.PlayerImage.Hue,
                        this.PlayerImage.Rotation, this.PlayerImage.Origin, this.PlayerImage.Scale, this.PlayerImage.Effect, this.PlayerImage.Depth);

            } else if (this.currentTab == 2) {

                this.exitGameButton.Draw(b);
            }

            this.backButton.Draw(b);
        }

        public void DrawItemCountWholeInventory(SpriteBatch b) {

            if (this.currentTab == 1) {

                foreach (InventoryCell i in this.InventoryItemCells) {

                    if (i.hasItem == true && i != null) {

                        i.DrawItemCount(b);
                    }
                }
            }
        }

        public void DrawItemCountMainInventory(SpriteBatch b) {

            foreach(InventoryCell i in this.InventoryItemCells) {

                if (i.CellID < 9) {

                    if (i.hasItem == true && i != null) {

                        i.DrawItemCount(b);
                    }
                }
            }
        }

        public void DrawWholeInventoryItemDescriptions(SpriteBatch b) {

            foreach (InventoryCell i in this.InventoryItemCells) {

                if (i.hasItem == true) {

                    if (i.Item.displayItemDescription == true) this.itemDesc.Draw(b);
                }
            }
        }

        public void DrawMainInventoryItemDescriptions(SpriteBatch b) {

            foreach (InventoryCell i in this.InventoryItemCells) {

                if (this.invetoryOpened == false && i.CellID < 9) {

                    if (i.hasItem == true) {

                        if (i.Item.displayItemDescription == true) this.itemDesc.Draw(b);
                    }
                }
            }
        }

        public void DrawPickedUpItem(SpriteBatch b) {

            if (this.isHoldingItem == true) {

                b.Draw(this.pickedUpItem.Sprite.Texture, this.pickedUpItem.Sprite.Position, this.pickedUpItem.Sprite.Rectangle, this.pickedUpItem.Sprite.Hue,
                        this.pickedUpItem.Sprite.Rotation, this.pickedUpItem.Sprite.Origin, this.pickedUpItem.Sprite.Scale, this.pickedUpItem.Sprite.Effect, this.pickedUpItem.Sprite.Depth);

                if (this.pickedUpItemCount > 1) {

                    Helper.DrawTextOutline(b, Main.defaultFont, "" + this.pickedUpItemCount, new Vector2(Input.getMouseX() + 36, Input.getMouseY() + 36), 2.5f, Color.Black, 0.75f, Vector2.Zero);
                    b.DrawString(Main.defaultFont, "" + this.pickedUpItemCount, new Vector2(Input.getMouseX() + 36, Input.getMouseY() + 36), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                }
            }
        }

        public Rectangle getInventoryTabRectangle() => new Rectangle((int)this.InventoryTab.Position.X, (int)this.InventoryTab.Position.Y, this.InventoryTab.Rectangle.Width, this.InventoryTab.Rectangle.Height);

        public Rectangle getOptionsTabRectangle() => new Rectangle((int)this.OptionsTab.Position.X, (int)this.OptionsTab.Position.Y, this.OptionsTab.Rectangle.Width, this.OptionsTab.Rectangle.Height);

    }
}