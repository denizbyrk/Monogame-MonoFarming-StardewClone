using System.Collections.Generic;

namespace MonoFarming.Entity.InventoryUtil {
    public class ToolData {

        public static List<ToolData> Data = new List<ToolData>();

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isStackable { get; set; }
        public string canBreak { get; set; }
    }
}