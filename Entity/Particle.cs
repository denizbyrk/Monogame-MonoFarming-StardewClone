using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoFarming.Util;
using MonoFarming.Animations.Animators;

namespace MonoFarming.Entity {
    public class Particle {

        public Sprite Particles;
        private Texture2D particleTexture;
        private ParticleAnimator ParticleAnimator;
        public int particleID;
        public bool dispose = false;

        public Particle(Vector2 Position) {

            this.ParticleAnimator = new ParticleAnimator();

            this.particleTexture = Main.contentManager.Load<Texture2D>("Sprites\\Particles");

            this.Particles = new Sprite(this.particleTexture, new Vector2(Position.X * Main.targetTileSize, Position.Y * Main.targetTileSize));
        }

        private void SetAnimations(int id) {

            this.ParticleAnimator.SetAnimation(id);
        }

        public void Update(GameTime dt) {

            if (this.ParticleAnimator.dispose == false) {

                this.SetAnimations(this.particleID);

            } else {

                this.dispose = true;
            }

            this.Particles.Rectangle = this.ParticleAnimator.Animate(dt, Main.targetTileSize, Main.targetTileSize);
        }

        public void Draw(SpriteBatch b) {

            b.Draw(this.Particles.Texture, this.Particles.Position, this.Particles.Rectangle, this.Particles.Hue,
                    this.Particles.Rotation, this.Particles.Origin, this.Particles.Scale, this.Particles.Effect, this.Particles.Depth);
        }
    }
}