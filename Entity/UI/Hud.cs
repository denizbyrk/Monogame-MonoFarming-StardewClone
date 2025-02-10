using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoFarming.Map;
using MonoFarming.Util;

namespace MonoFarming.Entity.UI {
    public class Hud {

        private Sprite Date;
        private DayTime dayTime;
        private Energy Energy;
        public static List<Pickup> pickupBars = new List<Pickup>();

        public Hud() {

            var dateChartTexture = Main.contentManager.Load<Texture2D>("Sprites\\EmptyFrame");
            var dateChartPosition = new Vector2(Main.screenDimensions[Main.currentScreenSize, 0] - (240), 16);

            this.Date = new Sprite(dateChartTexture, dateChartPosition);

            this.dayTime = new DayTime();

            this.Energy = new Energy();
        }

        public void Update(GameTime dt) {

            this.dayTime.Update(dt);

            this.Energy.Update();

            for (int i = 0; i < Hud.pickupBars.Count; i++) {

                Hud.pickupBars[i].FadeOut(dt, i);
            }
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.Date.Texture, this.Date.Position, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);

            this.dayTime.Draw(b, new Vector2(this.Date.Position.X + 52, this.Date.Position.Y + 68));

            this.Energy.Draw(b);

            foreach (Pickup p in Hud.pickupBars) {

                p.Draw(b);
            }
        }
    }
}