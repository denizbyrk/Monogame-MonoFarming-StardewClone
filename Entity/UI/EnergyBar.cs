using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Entity.UI {
    public class EnergyBar {

        public Texture2D energyTextureAtlas = Main.contentManager.Load<Texture2D>("Sprites\\Energy");
        public Sprite energySprite;

        public EnergyBar(Vector2 pos) {

            this.energySprite = new Sprite(this.energyTextureAtlas, pos);
        }
    }
}