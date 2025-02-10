using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Map {
    public class Light {

        private Sprite light;
        private Texture2D regularLight;
        private Texture2D strongLight;
        private Texture2D softLight;

        public Light(int type, Vector2 position, float radius, float strength) {

            Texture2D lightType;

            if (type == 0) {

                this.strongLight = Main.contentManager.Load<Texture2D>("Effects\\Strong Light");
                lightType = this.strongLight;

            } else if (type == 1) {

                this.softLight = Main.contentManager.Load<Texture2D>("Effects\\Soft Light");
                lightType = this.softLight;

            } else {

                this.regularLight = Main.contentManager.Load<Texture2D>("Effects\\Regular Light");
                lightType = this.regularLight;
            }

            Vector2 pos = position;

            this.light = new Sprite(lightType, pos);

            this.light.Hue = new Color(255, 255, 255);
            this.light.Hue *= strength;
            this.light.Scale = radius;
            this.light.Origin = new Vector2(lightType.Width / 2, lightType.Height / 2);
            this.light.Position = new Vector2(pos.X * Main.targetTileSize, pos.Y * Main.targetTileSize);
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.light.Texture, this.light.Position, null, this.light.Hue,
                        this.light.Rotation, this.light.Origin, this.light.Scale, this.light.Effect, 0f);
        }
    }
}
