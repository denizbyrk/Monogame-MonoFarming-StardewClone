using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Entity.UI {
    public class Pickup {

        private Texture2D PickUpTexture;
        private Sprite BoxSprite;
        private Sprite TextBoxSprite;
        private Sprite TextBoxSprite2;
        private Sprite Item;
        private string ItemName;
        private float scale = 1.5f;
        private float xScale;
        private float alpha = 1f;
        private float timer;

        public Pickup(Sprite item, string itemName) {

            this.Item = item;
            this.ItemName = itemName;

            this.PickUpTexture = Main.contentManager.Load<Texture2D>("Sprites\\PickedUpItemBox");

            this.BoxSprite = new Sprite(this.PickUpTexture, new Vector2(24, Main.screenDimensions[Main.currentScreenSize, 1] * 0.875f));
            this.BoxSprite.Rectangle = new Rectangle(0, 0, 50, 48);
            this.BoxSprite.Scale = this.scale;

            this.Item.Position = new Vector2(this.BoxSprite.Position.X + 34, this.BoxSprite.Position.Y + 36);
            this.Item.Scale = this.scale * 2f;

            Vector2 textSize = Main.defaultFont.MeasureString(this.ItemName);

            this.TextBoxSprite = new Sprite(this.PickUpTexture, new Vector2(this.BoxSprite.Position.X + (this.BoxSprite.Rectangle.Width * this.scale), this.BoxSprite.Position.Y));
            this.TextBoxSprite.Rectangle = new Rectangle(50, 0, 2, 48);
            this.TextBoxSprite.Scale = this.scale;
            this.xScale = textSize.X / 2 + 4;

            this.TextBoxSprite2 = new Sprite(this.PickUpTexture, new Vector2(this.BoxSprite.Position.X + ((this.xScale * 2) - 3) + ((this.TextBoxSprite.Rectangle.Width + this.TextBoxSprite.Rectangle.X) * this.scale), this.BoxSprite.Position.Y));
            this.TextBoxSprite2.Rectangle = new Rectangle(52, 0, 12, 48);
            this.TextBoxSprite2.Scale = this.scale;
        }

        public void FadeOut(GameTime dt, int count) {

            if (Hud.pickupBars.Count > 0) {

                this.timer += (float)dt.ElapsedGameTime.TotalSeconds;

                if (this.timer > 3f) {

                    this.alpha -= 0.02f;

                    if (this.alpha <= 0.1f) {

                        Hud.pickupBars.Remove(this);
                    }
                }
            }

            this.BoxSprite.Position.Y = Main.screenDimensions[Main.currentScreenSize, 1] * 0.875f - (count * 80);
            this.Item.Position.Y = this.BoxSprite.Position.Y + 36;
            this.TextBoxSprite.Position.Y = this.BoxSprite.Position.Y;
            this.TextBoxSprite2.Position.Y = this.BoxSprite.Position.Y;
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.BoxSprite.Texture, this.BoxSprite.Position, this.BoxSprite.Rectangle, this.BoxSprite.Hue * this.alpha,
                    this.BoxSprite.Rotation, this.BoxSprite.Origin, this.BoxSprite.Scale, this.BoxSprite.Effect, this.BoxSprite.Depth);

            b.Draw(this.TextBoxSprite.Texture, this.TextBoxSprite.Position, this.TextBoxSprite.Rectangle, this.TextBoxSprite.Hue * this.alpha,
                    this.TextBoxSprite.Rotation, this.TextBoxSprite.Origin, new Vector2(this.xScale, this.TextBoxSprite.Scale), this.TextBoxSprite.Effect, this.TextBoxSprite.Depth);

            b.Draw(this.TextBoxSprite2.Texture, this.TextBoxSprite2.Position, this.TextBoxSprite2.Rectangle, this.TextBoxSprite2.Hue * this.alpha,
                    this.TextBoxSprite2.Rotation, this.TextBoxSprite2.Origin, this.TextBoxSprite2.Scale, this.TextBoxSprite2.Effect, this.TextBoxSprite2.Depth);

            b.Draw(this.Item.Texture, this.Item.Position, this.Item.Rectangle, this.Item.Hue * this.alpha,
                    this.Item.Rotation, this.Item.Origin, this.Item.Scale, this.Item.Effect, this.Item.Depth);

            Helper.DrawTextOutline(b, Main.defaultFont, "" + this.ItemName, new Vector2(this.TextBoxSprite.Position.X + 4, this.TextBoxSprite.Position.Y + 27), 2.25f, Color.Black * this.alpha, 1f, Vector2.Zero);
            b.DrawString(Main.defaultFont, "" + this.ItemName, new Vector2(this.TextBoxSprite.Position.X + 4, this.TextBoxSprite.Position.Y + 27), Color.White * this.alpha);
        }
    }
}