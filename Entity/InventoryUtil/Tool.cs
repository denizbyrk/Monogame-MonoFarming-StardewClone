using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Util;

namespace MonoFarming.Entity.InventoryUtil {
    public class Tool : Item {

        public enum ToolType {

            Pickaxe,
            Axe,
            Hoe,
            Scythe,
            None
        }
        public ToolType Type { get; set; }

        public Texture2D toolTexture;

        public new int ID;
        public new string Name;
        public new string Description;
        public new bool isStackable;
        public string canBreak;

        public Tool(int ToolID, Texture2D texture, Vector2 position) {

            this.ID = ToolData.Data[ToolID].ID;
            this.Name = ToolData.Data[ToolID].Name;
            this.Description = ToolData.Data[ToolID].Description;
            this.isStackable = ToolData.Data[ToolID].isStackable;
            this.canBreak = ToolData.Data[ToolID].canBreak;

            this.Sprite = new Sprite(texture, position);
            this.Sprite.Rectangle = new Rectangle(16 * ToolID, 0, 16, 16);
            this.Sprite.Origin = new Vector2(8, 8);
        }
    }
}