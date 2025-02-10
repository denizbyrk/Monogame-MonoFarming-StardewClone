using Microsoft.Xna.Framework;

namespace MonoFarming.Animations {
    public class Animation {

        public int currentAnimation;
        public int currentFrame;
        public int totalFrame;
        public float speed;
        public bool looping;
        public bool animationChanged = false;
        public bool animationComplete = false;
        private float timer;

        public Animation() {

            this.currentFrame = 0;
            this.looping = true;
        }

        protected virtual void Play(GameTime dt) {

            if (this.animationChanged == true) this.currentFrame = 0;
            this.animationChanged = false;

            if (this.currentFrame > this.totalFrame) this.currentFrame = 0;

            this.timer += (float)dt.ElapsedGameTime.TotalSeconds;

            if (this.timer > this.speed) {

                this.timer = 0f;
                this.currentFrame++;

                if ((this.currentFrame > this.totalFrame) && looping == true) {

                    this.currentFrame = 0;

                } else if ((this.currentFrame >= this.totalFrame) && looping == false) {

                    this.animationComplete = true;
                }
            }
        }

        protected virtual void Stop() {

            this.currentFrame = 0;
        }

        protected virtual Rectangle AnimateSheet(GameTime dt, int width, int height) {

            this.Play(dt);

            return new Rectangle(this.currentFrame * width, this.currentAnimation * height, width, height);
        }

        protected virtual void ResetAnimation() {

            this.currentFrame = 0;
        }

        protected virtual void GetDefaultAnimation(int currentAnimation, int totalFrame, float speed) {

            this.currentAnimation = currentAnimation;
            this.totalFrame = totalFrame;
            this.speed = speed;
        }
    }
}