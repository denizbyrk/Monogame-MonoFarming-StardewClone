using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFarming.Util {
    public class Sprite {

        public Texture2D Texture; //texture of the sprite
        public Rectangle Rectangle; //rectangle of the sprite
        public Vector2 Position; //position of the sprite
        public Vector2 Origin; //origin of the sprite
        public Color Hue; //color of the sprite
        public float Rotation; //rotation of the sprite
        public float Scale; //scale of the sprite
        public float Depth; //depth of the sprite
        public SpriteEffects Effect; //effect of the sprite

        public Sprite(Texture2D t, Vector2 p) {

            if (t != null) this.Texture = t;

            this.Position = p;

            this.Default();
        }

        //method for default values
        private void Default() {

            this.Origin = Vector2.Zero;
            this.Hue = Color.White;
            this.Rotation = 0f;
            this.Scale = 1f;
            this.Depth = 0f;
            this.Effect = SpriteEffects.None;
        }
    }
}