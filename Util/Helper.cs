using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoFarming.Map;
using MonoFarming.Entity;
using MonoFarming.Scene.Scenes;

namespace MonoFarming.Util {
    public class Helper {

        private static float timer;
        public static float alpha = 0f;
        private static float transitionTime = 0.6f;
        private static float transitionSpeed = 0.032f;
        public static bool goToNextMap = false;

        public static Vector2 currentTilePosition;
        public static float tileCursorAlpha = 1f;
        public static Color tileCursorColor = Color.White;

        public static void BlackScreenTransition(SpriteBatch b, GameTime dt) {

            Overworld.freezeTime = true;

            Helper.timer += (float)dt.ElapsedGameTime.TotalSeconds;

            Rectangle blackScreen = new Rectangle(0, 0, Main.screenDimensions[Main.currentScreenSize, 0], Main.screenDimensions[Main.currentScreenSize, 1]);

            Helper.alpha = (Helper.timer < Helper.transitionTime) ? Helper.alpha += Helper.transitionSpeed : (Helper.timer > Helper.transitionTime) ? Helper.alpha -= Helper.transitionSpeed : Helper.alpha = -1f;
            Helper.alpha = MathHelper.Clamp(Helper.alpha, 0f, 1f);

            Helper.goToNextMap = Helper.alpha == 1f ? true : false;

            if (Helper.alpha == 0f) {

                Overworld.freezeTime = false;
                Player.canAction = true;
                Player.freeze = false;
                Overworld.doTransition = false;
                Helper.alpha = 0f;
                Helper.timer = 0f;
                Overworld.Camera.isInstant = false;
            }

            b.Draw(Main.pixel, blackScreen, Color.Black * Helper.alpha);
        }

        public static void DrawLine(SpriteBatch b, Vector2 point1, Vector2 point2, float stroke, Color color) {

            float x = Vector2.Distance(point1, point2);
            float rotation = (float)Math.Atan2((double)point2.Y - (double)point1.Y, (double)point2.X - (double)point1.X);

            Vector2 origin = new Vector2(0.0f, 0.5f);
            Vector2 scale = new Vector2(x, (float)stroke);

            b.Draw(Main.pixel, point1, null, color, rotation, origin, scale, SpriteEffects.None, 0.0f);
        }

        public static void DrawOutlineRectangle(SpriteBatch b, int posX, int posY, int width, int height, int stroke, Color color) {

            Rectangle edge1 = new Rectangle(posX, posY, width, stroke);
            Rectangle edge2 = new Rectangle(posX, posY, stroke, height);
            Rectangle edge3 = new Rectangle(posX + width - stroke, posY, stroke, height);
            Rectangle edge4 = new Rectangle(posX, posY + height - stroke, width, stroke);

            List<Rectangle> edges = new List<Rectangle> { edge1, edge2, edge3, edge4 };

            foreach (Rectangle edge in edges) {

                b.Draw(Main.pixel, edge, color);
            }
        }

        public static void DrawTextOutline(SpriteBatch b, SpriteFont font, string text, Vector2 textPosition, float stroke, Color outlineColor, float size, Vector2 origin) {

            b.DrawString(font, text, new Vector2(textPosition.X - stroke, textPosition.Y), outlineColor, 0f, origin, size, SpriteEffects.None, 0f);
            b.DrawString(font, text, new Vector2(textPosition.X + stroke, textPosition.Y), outlineColor, 0f, origin, size, SpriteEffects.None, 0f);
            b.DrawString(font, text, new Vector2(textPosition.X, textPosition.Y - stroke), outlineColor, 0f, origin, size, SpriteEffects.None, 0f);
            b.DrawString(font, text, new Vector2(textPosition.X, textPosition.Y + stroke), outlineColor, 0f, origin, size, SpriteEffects.None, 0f);
        }

        public static void DrawGrid(SpriteBatch b) {

            for (int i = 0; i < Overworld.currentMap.X; i++) {

                float stroke = 0.5f;
                if (Overworld.Camera.Zoom < 2f) {

                    stroke = 1f;
                }

                if (Main.currentScreenSize == 0) {

                    stroke = 1f;

                    if (Overworld.Camera.Zoom < 2f) {

                        stroke = 2f;
                    }

                    if (Overworld.Camera.Zoom < 1.5f) {

                        stroke = 3f;
                    }
                }
                Color color = Color.Black;

                Helper.DrawLine(b, new Vector2(i * Main.targetTileSize, 0), new Vector2(i * Main.targetTileSize, 3840), stroke, color);
                Helper.DrawLine(b, new Vector2(0, i * Main.targetTileSize), new Vector2(2160, i * Main.targetTileSize), stroke, color);
            }
        }

        public static Vector2 getMouseTile() {

            Vector2 mouseTile;

            mouseTile = new Vector2((float)Math.Floor(Input.getRelativeMouseX() / Main.targetTileSize), (float)Math.Floor(Input.getRelativeMouseY() / Main.targetTileSize));

            return mouseTile;
        }

        public static void DrawMouseCursor(SpriteBatch b, Texture2D t) {

            Vector2 Position = new Vector2(Input.getMouseX() - 4, Input.getMouseY() - 4);

            float scale = 2f;

            b.Draw(t, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void DrawTileCursor(SpriteBatch b, Texture2D t) {

            Helper.currentTilePosition = new Vector2(Helper.getMouseTile().X * Main.targetTileSize, Helper.getMouseTile().Y * Main.targetTileSize);

            float scale = 1f;

            b.Draw(t, Helper.currentTilePosition, null, Helper.tileCursorColor * Helper.tileCursorAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public static void MakeMapTransition(MapLoader map) {

            Overworld.currentMap = map;
        }

        public static bool CheckRectangleCollisionLeft(Rectangle r1, Rectangle r2, Vector2 ray) {

            return r1.Right + ray.X > r2.Left &&
                   r1.Left < r2.Left &&
                   r1.Bottom > r2.Top &&
                   r1.Top < r2.Bottom;
        }

        public static bool CheckRectangleCollisionRight(Rectangle r1, Rectangle r2, Vector2 ray) {

            return r1.Left + ray.X < r2.Right &&
                   r1.Right < r2.Right &&
                   r1.Bottom > r2.Top &&
                   r1.Top < r2.Bottom;
        }

        public static bool CheckRectangleCollisionTop(Rectangle r1, Rectangle r2, Vector2 ray) {

            return r1.Bottom + ray.Y > r2.Top &&
                   r1.Top < r2.Top &&
                   r1.Right > r2.Left &&
                   r1.Left < r2.Right;
        }

        public static bool CheckRectangleCollisionBottom(Rectangle r1, Rectangle r2, Vector2 ray) {

            return r1.Top + ray.Y < r2.Bottom &&
                   r1.Bottom < r2.Bottom &&
                   r1.Right > r2.Left &&
                   r1.Left < r2.Right;
        }
    }
}