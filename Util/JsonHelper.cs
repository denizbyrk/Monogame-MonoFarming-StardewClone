using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MonoFarming.Entity.InventoryUtil;

namespace MonoFarming.Util {
    public class JsonHelper {


        public static List<ItemData> ReadItemData(string path) {

            List<ItemData> itemData = new List<ItemData>();

            using (StreamReader reader = new StreamReader(path)) {

                string json = reader.ReadToEnd();
                itemData = JsonSerializer.Deserialize<List<ItemData>>(json);
            }

            return itemData;
        }

        public static List<ToolData> ReadToolData(string path) {

            List<ToolData> itemData = new List<ToolData>();

            using (StreamReader reader = new StreamReader(path)) {

                string json = reader.ReadToEnd();
                itemData = JsonSerializer.Deserialize<List<ToolData>>(json);
            }

            return itemData;
        }

        public static ItemData Deserialize1(string json) {

            ItemData itemData = JsonSerializer.Deserialize<ItemData>(json);

            return itemData;
        }

        public static ToolData DeserializeT(string json) {

            ToolData toolData = JsonSerializer.Deserialize<ToolData>(json);

            return toolData;
        }
    }
}