using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Map.Weather {
    public class Raindrop {

        private Texture2D rainDropTexture;
        public Sprite dropSprite;
        private float rainDropSpeed;
        private float rainDropTime;
        private float rainDropOpacity;
        public bool drawParticle = false;
        public bool destroy = false;
        private float timer;
        private float particleTimer;

        public Raindrop() {

            this.rainDropTexture = Main.contentManager.Load<Texture2D>("Sprites\\Raindrop");

            this.dropSprite = new Sprite(this.rainDropTexture, new Vector2(0, 0));

            int dropType = Main.random.Next(2);

            this.dropSprite.Rectangle = dropType == 0 ? this.dropSprite.Rectangle = new Rectangle(0, 0, 16, 16) : this.dropSprite.Rectangle = new Rectangle(0, 16, 16, 16);
            this.dropSprite.Scale = 0.75f;

            this.dropSprite.Position.X = Main.random.Next(Main.graphics.PreferredBackBufferWidth) * 1.25f;

            this.rainDropSpeed = Main.random.Next(20, 30) / 10f;
            this.rainDropTime = Main.random.Next(1000, 5000);
            this.rainDropOpacity = Main.random.Next(5, 11) / 10f;
        }

        public void DrawDropParticle(GameTime dt) {

            this.rainDropSpeed = 0f;

            this.particleTimer += (float)dt.ElapsedGameTime.TotalSeconds;

            if (this.particleTimer > 0.2f) {

                this.particleTimer = 0f;

                this.dropSprite.Rectangle.X += 16;

                if (this.dropSprite.Rectangle.X > 48) {

                    this.destroy = true;
                }
            }
        }

        public void Update(GameTime dt) {

            this.timer += (float)dt.ElapsedGameTime.TotalMilliseconds;

            if (this.timer > this.rainDropTime) {

                this.drawParticle = true;
            }

            this.dropSprite.Position.X -= this.rainDropSpeed / 2f;
            this.dropSprite.Position.Y += this.rainDropSpeed;
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.dropSprite.Texture, this.dropSprite.Position, this.dropSprite.Rectangle, this.dropSprite.Hue * this.rainDropOpacity,
                         this.dropSprite.Rotation, this.dropSprite.Origin, this.dropSprite.Scale, this.dropSprite.Effect, this.dropSprite.Depth);
        }
    }
}