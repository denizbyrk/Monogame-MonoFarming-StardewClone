using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;
using MonoFarming.Entity.UI;

namespace MonoFarming.Entity {
    public class Villager : NPC {

        private Texture2D Texture;
        public bool canInteract = false;

        public Villager() {

            this.Texture = Main.contentManager.Load<Texture2D>("Sprites\\Villager");

            this.X = 68;
            this.Y = 12;

            this.Sprite = new Sprite(this.Texture, new Vector2(this.X * Main.targetTileSize - 4, this.Y * Main.targetTileSize - 8));
            this.Sprite.Rectangle = new Rectangle(0, 0, 24, 24);

            this.Indicator = new EntityIndicator(new Vector2(this.Sprite.Position.X + 4.5f, this.Sprite.Position.Y - 12));

            this.hasDialog = true;

            if (this.hasDialog == true) {

                this.Dialog = new Dialog();

                this.Dialog.addDialog("Press E to toggle inventory.");
                this.Dialog.addDialog("Press LShift to sprint.\n\nPress Z for zooming in and X\n\nfor zooming out.");
                this.Dialog.addDialog("You can access the debug mode\n\nby pressing CTRL + P.");
                this.Dialog.addDialog("If you are in debug mode,\n\npress M to toggle rain, and\n\npress N to toggle time speed.");
                this.Dialog.addDialog("If you got stuck somewhere,\n\npress L to reset position.");

                this.Dialog.DialogSprite.Origin = new Vector2(this.Dialog.DialogSprite.Texture.Width / 2, this.Dialog.DialogSprite.Texture.Height / 2);
                this.Dialog.DialogSprite.Scale = 1f;

                this.Dialog.drawDialog = false;
            }
        }

        public override void Update(GameTime dt) {

            if (this.Indicator.drawIndicator == true) this.Indicator.AnimateIndicator(dt);

            this.Dialog.AnimateIcon(dt);
        }

        public override void DisplayDialog(SpriteBatch b) {

            base.DisplayDialog(b);
        }

        public override void Draw(SpriteBatch b) {

            b.Draw(this.Sprite.Texture, this.Sprite.Position, this.Sprite.Rectangle, this.Sprite.Hue,
                    this.Sprite.Rotation, this.Sprite.Origin, this.Sprite.Scale, this.Sprite.Effect, this.Sprite.Depth);

            if (this.Indicator.drawIndicator == true) this.Indicator.Draw(b);
        }
    }
}