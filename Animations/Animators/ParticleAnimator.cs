using Microsoft.Xna.Framework;

namespace MonoFarming.Animations.Animators {
    public class ParticleAnimator : Animation {

        public bool dispose = false;

        public ParticleAnimator() {

            this.looping = false;
        }

        private void SetAnimationProperties(int currentAnimation, int totalFrame, float speed) {

            this.currentAnimation = currentAnimation;
            this.totalFrame = totalFrame;
            this.speed = speed;
        }

        public void SetAnimation(int animationID) {

            if (this.animationComplete == true) this.dispose = true;

            switch (animationID) {

                case 0:

                    this.SetAnimationProperties(1, 8, 0.1f);

                    break;

                case 1:

                    this.SetAnimationProperties(0, 9, 0.1f);

                    break;

                case 2:

                    this.SetAnimationProperties(2, 8, 0.1f);

                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Invalid Animation");
                    break;
            }
        }

        public Rectangle Animate(GameTime dt, int width, int height) {

            return this.AnimateSheet(dt, width, height);
        }
    }
}