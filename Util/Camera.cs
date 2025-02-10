using Microsoft.Xna.Framework;
using System;
using MonoFarming.Scene.Scenes;

namespace MonoFarming.Util {
    public class Camera {

        public bool firstFrame = true;
        public bool isInstant = true;
        public Vector2 Center { get; set; }

        private Vector2 quarterScreen;
        public Vector2 GetTopLeft() => Center - quarterScreen;

        public float defaultZoom = 2.5f;
        private float zoom = 2.5f;
        public float Zoom {
            get {

                return this.zoom;
            }
            set {

                value = MathHelper.Clamp(value, 1920 / (Overworld.currentMap.X * Main.targetTileSize * 1.5f), 6f);
                this.zoom = value;
            }
        }

        public void UpdateZoom() {

            this.zoom = this.zoom < Overworld.currentMap.maxZoomLevel ? this.zoom = Overworld.currentMap.maxZoomLevel : this.zoom;
        }

        public void ResetZoom() {

            this.zoom = this.defaultZoom;
        }

        public float setBoundaryX() => Overworld.currentMap.X * (Main.targetTileSize * (this.zoom - Overworld.currentMap.maxZoomLevel));

        public float setBoundaryY() => Overworld.currentMap.Y * (Main.targetTileSize * (this.zoom - Overworld.currentMap.maxZoomLevel));

        public float setBoundaryInteriorX() => Overworld.currentMap.X / 2 * (Main.targetTileSize * (this.zoom - Overworld.currentMap.maxZoomLevel));

        public float setBoundaryInteriorY() => Overworld.currentMap.Y / 2 * (Main.targetTileSize * (this.zoom - Overworld.currentMap.maxZoomLevel));

        public Matrix getTransformation() {

            Matrix cameraTransform = Matrix.CreateTranslation(new Vector3(-Overworld.Camera.GetTopLeft().X, -Overworld.Camera.GetTopLeft().Y, 0)) *
                                        Matrix.CreateScale(new Vector3(Overworld.Camera.Zoom, Overworld.Camera.Zoom, 0));

            cameraTransform.M41 = (float)Math.Round(cameraTransform.M41);
            cameraTransform.M42 = (float)Math.Round(cameraTransform.M42);

            if (Overworld.currentMap.isInterior == false) {

                cameraTransform.M41 = MathHelper.Clamp(cameraTransform.M41, -this.setBoundaryX(), 0);
                cameraTransform.M42 = MathHelper.Clamp(cameraTransform.M42, -this.setBoundaryY() - (Overworld.currentMap.boundaryY), 0);

            } else {

                cameraTransform.M41 = MathHelper.Clamp(cameraTransform.M41, -this.setBoundaryInteriorX(), this.setBoundaryX());
                cameraTransform.M42 = MathHelper.Clamp(cameraTransform.M42, -this.setBoundaryInteriorY(), this.setBoundaryY());
            }

            if (Overworld.mapChanged == true) {

                cameraTransform.M41 = Overworld.currentMap.defaultX * Main.targetTileSize;
                cameraTransform.M42 = Overworld.currentMap.defaultY * Main.targetTileSize;
            }

            return cameraTransform;
        }

        public void MoveToward(Vector2 target, float deltaTimeInMs, float movePercentage) {

            this.quarterScreen = new Vector2(Main.graphics.PreferredBackBufferWidth / (this.zoom * (float)(Main.screenDimensions[Main.currentScreenSize, 0] / 640)) - (Main.targetTileSize - 4), Main.graphics.PreferredBackBufferHeight / (this.zoom * (float)(Main.screenDimensions[Main.currentScreenSize, 1] / 360)) - (Main.targetTileSize - 2));

            Vector2 differenceInPosition = target - this.Center;

            differenceInPosition *= movePercentage;

            if (this.isInstant == true) {

                this.Center += differenceInPosition;

                this.Center = target;

            } else if (this.isInstant == false) {

                var fractionOfPassedTime = deltaTimeInMs / 10;
                this.Center += differenceInPosition * fractionOfPassedTime;

                if ((target - this.Center).Length() < movePercentage) {

                    this.Center = target;

                }
            }
        }

        public void CameraControl() {

            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Z)) {
                this.Zoom += 0.5f;
            }
            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.X)) {
                this.Zoom -= 0.5f;
            }
            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.C)) {
                this.ResetZoom();
            }
        }
    }
}