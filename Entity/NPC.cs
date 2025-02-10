using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Entity.UI;
using MonoFarming.Util;

namespace MonoFarming.Entity {
    public class NPC {

        public Sprite Sprite;
        public int X;
        public int Y;
        public EntityIndicator Indicator;
        public bool hasDialog;
        public Dialog Dialog;
        
        public virtual void Update(GameTime dt) { }

        public virtual void Draw(SpriteBatch b) { }

        public virtual void DisplayDialog(SpriteBatch b) {

            if (this.Dialog != null && this.Dialog.drawDialog == true) {
                
                this.Dialog.DisplayDialog(b);
            }
        }
    }
}