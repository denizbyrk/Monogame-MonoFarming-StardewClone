using Microsoft.Xna.Framework;
using MonoFarming.Audio;
using MonoFarming.Entity;

namespace MonoFarming.Animations.Animators {
    public class PlayerAnimator : Animation {

        public PlayerAnimator() { }

        private void SetAnimationProperties(int currentAnimation, int totalFrame, float speed) {

            this.currentAnimation = currentAnimation;
            this.totalFrame = totalFrame;
            this.speed = speed;
        }

        public void SetAnimation(string animationToPlay, byte facing) {

            if (this.animationComplete == true) {

                this.GetDefaultAnimation(currentAnimation, totalFrame, speed);
            }

            switch (animationToPlay) {

                case "Idle":

                    this.animationComplete = false;
                    this.looping = true;

                    switch (facing) {

                        case 1:
                            this.SetAnimationProperties(1, 1, 0.4f);
                            break;
                        case 2:
                            this.SetAnimationProperties(0, 1, 0.4f);
                            break;
                        case 3:
                            this.SetAnimationProperties(3, 1, 0.4f);
                            break;
                        case 4:
                            this.SetAnimationProperties(2, 1, 0.4f);
                            break;
                    }

                    break;

                case "Run":

                    if (Player.freeze == false) {

                        this.animationComplete = false;
                        this.looping = true;

                        //play step sound effect every 2 animation frames
                        if (this.currentFrame % 2 == 1) {

                            if (Player.isInterior == false) {

                                SoundEffectAudio.SoundEffectInstances["Step"].Pitch = (float)Main.random.Next(0, 5) / 10f;
                                SoundEffectAudio.SoundEffectInstances["Step"].Volume = 0.2f;
                                SoundEffectAudio.SoundEffectInstances["Step"].Play();

                            } else {

                                SoundEffectAudio.SoundEffectInstances["Step2"].Pitch = (float)Main.random.Next(-5, 5) / 10f;
                                SoundEffectAudio.SoundEffectInstances["Step2"].Volume = 0.2f;
                                SoundEffectAudio.SoundEffectInstances["Step2"].Play();
                            }
                        }
                    }

                    switch (facing) {

                        case 1:
                            this.SetAnimationProperties(5, 3, 0.2f);
                            break;
                        case 2:
                            this.SetAnimationProperties(4, 3, 0.2f);
                            break;
                        case 3:
                            this.SetAnimationProperties(7, 3, 0.2f);
                            break;
                        case 4:
                            this.SetAnimationProperties(6, 3, 0.225f);
                            break;
                    }

                    break;

                case "UseTool":

                    this.looping = false;

                    if (this.currentFrame == 0) this.animationChanged = true;

                    if (this.currentFrame > 2) Player.isUsingTool = false;

                    switch (facing) {

                        case 1:

                            this.SetAnimationProperties(17, 2, 0.3f);
                            break;
                        case 2:
                            this.SetAnimationProperties(16, 2, 0.3f);
                            break;
                        case 3:
                            this.SetAnimationProperties(19, 2, 0.3f);
                            break;
                        case 4:
                            this.SetAnimationProperties(18, 2, 0.3f);
                            break;
                    }

                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Invalid Animation");
                    break;
            }
        }

        public Rectangle Animate(GameTime dt, int width, int height) {

            return this.AnimateSheet(dt, width, height);
        }

        public int CurrentAnimationFrame() => this.currentFrame;
    }
}