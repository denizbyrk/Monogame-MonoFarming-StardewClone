using System.Collections.Generic;

namespace MonoFarming.Entity.InventoryUtil {
    public class ItemData {

        public static List<ItemData> Data = new List<ItemData>();

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public bool isStackable { get; set; }
    }
}