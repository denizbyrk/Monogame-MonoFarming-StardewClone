using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Entity.UI {
    public class EntityIndicator {

        private Texture2D Texture;
        private Sprite IndicatorSprite;
        private Vector2 defaultPosition;
        public float movingOffset;
        public bool drawIndicator = true;
        private float timer;

        public EntityIndicator(Vector2 Position) {

            this.Texture = Main.contentManager.Load<Texture2D>("Sprites\\Indicator");
            this.defaultPosition = Position;

            this.IndicatorSprite = new Sprite(this.Texture, this.defaultPosition);
            this.movingOffset = 4;
        }

        public void AnimateIndicator(GameTime dt) {

            this.timer += (float)dt.ElapsedGameTime.TotalSeconds;

            if (this.timer >= 0.5f) {

                if (this.IndicatorSprite.Position.Y == this.defaultPosition.Y) this.IndicatorSprite.Position.Y = this.defaultPosition.Y - this.movingOffset;
                else if (this.IndicatorSprite.Position.Y == this.defaultPosition.Y - this.movingOffset) this.IndicatorSprite.Position.Y = this.defaultPosition.Y;
                else this.IndicatorSprite.Position.Y = this.defaultPosition.Y;

                this.timer = 0f;
            }
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.IndicatorSprite.Texture, this.IndicatorSprite.Position, null, this.IndicatorSprite.Hue,
                    this.IndicatorSprite.Rotation, this.IndicatorSprite.Origin, this.IndicatorSprite.Scale, this.IndicatorSprite.Effect, this.IndicatorSprite.Depth);
        }
    }
}