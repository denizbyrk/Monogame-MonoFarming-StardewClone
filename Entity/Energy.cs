using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoFarming.Util;
using MonoFarming.Entity.UI;

namespace MonoFarming.Entity {
    public class Energy {
 
        public List<EnergyBar> energyBar = new List<EnergyBar>();
        public int barCount = 5;
        public Rectangle barRect;
        public Sprite energyBG;

        public static int playerEnergy = 200;
        public int maxEnergy = 200;
        public static bool decreaseEnergy;

        public Energy() {

            for (int i = 0; i < this.barCount; i++) {

                this.energyBar.Add(new EnergyBar(new Vector2(Main.screenDimensions[Main.currentScreenSize, 0] - 66, Main.screenDimensions[Main.currentScreenSize, 1] - (Main.screenDimensions[Main.currentScreenSize, 1] / 2.8f) + (i * 48))));

                this.energyBar[i].energySprite.Rectangle = new Rectangle(0, 0, 16, 16);
                this.energyBar[i].energySprite.Scale = 3f;
            }

            this.barRect = new Rectangle((int)this.energyBar[0].energySprite.Position.X, (int)this.energyBar[0].energySprite.Position.Y, 48, 10 * 48);

            this.energyBG = new Sprite(this.energyBar[0].energyTextureAtlas, new Vector2(this.energyBar[0].energySprite.Position.X - 12, this.energyBar[0].energySprite.Position.Y - 15));
            this.energyBG.Rectangle = new Rectangle(0, 16, 25, 90);
            this.energyBG.Scale = 3f;
        }

        private bool IsHovered() {

            if (Input.getMouseRectangle().Intersects(this.barRect)) {

                return true;
            }

            return false;
        }

        private int shakeTimer = 0;
        public float temp2 = (float)Math.Ceiling((float)Energy.playerEnergy / (200 / (5 * 2)));
        private bool shake = false;
        private bool shakeFinished = false;

        private void ResetPosition() {

            for (int i = 0; i < this.barCount; i++) {

                this.energyBar[i].energySprite.Position.X = Main.screenDimensions[Main.currentScreenSize, 0] - 66;
                this.energyBar[i].energySprite.Position.Y = Main.screenDimensions[Main.currentScreenSize, 1] - (Main.screenDimensions[Main.currentScreenSize, 1] / 2.8f) + (i * 48);
            }

            this.energyBG.Position = new Vector2(this.energyBar[0].energySprite.Position.X - 12, this.energyBar[0].energySprite.Position.Y - 15);
        }

        private void Shake(int duration, int strength) {

            this.shakeFinished = false;

            if (this.shakeTimer >= duration) {

                this.shake = false;
                this.shakeTimer = 0;
                this.shakeFinished = true;
            }

            if (this.shakeTimer < duration && this.shake == true) {

                int randomX = 0, randomY = 0;

                this.energyBG.Position = new Vector2(this.energyBar[0].energySprite.Position.X - 12, this.energyBar[0].energySprite.Position.Y - 15);

                for (int i = 0; i < this.barCount; i++) {

                    this.energyBar[i].energySprite.Position.X = Main.screenDimensions[Main.currentScreenSize, 0] - 66;
                    this.energyBar[i].energySprite.Position.Y = Main.screenDimensions[Main.currentScreenSize, 1] - (Main.screenDimensions[Main.currentScreenSize, 1] / 2.8f) + (i * 48);
                }

                randomX = Main.random.Next(0, 2) == 0 ? randomX = -strength : randomX = strength;
                randomY = Main.random.Next(0, 2) == 0 ? randomY = -strength : randomY = strength;

                this.energyBG.Position.X += randomX;
                this.energyBG.Position.Y += randomY;

                for (int i = 0; i < this.barCount; i++) {

                    this.energyBar[i].energySprite.Position.X += randomX;
                    this.energyBar[i].energySprite.Position.Y += randomY;
                }

                this.shakeTimer++;
            }
        }

        private void UpdateEnergyIcons() {

            float temp = (float)Math.Ceiling((float)Energy.playerEnergy / (this.maxEnergy / (this.barCount * 2)));

            if (Energy.playerEnergy != this.maxEnergy && Energy.playerEnergy % 20 == 0) {

                this.shake = true;
            }

            for (int i = 0; i < this.barCount; i++) {

                if (temp > (this.barCount - 1 - i) * 2 + 1) {

                    this.energyBar[i].energySprite.Rectangle = new Rectangle(0, 0, 16, 16);

                    break;

                } else if (temp > (barCount - 1 - i) * 2) {

                    this.energyBar[i].energySprite.Rectangle = new Rectangle(16, 0, 16, 16);

                    break;

                } else {

                    this.energyBar[i].energySprite.Rectangle = new Rectangle(32, 0, 16, 16);
                }
            }
        }

        public void Update() {

            if (this.shake == true) {

                this.Shake(30, 1);
            } else if (this.shake == false) this.ResetPosition();

            if (Energy.decreaseEnergy == true) {

                Energy.playerEnergy -= 2;
                Energy.decreaseEnergy = false;

                this.UpdateEnergyIcons();
            }

            Energy.playerEnergy = MathHelper.Clamp(Energy.playerEnergy, 0, this.maxEnergy);
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.energyBG.Texture, this.energyBG.Position, this.energyBG.Rectangle, this.energyBG.Hue,
                    this.energyBG.Rotation, this.energyBG.Origin, this.energyBG.Scale, this.energyBG.Effect, this.energyBG.Depth);

            if (Main.debugMode == true) b.Draw(Main.pixel, this.barRect, Color.HotPink);

            foreach(EnergyBar e in this.energyBar) {

                b.Draw(e.energySprite.Texture, e.energySprite.Position, e.energySprite.Rectangle, e.energySprite.Hue,
                        e.energySprite.Rotation, e.energySprite.Origin, e.energySprite.Scale, e.energySprite.Effect, e.energySprite.Depth);
            }

            if (this.IsHovered() == true) {

                Helper.DrawTextOutline(b, Main.defaultFont, "" + Energy.playerEnergy + "/" + this.maxEnergy, new Vector2(this.barRect.X - 80, this.barRect.Y - 48), 2.25f, new Color(120, 80, 0), 1.25f, Vector2.Zero);
                b.DrawString(Main.defaultFont, "" + Energy.playerEnergy + "/" + this.maxEnergy, new Vector2(this.barRect.X - 80, this.barRect.Y - 48), new Color(255, 160, 0), 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0f);
            }
        }
    }
}