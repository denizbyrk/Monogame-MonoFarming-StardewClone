using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Entity.InventoryUtil {
    public class InventoryCell {

        public int CellID;
        public Sprite Sprite;
        public Rectangle defaultRectangle = new Rectangle(0, 297, 48, 48);
        public Rectangle highlightRectangle = new Rectangle(0, 345, 48, 48);
        public Item Item = new Item();
        public bool hasItem = false;
        public int itemCount = 0;
        public int maxItemCount = 10;
        public bool soundPlayed = false;

        public InventoryCell(Sprite sprite) {

            this.Item.Sprite = new Sprite(Main.debugTile, Vector2.Zero);

            this.Sprite = sprite;
        }

        public void AddItem(Item item) {

            this.Item = item;
            this.hasItem = true;

            if (item.isStackable == true) this.itemCount++;
            else this.itemCount = 1;
        }

        public void SetItem(Item item, int itemCount) {

            this.Item = item;
            this.hasItem = true;
            this.itemCount = itemCount;

            this.Item.Sprite.Position = new Vector2(this.Sprite.Position.X + 24, this.Sprite.Position.Y + 24);
        }

        public void DrawItemCount(SpriteBatch b) {

            if (this.itemCount > 1) {

                Helper.DrawTextOutline(b, Main.defaultFont, "" + this.itemCount, new Vector2(this.Sprite.Position.X + 36, this.Sprite.Position.Y + 36), 2.5f, Color.Black, 0.75f, Vector2.Zero);
                b.DrawString(Main.defaultFont, "" + this.itemCount, new Vector2(this.Sprite.Position.X + 36, this.Sprite.Position.Y + 36), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }
        }

        public Rectangle GetRectangle() => new Rectangle((int)this.Sprite.Position.X, (int)this.Sprite.Position.Y, this.Sprite.Rectangle.Width, this.Sprite.Rectangle.Height);
    }
}