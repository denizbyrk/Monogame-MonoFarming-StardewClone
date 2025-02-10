using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoFarming.Map.Maps {
    public class HouseMap : MapLoader {

        private Texture2D tileMap;
        private Dictionary<Tile, int> floor;
        private Dictionary<Tile, int> wall;
        private Dictionary<Tile, int> obj;
        private Dictionary<Tile, int> obj2;
        private Dictionary<Tile, int> collisions;

        public HouseMap() {

            this.name = "House";
            this.defaultX = 8;
            this.defaultY = 13;
            this.X = 26;
            this.Y = 14;
            this.isInterior = true;
            this.maxZoomLevel = 1f;

            this.tileMap = Main.contentManager.Load<Texture2D>("Tiles\\Interior");
            this.tilesetColumnCount = this.tileMap.Width / Main.targetTileSize;

            string path = "Content\\Map Data\\House\\house_";

            this.floor = this.LoadLayer(path + "Floor.csv");
            this.wall = this.LoadLayer(path + "Walls.csv");
            this.obj = this.LoadLayer(path + "Decorations.csv");
            this.obj2 = this.LoadLayer(path + "DecorationsFront.csv");
            this.collisions = this.LoadLayer(path + "Collisions.csv");

            this.collisionTiles = this.collisions;

            this.transitionTiles.Add(new Vector2(8, 14));
        }

        public override void DrawMap(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.floor, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.wall, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.obj, this.tilesetColumnCount);

            this.DrawCollisions(b, this.collisions);
        }

        public override void DrawMapFront(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.obj2, this.tilesetColumnCount);
        }
    }
}