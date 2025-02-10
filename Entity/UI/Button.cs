using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;
using MonoFarming.Audio;

namespace MonoFarming.Entity.UI {
    public class Button {

        private Texture2D buttonTexture = Main.contentManager.Load<Texture2D>("Sprites\\Buttons");
        public Sprite ButtonSprite;
        public float ButtonWidth;
        public float ButtonHeight;
        public float scale = 3f;
        private float minWidth;
        private float minHeight;
        private float maxWidth;
        private float maxHeight;

        public string Text = "";
        public float textSize;
        private float minTextSize;
        private float maxTextSize;
        public Vector2 textPosition;
        public Color textColor = Color.White;
        public bool isClicked = false;
        public bool isReleased = false;
        public bool doAction = false;
        private bool soundPlayed = false;

        public Button(Vector2 position, float buttonWidth, float buttonHeight, float textSize) {

            this.ButtonWidth = buttonWidth;
            this.ButtonHeight = buttonHeight;
            this.minWidth = buttonWidth;
            this.minHeight = buttonHeight;
            this.maxWidth = buttonWidth + 0.5f;
            this.maxHeight = buttonHeight + 0.5f;

            this.ButtonSprite = new Sprite(this.buttonTexture, position);
            this.ButtonSprite.Rectangle = new Rectangle(0, 0, 16, 16);
            this.ButtonSprite.Scale = this.scale;
            this.ButtonSprite.Origin = new Vector2(this.ButtonSprite.Rectangle.Width / 2, this.ButtonSprite.Rectangle.Height / 2);

            this.textPosition = new Vector2(this.ButtonSprite.Position.X + 1, this.ButtonSprite.Position.Y);
            this.textSize = textSize;
            this.minTextSize = textSize;
            this.maxTextSize = textSize + (this.minTextSize / 3f);
        }

        public void AddText(string text, Color color) {

            this.Text = text;
            this.textColor = color;
        }

        private void OnClick() {

            this.ButtonSprite.Rectangle = new Rectangle(16, 0, 16, 16);

            this.textPosition.Y += this.ButtonSprite.Scale * 1.25f;

            this.isClicked = true;
        }

        private void AfterClick() {

            this.ButtonSprite.Rectangle = new Rectangle(0, 0, 16, 16);

            this.isReleased = true;
            this.isClicked = false;
            this.textPosition.Y -= this.ButtonSprite.Scale * 1.25f;
        }

        private bool IsHovered() {

            if (Input.getMouseRectangle().Intersects(new Rectangle((int)(this.ButtonSprite.Position.X - (this.ButtonWidth * this.ButtonSprite.Origin.X)), (int)(this.ButtonSprite.Position.Y - (this.ButtonWidth * this.ButtonSprite.Origin.Y)),
                                                        this.ButtonSprite.Rectangle.Width * (int)this.ButtonWidth, this.ButtonSprite.Rectangle.Height * (int)this.ButtonWidth))) {

                return true;
            }

            return false;
        }

        private void ScaleButton(bool isHovering) {

            if (isHovering) {

                this.ButtonWidth += 0.1f;
                this.ButtonHeight += 0.1f;
                this.textSize += 0.1f;

            } else {

                this.ButtonWidth -= 0.1f;
                this.ButtonHeight -= 0.1f;
                this.textSize -= 0.1f;
            }

            //this.textSize = this.ButtonSprite.Scale / 2;
            this.textSize = MathHelper.Clamp(this.textSize, this.minTextSize, this.maxTextSize);
            this.ButtonWidth = MathHelper.Clamp(this.ButtonWidth, this.minWidth, this.maxWidth);
            this.ButtonHeight = MathHelper.Clamp(this.ButtonHeight, this.minHeight, this.maxHeight);
            this.ButtonSprite.Scale = MathHelper.Clamp(this.ButtonSprite.Scale, this.ButtonWidth, this.ButtonWidth + 0.5f);
        }

        public void Update(GameTime dt) {

            this.isReleased = false;
            this.doAction = false;

            if (this.isClicked == true && Input.IsLeftClickReleased()) this.AfterClick();

            if (this.IsHovered() == true && Input.IsLeftClickDown() == true) {

                this.OnClick();
            }

            if (this.IsHovered() == true) {

                this.ScaleButton(true);
                if (this.soundPlayed == false) SoundEffectAudio.SoundEffectInstances["Hover"].Play();
                this.soundPlayed = true;

            } else {
             
                this.ScaleButton(false);
                this.soundPlayed = false;
            }

            if (this.isReleased == true) this.doAction = true;
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.ButtonSprite.Texture, this.ButtonSprite.Position, this.ButtonSprite.Rectangle, this.ButtonSprite.Hue,
                    this.ButtonSprite.Rotation, this.ButtonSprite.Origin, new Vector2(this.ButtonWidth, this.ButtonHeight), this.ButtonSprite.Effect, this.ButtonSprite.Depth);

            if (!this.Text.Equals("")) {

                Vector2 textSizeInPixels = Main.defaultFont.MeasureString(this.Text);

                Helper.DrawTextOutline(b, Main.defaultFont, this.Text, this.textPosition, 3, new Color(65, 0, 0), this.textSize, textSizeInPixels / 2);
                b.DrawString(Main.defaultFont, this.Text, this.textPosition, this.textColor, 0f, new Vector2(textSizeInPixels.X / 2, textSizeInPixels.Y / 2), this.textSize, SpriteEffects.None, 0f);
            }
        }
    }
}