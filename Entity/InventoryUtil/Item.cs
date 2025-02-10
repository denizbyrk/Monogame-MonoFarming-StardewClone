using MonoFarming.Util;

namespace MonoFarming.Entity.InventoryUtil {
    public class Item {

        public Sprite Sprite;
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string CanBreak { get; set; }
        public bool isStackable { get; set; }
        public bool displayItemDescription = false;

        public Item() { }

        public void SetToolProperties(string name, string desc, string canBreak) {

            this.Name = name;
            this.Description = desc;
            this.Type = "Tool";

            if (this.Type.Equals("Tool")) {

                this.CanBreak = canBreak;
            }
        }

        public void SetSeedProperties() {

        }

        public override string ToString() {

            if (this.Type.Equals("Tool")) return this.Name + " / " + this.CanBreak;

            else return this.Name;
        }
    }
}