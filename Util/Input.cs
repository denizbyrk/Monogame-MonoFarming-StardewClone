using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoFarming.Scene.Scenes;

namespace MonoFarming.Util {
    public class Input {

        private static KeyboardState prevKState; 
        private static KeyboardState currentKState;

        private static MouseState prevMState;
        private static MouseState currentMState;

        public static int prevScroll;
        public static int currentScroll;

        //check key press
        public static bool IsKeyDown(Keys k) => Input.prevKState.IsKeyUp(k) && Input.currentKState.IsKeyDown(k);

        //check if a key is being held
        public static bool IsKeyHold(Keys k) => Input.currentKState.IsKeyDown(k);

        //check left click
        public static bool IsLeftClickDown() => Input.prevMState.LeftButton == ButtonState.Released && Input.currentMState.LeftButton == ButtonState.Pressed;

        //check left click release
        public static bool IsLeftClickReleased() => Input.prevMState.LeftButton == ButtonState.Pressed && Input.currentMState.LeftButton == ButtonState.Released;

        //check right click
        public static bool IsRightClickDown() => Input.prevMState.RightButton == ButtonState.Released && Input.currentMState.RightButton == ButtonState.Pressed;

        //check right click release
        public static bool IsRightClickReleased() => Input.prevMState.RightButton == ButtonState.Pressed && Input.currentMState.RightButton == ButtonState.Released;

        //get mouse position X
        public static float getMouseX() => Mouse.GetState().X;

        //get mouse position Y
        public static float getMouseY() => Mouse.GetState().Y;

        //get mouse position
        public static Vector2 getMousePosition() => new Vector2(Input.getMouseX(), Input.getMouseY());

        //get mouse position X, relative to the current map and camera position
        public static float getRelativeMouseX() => (Input.getMouseX() / Overworld.Camera.Zoom) + (-Overworld.Camera.getTransformation().M41 / Overworld.Camera.Zoom);

        //get mouse position Y, relative to the current map and camera position
        public static float getRelativeMouseY() => (Input.getMouseY() / Overworld.Camera.Zoom) + (-Overworld.Camera.getTransformation().M42 / Overworld.Camera.Zoom);

        //get mouse hitbox
        public static Rectangle getMouseRectangle() => new Rectangle((int)Input.getMouseX(), (int)Input.getMouseY(), 1, 1);

        //get mouse hitbox, relative to current map and camera position
        public static Rectangle getRelativeMouseRectangle() => new Rectangle((int)Input.getRelativeMouseX(), (int)Input.getRelativeMouseY(), 1, 1);

        //get scroll
        public static int getScroll() => Input.currentMState.ScrollWheelValue / 120;

        //update inputs
        public static void Update() {

            Input.prevKState = Input.currentKState;
            Input.currentKState = Keyboard.GetState();

            Input.prevMState = Input.currentMState;
            Input.currentMState = Mouse.GetState();

            Input.prevScroll = Input.currentScroll;
            Input.currentScroll = Input.currentMState.ScrollWheelValue;
        }
    }
}