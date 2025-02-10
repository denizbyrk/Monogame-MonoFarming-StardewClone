using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoFarming.Map.Weather {
    public class Rain {
 
        public List<Raindrop> raindrops;
        private float rainDensity;
        private float rainSpeed;
        private float timer;

        public Rain() {
            
            this.raindrops = new List<Raindrop>();

            this.rainDensity = 200;
            this.rainSpeed = 0.6f;
        }

        public void Update(GameTime dt) {

            this.timer += (float)dt.ElapsedGameTime.TotalSeconds;

            if (this.timer > this.rainSpeed) {

                Raindrop rd = new Raindrop();
                this.raindrops.Add(rd);
            }

            if (this.raindrops.Count > 0) {

                for (int i = 0; i< this.raindrops.Count; i++) {

                    this.raindrops[i].Update(dt);

                    if (this.raindrops[i].drawParticle == true) this.raindrops[i].DrawDropParticle(dt);
                    if (this.raindrops[i].destroy == true) this.raindrops.Remove(this.raindrops[i]);
                }
            }

            if (this.raindrops.Count > this.rainDensity) this.raindrops.Remove(this.raindrops.Last());
        }

        public void DrawRain(SpriteBatch b) {

            if (this.raindrops.Count > 0) {

                foreach (Raindrop r in this.raindrops) {

                    r.Draw(b);
                }
            }
        }
    }
}