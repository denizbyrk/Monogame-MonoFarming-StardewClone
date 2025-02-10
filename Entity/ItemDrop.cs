using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using MonoFarming.Scene.Scenes;

namespace MonoFarming.Entity {
    public class ItemDrop {

        public Texture2D itemTexture = Main.contentManager.Load<Texture2D>("Sprites\\Items");
        public Vector2 dropPosition;
        public Rectangle dropHitbox;
        public int itemID;
        public Vector2 velocity;
        private float minVelocityX = 2.0f;
        private float maxVelocityX = 4.0f;
        private float minVelocityY = 2.0f;
        private float maxVelocityY = 3.0f;
        public float horizontalDistance;
        public float verticalDistance;
        public bool canPickUp = false;
        public bool itemDestroyed = false;

        public ItemDrop(int itemID, Vector2 Position) {

            this.itemID = itemID;
            this.dropPosition = Position;
            this.dropHitbox = new Rectangle((int)this.dropPosition.X, (int)this.dropPosition.Y, 8, 8);

            Vector2 randomVelocity;

            randomVelocity = new Vector2(Main.random.NextSingle() * (this.maxVelocityX - this.minVelocityX) + this.minVelocityX, Main.random.NextSingle() * (this.maxVelocityY - this.minVelocityY) + this.minVelocityY);

            int temp = Main.random.Next(2);

            randomVelocity.X = temp == 0 ? randomVelocity.X *= -1 : randomVelocity.X;

            this.velocity = randomVelocity;
        }

        public void Update(GameTime dt) {

            float speed = 0.2f;

            if (this.velocity.X > 0) {

                this.velocity.X -= speed;
                this.velocity.X = MathHelper.Clamp(this.velocity.X, 0, this.maxVelocityX);
            }
            if (this.velocity.X < 0) {

                this.velocity.X += speed;
                this.velocity.X = MathHelper.Clamp(this.velocity.X, -this.maxVelocityX, 0);
            }

            this.velocity.Y -= speed;

            this.velocity.Y = MathHelper.Clamp(this.velocity.Y, 0, this.maxVelocityY);

            horizontalDistance = (float)Main.random.NextDouble() * 3;
            verticalDistance = (float)Main.random.NextDouble() * 3;

            dropPosition.X += velocity.X;
            dropPosition.Y -= velocity.Y;

            this.dropHitbox = new Rectangle((int)this.dropPosition.X, (int)this.dropPosition.Y, 8, 8);

            if (this.velocity == new Vector2(0, 0)) this.canPickUp = true;
        }

        public float CalculateDistance(Vector2 position, Vector2 position2) {

            float distance = (float)Math.Sqrt((position.X - position2.X) * (position.X - position2.X) + (position.Y - position2.Y) * (position.Y - position2.Y));

            return distance;
        }

        public void MoveTowards(ItemDrop itemToDestroy, Rectangle dest) {

            Vector2 destination = new Vector2(dest.X, dest.Y);

            Vector2 direction = destination - this.dropPosition;

            this.dropPosition += direction * 0.15f;

            //if (this.CalculateDistance(destination, this.dropPosition) < 1) {

            //    Overworld.DroppedItems.Remove(itemToDestroy);
            //}

            if (itemToDestroy.dropHitbox.Intersects(dest)) {

                Overworld.currentMap.DroppedItems.Remove(itemToDestroy);
                this.itemDestroyed = true;
            }
        }
    }
}