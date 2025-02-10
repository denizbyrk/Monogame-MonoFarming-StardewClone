using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoFarming.Util;
using MonoFarming.Scene.Scenes;
using System.Threading;
using MonoFarming.Audio;

namespace MonoFarming.Entity.UI {
    public class Dialog {

        private Texture2D DialogBoxTexture;
        public Sprite DialogSprite;
        public Sprite Icon;
        public List<string> DialogTexts;
        public int DialogCount;
        public int currentDialog = 0;
        public float maxScale = 5f;
        public bool isShowingUp = false;
        public bool isDisposing = false;
        public bool drawDialog;
        private float timer;
        private bool showUpSoundPlayed = false;
        private bool disposeSoundPlayed = false;

        public Dialog() {

            this.DialogBoxTexture = Main.contentManager.Load<Texture2D>("Sprites\\Dialogue Box");

            this.DialogSprite = new Sprite(this.DialogBoxTexture, new Vector2(Main.screenDimensions[Main.currentScreenSize, 0] / 2, Main.screenDimensions[Main.currentScreenSize, 1] - 80));
            this.DialogSprite.Rectangle = new Rectangle(0, 0, 128, 36);
            this.DialogTexts = new List<string>();

            this.Icon = new Sprite(this.DialogBoxTexture, new Vector2(this.DialogSprite.Position.X + 240, this.DialogSprite.Position.Y - 48));
            this.Icon.Rectangle = new Rectangle(0, 36, 16, 16);
            this.Icon.Scale = this.maxScale;
        }

        public void addDialog(string text) {

            this.DialogTexts.Add(text);
            this.DialogCount = this.DialogTexts.Count;
        }

        public void DisplayDialog(SpriteBatch b) {

            if (this.drawDialog == true) {

                Player.freeze = true;
                Overworld.freezeTime = true;
                Overworld.isDialogShown = true;
                
                if (this.DialogSprite.Scale >= this.maxScale) this.isShowingUp = false;
                if (this.currentDialog == this.DialogCount && this.DialogSprite.Scale >= this.maxScale && Input.IsLeftClickDown() == true) this.isDisposing = true;

                if (this.isDisposing == true) {

                    this.Dispose(0.375f);

                } else if (this.DialogSprite.Scale <= this.maxScale && this.isDisposing == false) {

                    this.ShowUp(0.375f);
                }

                b.Draw(this.DialogSprite.Texture, this.DialogSprite.Position, this.DialogSprite.Rectangle, this.DialogSprite.Hue, this.DialogSprite.Rotation,
                        this.DialogSprite.Origin, this.DialogSprite.Scale, this.DialogSprite.Effect, this.DialogSprite.Depth);
            
                if (this.DialogSprite.Scale >= this.maxScale) {

                    if (Input.IsLeftClickDown() == true || Input.IsRightClickDown() == true) this.GoToNextLine();

                    Vector2 textPos = new Vector2(this.DialogSprite.Position.X / 2 + 42, this.DialogSprite.Position.Y - 104);

                    Helper.DrawTextOutline(b, Main.defaultFont, this.DialogTexts[this.currentDialog], textPos, 2f, Color.Black, 1.25f, Vector2.Zero);
                    b.DrawString(Main.defaultFont, this.DialogTexts[this.currentDialog], textPos, Color.White, 0f, Vector2.Zero, 1.25f, SpriteEffects.None, 0f);

                    b.Draw(this.Icon.Texture, this.Icon.Position, this.Icon.Rectangle, this.Icon.Hue, this.Icon.Rotation, this.Icon.Origin, this.Icon.Scale, this.Icon.Effect, this.Icon.Depth);
                }
            }

            if (this.DialogSprite.Scale <= 0f && this.isDisposing == false) this.drawDialog = false;
        }

        public void ShowUp(float speed) {

            this.isShowingUp = true;

            this.DialogSprite.Scale += speed;
            this.DialogSprite.Scale = MathHelper.Clamp(this.DialogSprite.Scale, 1f, this.maxScale);

            if (this.showUpSoundPlayed == false) SoundEffectAudio.SoundEffectInstances["OpenDialog"].Play();
            this.showUpSoundPlayed = true;
        }

        public void Dispose(float speed) {

            this.DialogSprite.Scale -= speed;
            this.DialogSprite.Scale = MathHelper.Clamp(this.DialogSprite.Scale, 0f, this.maxScale);

            if (this.disposeSoundPlayed == false) SoundEffectAudio.SoundEffectInstances["CloseDialog"].Play();
            this.disposeSoundPlayed = true;

            if (this.DialogSprite.Scale <= 0f) {

                Overworld.isDialogShown = false;
                this.isDisposing = false;
                this.currentDialog = -1;

                this.showUpSoundPlayed = false;
                this.disposeSoundPlayed = false;
            }
        }

        public void GoToNextLine() {

            this.currentDialog += 1;

            if (this.currentDialog == this.DialogTexts.Count) this.isDisposing = true;

            this.currentDialog = MathHelper.Clamp(this.currentDialog, 0, this.DialogTexts.Count - 1);

            SoundEffectAudio.SoundEffectInstances["Hover"].Play();
        }

        public void AnimateIcon(GameTime dt) {

            this.timer += (float)dt.ElapsedGameTime.TotalSeconds;

            if (this.timer > 0.1f) {

                this.timer = 0f;

                this.Icon.Rectangle.X += 16;

                if (this.Icon.Rectangle.X > 96) {

                    this.Icon.Rectangle.X = 0;
                }
            }
        }
    }
}