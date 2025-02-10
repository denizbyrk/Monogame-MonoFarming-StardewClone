using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoFarming.Map.Maps {
    public class MountainMap : MapLoader {

        private Texture2D tileMap;
        private Dictionary<Tile, int> bg;
        private Dictionary<Tile, int> cl1;
        private Dictionary<Tile, int> cl2;
        private Dictionary<Tile, int> cl3;
        private Dictionary<Tile, int> cl4;
        private Dictionary<Tile, int> cl5;
        private Dictionary<Tile, int> cl6;
        private Dictionary<Tile, int> collisions;

        public MountainMap() {

            this.name = "Mountain";
            this.defaultX = 16;
            this.defaultY = 66;
            this.X = 120;
            this.Y = 67;
            this.boundaryX = 240;
            this.boundaryY = 134;
            this.isInterior = false;
            this.maxZoomLevel = 1f;

            this.tileMap = Main.contentManager.Load<Texture2D>("Tiles\\Tileset");
            this.tilesetColumnCount = this.tileMap.Width / Main.targetTileSize;

            string path = "Content\\Map Data\\Mountain\\mountain_";

            this.bg = this.LoadLayer(path + "Background.csv");
            this.cl1 = this.LoadLayer(path + "Cliff1.csv");
            this.cl2 = this.LoadLayer(path + "Cliff2.csv");
            this.cl3 = this.LoadLayer(path + "Cliff3.csv");
            this.cl4 = this.LoadLayer(path + "Cliff4.csv");
            this.cl5 = this.LoadLayer(path + "Cliff5.csv");
            this.cl6 = this.LoadLayer(path + "Cliff6.csv");
            this.collisions = this.LoadLayer(path + "Collisions.csv");

            this.collisionTiles = this.collisions;

            this.transitionTiles.Add(new Vector2(14, 67));
            this.transitionTiles.Add(new Vector2(15, 67));
            this.transitionTiles.Add(new Vector2(16, 67));
            this.transitionTiles.Add(new Vector2(17, 67));

            this.objectTileset = Main.contentManager.Load<Texture2D>("Tiles\\Ground Objects");
            this.objectTilesetColumnCount = this.objectTileset.Width / Main.targetTileSize;
        }

        public override void DrawMap(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.bg, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl1, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl2, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl3, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl4, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl5, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl6, this.tilesetColumnCount);

            this.DrawCollisions(b, this.collisions);
        }

        public override void DrawMapFront(SpriteBatch b) {

        }
    }
}