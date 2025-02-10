using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Scene.Scenes {
    public class Splash : SceneManager {

        private Sprite Logo;
        private Rectangle BlackScreen;
        private float screenOpacity = 1f;

        public Splash() {

            var LogoTexture = Main.contentManager.Load<Texture2D>("Sprites\\Logo");
            Vector2 logoPosition = new Vector2(0, 0);

            this.Logo = new Sprite(LogoTexture, logoPosition);
            this.Logo.Origin = new Vector2(this.Logo.Texture.Width / 2, this.Logo.Texture.Height / 2);

            this.Logo.Position = new Vector2(Main.screenDimensions[Main.currentScreenSize, 0] / 2 - LogoTexture.Width / 2 + this.Logo.Origin.X,
                                               Main.screenDimensions[Main.currentScreenSize, 1] / 2 - (LogoTexture.Height) / 2 + this.Logo.Origin.Y);

            this.BlackScreen = new Rectangle(0, 0, Main.screenDimensions[Main.currentScreenSize, 0], Main.screenDimensions[Main.currentScreenSize, 1]);
        }

        public override void LoadContent() {

        }

        public override void UnloadContent() {

            this.Logo.Texture.Dispose();
        }

        public override void Update(GameTime dt) {

            if (this.Logo.Scale > 1.5f && this.screenOpacity == 1) {

                this.UnloadContent();

                Main.currentGameScene = Main.Scenes.Overworld;
                Main.currentScene = new Overworld();

                Main.LoadSceneContent(Main.currentScene);

            } else if (this.Logo.Scale > 1.5f) {

                this.screenOpacity += 0.015f;

            } else {

                this.screenOpacity -= 0.005f;
            }

            this.Logo.Scale += 0.0025f;
            this.screenOpacity = MathHelper.Clamp(this.screenOpacity, 0, 1);
        }

        public override void Draw(SpriteBatch b, GameTime dt) {

            b.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null);

            b.Draw(this.Logo.Texture, this.Logo.Position, null, this.Logo.Hue,
                    this.Logo.Rotation, this.Logo.Origin, this.Logo.Scale, this.Logo.Effect, this.Logo.Depth);

            b.Draw(Main.pixel, new Vector2(this.BlackScreen.X, this.BlackScreen.Y), this.BlackScreen, Color.Black * this.screenOpacity, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            b.End();
        }
    }
}
