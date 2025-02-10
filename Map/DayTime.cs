using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoFarming.Scene.Scenes;
using MonoFarming.Util;

namespace MonoFarming.Map {
    public class DayTime {

        public int mins = 0;
        public int hours = 10;
        public int day = 1;
        public string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
        public string displayedMins;
        public string displayedHours;
        private float speed = 0.75f;
        private float hourAlpha = 1f;
        private float timer = 0f;
        private float alphaTimer = 0f;

        public DayTime() {

            this.displayedMins = this.mins.ToString("D2");
            this.displayedHours = this.hours.ToString("D2");
        }

        private void UpdateDayState() {

            if (this.hours == 0 && this.mins == 0) {

                int rainChance = Main.random.Next(2);

                if (rainChance == 0) Overworld.isRaining = true;
                else Overworld.isRaining = false;
            }

            if (this.hours >= 5 && this.hours <= 9) {

                Overworld.currentDayState = Overworld.DayState.Morning;
                Overworld.isNight = true;

            } else if (this.hours >= 10 && this.hours <= 18) {

                Overworld.currentDayState = Overworld.DayState.Noon;
                Overworld.isNight = false;

            } else if (this.hours >= 19 && this.hours <= 22) {

                Overworld.currentDayState = Overworld.DayState.Evening;
                Overworld.isNight = true;

            } else if (this.hours >= 23 || (this.hours >= 0 && this.hours <= 4)) {

                Overworld.currentDayState = Overworld.DayState.Midnight;
                Overworld.isNight = true;
            }
        }

        private void DayNightCycle() {

            if (Overworld.currentDayState == Overworld.DayState.Evening) {

                Overworld.nightColor.R--;
                Overworld.nightColor.G--;
                Overworld.nightAlpha--;

            } else if (Overworld.currentDayState == Overworld.DayState.Morning) {

                Overworld.nightColor.R++;
                Overworld.nightColor.G++;
                Overworld.nightAlpha++;

            } else if (Overworld.currentDayState == Overworld.DayState.Noon) {

                Overworld.nightColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);

            } else if (Overworld.currentDayState == Overworld.DayState.Midnight) {

                Overworld.nightColor = new Color(15, 30, byte.MaxValue);
            }

            Overworld.nightColor.R = (byte)MathHelper.Clamp(Overworld.nightColor.R, 15, byte.MaxValue - 1);
            Overworld.nightColor.G = (byte)MathHelper.Clamp(Overworld.nightColor.G, 30, byte.MaxValue - 1);
            Overworld.nightAlpha = MathHelper.Clamp(Overworld.nightAlpha, 128, byte.MaxValue - 1);
        }

        private void Time() {

            this.mins++;

            if (this.mins == 60) {

                this.mins = 0;
                this.hours++;
            }

            if (this.hours == 24) {

                this.hours = 0;
                this.day++;
            }

            this.UpdateDayState();

            this.DayNightCycle();
        }

        public void Update(GameTime dt) {

            if (Overworld.freezeTime == false) {

                this.hourAlpha = 1f;

                this.timer += (float)dt.ElapsedGameTime.TotalSeconds;

                if (this.timer >= this.speed) {

                    this.timer = 0f;

                    this.Time();

                    this.displayedMins = mins.ToString("D2");
                    this.displayedHours = hours.ToString("D2");
                }

            } else {

                this.alphaTimer += (float)dt.ElapsedGameTime.TotalSeconds;

                if (this.alphaTimer >= 0.5f) {

                    this.alphaTimer = 0f;

                    if (this.hourAlpha == 0.5f) this.hourAlpha = 1f;
                    else if (this.hourAlpha == 1f) this.hourAlpha = 0.5f;
                }
            }

            if (Main.debugMode == true && Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.M)) Overworld.isRaining = !Overworld.isRaining;
            if (Main.debugMode == true && Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N) && this.speed == 0.75f) this.speed = 0.005f;
            else if (Main.debugMode == true && Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.N) && this.speed == 0.005f) this.speed = 0.75f;
        }

        public void Draw(SpriteBatch b, Vector2 pos) {

            Helper.DrawTextOutline(b, Main.defaultFont, this.seasons[0] + " " + this.day, new Vector2(pos.X - 32, pos.Y - 32), 3f, Color.Black * this.hourAlpha, 1.5f, Vector2.Zero);
            Helper.DrawTextOutline(b, Main.defaultFont, this.displayedHours + ":" + this.displayedMins, pos, 3f, Color.Black * this.hourAlpha, 1.5f, Vector2.Zero);

            b.DrawString(Main.defaultFont, this.seasons[0] + " " + this.day, new Vector2(pos.X - 32, pos.Y - 32), Color.White * this.hourAlpha, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            b.DrawString(Main.defaultFont, this.displayedHours + ":" + this.displayedMins, pos, Color.White * this.hourAlpha, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
        }
    }
}