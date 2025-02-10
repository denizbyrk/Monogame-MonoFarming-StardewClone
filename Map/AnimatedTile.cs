using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace MonoFarming.Map {
    public class AnimatedTile {
 
        public string TileName;
        public List<Tile> tileList;

        public AnimatedTile(Vector2 tilePosition) {

            
        }

        public void LoadLayer(string filepath, string line, int y) {

            Dictionary<AnimatedTile, int> mapData = new Dictionary<AnimatedTile, int>();

            StreamReader reader = new StreamReader(filepath);

            while ((line = reader.ReadLine()) != null) {

                string[] tiles = line.Split(",");

                for (int x = 0; x < tiles.Length; x++) {

                    if (int.TryParse(tiles[x], out int value)) {

                        if (value > -1) {


                        }
                    }
                }

                y++;
            }
        }
    }
}