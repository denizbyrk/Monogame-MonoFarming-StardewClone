using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Entity.UI {
    public class ItemDescription {

        private Texture2D BoxTexture;
        private Sprite nameBox1;
        private Sprite nameTextBox;
        private Sprite nameBox2;
        private Sprite descriptionTextBox;
        private Sprite descriptionHorizontal;
        private Sprite descriptionVertical;
        private Sprite fill;
        public Vector2 Position; //position of the box
        public string ItemName; //item name
        public string ItemType; //item type
        public string Description; //item description
        private float scale = 1.5f; //general scale
        private float xScale; //how much the box extends in x axis
        private float yScale; //how much the box extends in y axis
        private SpriteFont DescriptionFont;

        public ItemDescription(Vector2 Position, string itemName, string itemType, string itemDescription) {

            this.DescriptionFont = Main.contentManager.Load<SpriteFont>("Font\\DescriptionFont");
            this.BoxTexture = Main.contentManager.Load<Texture2D>("Sprites\\ItemProperties");

            this.Position = Position;
            this.ItemName = itemName;
            this.ItemType = itemType;
            this.Description = itemDescription;

            //measure the string length
            Vector2 nameLength = Main.defaultFont.MeasureString(this.ItemName);
            Vector2 descLength = Main.defaultFont.MeasureString(this.Description);

            //set the box scale
            this.xScale = (nameLength.X > descLength.X) ? (nameLength.X / 3) : (nameLength.X < descLength.X ? (descLength.X / 3) : (nameLength.X / 3));
            this.yScale = descLength.Y / 2.5f;

            this.nameBox1 = new Sprite(this.BoxTexture, this.Position);
            this.nameBox1.Rectangle = new Rectangle(0, 0, 8, 46);
            this.nameBox1.Scale = this.scale;

            this.nameTextBox = new Sprite(this.BoxTexture, new Vector2(this.nameBox1.Position.X + (this.nameBox1.Rectangle.Width * this.scale), this.nameBox1.Position.Y));
            this.nameTextBox.Rectangle = new Rectangle(8, 0, 2, 46);
            this.nameTextBox.Scale = this.scale;

            this.nameBox2 = new Sprite(this.BoxTexture, new Vector2(this.nameBox1.Position.X + ((this.xScale * 2) - 3) + ((this.nameBox1.Rectangle.Width + this.nameBox1.Rectangle.X) * this.scale), this.nameTextBox.Position.Y));
            this.nameBox2.Rectangle = new Rectangle(10, 0, 8, 46);
            this.nameBox2.Scale = this.scale;

            this.descriptionTextBox = new Sprite(this.BoxTexture, new Vector2(this.nameBox1.Position.X, this.nameBox1.Position.Y + (this.nameBox1.Rectangle.Height * this.scale) - 3));
            this.descriptionTextBox.Rectangle = new Rectangle(0, 44, 8, 6);
            this.descriptionTextBox.Scale = this.scale;

            this.descriptionHorizontal = new Sprite(this.BoxTexture, new Vector2(this.descriptionTextBox.Position.X + 12, this.descriptionTextBox.Position.Y));
            this.descriptionHorizontal.Rectangle = new Rectangle(8, 44, 2, 6);
            this.descriptionHorizontal.Scale = this.scale;

            this.descriptionVertical = new Sprite(this.BoxTexture, new Vector2(this.nameBox1.Position.X, this.descriptionTextBox.Position.Y + 9));
            this.descriptionVertical.Rectangle = new Rectangle(0, 50, 8, 2);
            this.descriptionVertical.Scale = this.scale;
            
            this.fill = new Sprite(this.BoxTexture, new Vector2(this.descriptionVertical.Position.X + 12, this.descriptionVertical.Position.Y));
            this.fill.Rectangle = new Rectangle(8, 50, 2, 2);
        }

        public void Draw(SpriteBatch b) {

            //draw left side of name box
            b.Draw(this.nameBox1.Texture, this.nameBox1.Position, this.nameBox1.Rectangle, this.nameBox1.Hue,
                    this.nameBox1.Rotation, this.nameBox1.Origin, this.nameBox1.Scale, this.nameBox1.Effect, this.nameBox1.Depth);

            //draw the box for name box
            b.Draw(this.nameTextBox.Texture, this.nameTextBox.Position, this.nameTextBox.Rectangle, this.nameTextBox.Hue,
                    this.nameTextBox.Rotation, this.nameTextBox.Origin, new Vector2(this.xScale, this.nameTextBox.Scale), this.nameTextBox.Effect, this.nameTextBox.Depth);

            //draw the right side for name box
            b.Draw(this.nameBox2.Texture, this.nameBox2.Position, this.nameBox2.Rectangle, this.nameBox2.Hue,
                    this.nameBox2.Rotation, this.nameBox2.Origin, this.nameBox2.Scale, this.nameBox2.Effect, this.nameBox2.Depth);

            //fill the description box background
            b.Draw(this.fill.Texture, this.fill.Position, this.fill.Rectangle, this.fill.Hue,
                    this.fill.Rotation, this.fill.Origin, new Vector2(this.xScale, this.yScale), this.fill.Effect, this.fill.Depth);

            //draw description box horizontally for the top side
            b.Draw(this.descriptionHorizontal.Texture, this.descriptionHorizontal.Position, this.descriptionHorizontal.Rectangle, this.descriptionHorizontal.Hue,
                    this.descriptionHorizontal.Rotation, this.descriptionHorizontal.Origin, new Vector2(this.xScale, this.descriptionHorizontal.Scale), this.descriptionHorizontal.Effect, this.descriptionHorizontal.Depth);

            //draw description box horizontally for the bottom side
            b.Draw(this.nameTextBox.Texture, new Vector2(this.descriptionHorizontal.Position.X, this.descriptionTextBox.Position.Y + 9 + (this.descriptionTextBox.Rectangle.Height * this.yScale / 3)),
                    new Rectangle(8, 0, 2, 8), this.nameTextBox.Hue, this.nameTextBox.Rotation, this.nameTextBox.Origin, new Vector2(this.xScale, this.nameTextBox.Scale), SpriteEffects.FlipVertically, this.nameTextBox.Depth);

            //draw description box vertically for the left side
            b.Draw(this.descriptionVertical.Texture, this.descriptionVertical.Position, this.descriptionVertical.Rectangle, this.descriptionVertical.Hue,
                    this.descriptionVertical.Rotation, this.descriptionVertical.Origin, new Vector2(this.descriptionVertical.Scale, this.yScale), this.descriptionVertical.Effect, this.descriptionVertical.Depth);

            //draw description box vertically for the right side
            b.Draw(this.descriptionVertical.Texture, new Vector2(this.nameBox2.Position.X, this.descriptionVertical.Position.Y), this.descriptionVertical.Rectangle, this.descriptionVertical.Hue,
                    this.descriptionVertical.Rotation, this.descriptionVertical.Origin, new Vector2(this.descriptionVertical.Scale, this.yScale), SpriteEffects.FlipHorizontally, this.descriptionVertical.Depth);
            
            //draw top left corner of description box
            b.Draw(this.descriptionTextBox.Texture, this.descriptionTextBox.Position, this.descriptionTextBox.Rectangle, this.descriptionTextBox.Hue,
                    this.descriptionTextBox.Rotation, this.descriptionTextBox.Origin, this.descriptionTextBox.Scale, this.descriptionTextBox.Effect, this.descriptionTextBox.Depth);

            //draw bottom left corner of description box
            b.Draw(this.nameBox1.Texture, new Vector2(this.descriptionTextBox.Position.X, this.descriptionTextBox.Position.Y + 9 + (this.descriptionTextBox.Rectangle.Height * this.yScale / 3)),
                    new Rectangle(this.nameBox1.Rectangle.X, this.nameBox1.Rectangle.Y, 8, 8), this.nameBox1.Hue, this.nameBox1.Rotation, this.nameBox1.Origin, this.nameBox1.Scale, SpriteEffects.FlipVertically, this.nameBox1.Depth);

            //draw top right corner of description box
            b.Draw(this.nameBox1.Texture, new Vector2(this.nameBox2.Position.X, this.descriptionTextBox.Position.Y),
                    new Rectangle(10, 44, 8, 6), this.nameBox1.Hue, this.nameBox1.Rotation, this.nameBox1.Origin, this.nameBox1.Scale, SpriteEffects.None, this.nameBox1.Depth);

            //draw the bottom right corener of the description box
            b.Draw(this.nameBox1.Texture, new Vector2(this.nameBox2.Position.X, this.descriptionTextBox.Position.Y + 9 + (this.descriptionTextBox.Rectangle.Height * this.yScale / 3)),
                    new Rectangle(10, 52, 8, 8), this.nameBox1.Hue, this.nameBox1.Rotation, this.nameBox1.Origin, this.nameBox1.Scale, SpriteEffects.None, this.nameBox1.Depth);

            //draw item name
            Helper.DrawTextOutline(b, Main.defaultFont, "" + this.ItemName, new Vector2(this.nameTextBox.Position.X + 1, this.nameTextBox.Position.Y + 20), 2f, Color.Black, 1f, Vector2.Zero);
            b.DrawString(Main.defaultFont, "" + this.ItemName, new Vector2(this.nameTextBox.Position.X + 1, this.nameTextBox.Position.Y + 20), Color.White);

            //draw item type
            b.DrawString(Main.defaultFont, "" + this.ItemType, new Vector2(this.nameTextBox.Position.X + 1, this.nameTextBox.Position.Y + 42), Color.Black, 0f, Vector2.Zero, 0.85f, SpriteEffects.None, 0f);

            //draw item description
            Helper.DrawTextOutline(b, this.DescriptionFont, "" + this.Description, new Vector2(this.fill.Position.X + 1, this.fill.Position.Y + 2), 1.75f, Color.Black, 1f, Vector2.Zero);
            b.DrawString(this.DescriptionFont, "" + this.Description, new Vector2(this.fill.Position.X + 1, this.fill.Position.Y + 2), Color.White);
        }
    }
}