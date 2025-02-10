using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoFarming.Map.MapObjects;
using System.Collections.Generic;

namespace MonoFarming.Map.Maps {
    public class LakeMap : MapLoader {

        private Texture2D tileMap;
        private Dictionary<Tile, int> bg;
        private Dictionary<Tile, int> tillable;
        private Dictionary<Tile, int> obj;
        private Dictionary<Tile, int> water;
        private Dictionary<Tile, int> front;
        private Dictionary<Tile, int> collisions;

        private MapObject Wood;
        private MapObject Stone;
        private MapObject Fiber;

        public LakeMap() {

            this.name = "Lake";
            this.defaultX = 0;
            this.defaultY = 14;
            this.X = 50;
            this.Y = 28;
            this.isInterior = false;
            this.maxZoomLevel = 1.6f;

            this.tileMap = Main.contentManager.Load<Texture2D>("Tiles\\Tileset");
            this.tilesetColumnCount = this.tileMap.Width / Main.targetTileSize;

            string path = "Content\\Map Data\\Lake\\lake_";

            this.bg = this.LoadLayer(path + "Background.csv");
            this.tillable = this.LoadLayer(path + "Tillable.csv");
            this.water = this.LoadLayer(path + "Water.csv");
            this.obj = this.LoadLayer(path + "Objects.csv");
            this.front = this.LoadLayer(path + "Front.csv");
            this.collisions = this.LoadLayer(path + "Collisions.csv");

            this.collisionTiles = this.collisions;

            this.transitionTiles.Add(new Vector2(-1, 11));
            this.transitionTiles.Add(new Vector2(-1, 12));
            this.transitionTiles.Add(new Vector2(-1, 13));
            this.transitionTiles.Add(new Vector2(-1, 14));
            this.transitionTiles.Add(new Vector2(-1, 15));
            this.transitionTiles.Add(new Vector2(-1, 16));

            this.Wood = new Wood(40);
            this.Stone = new Stone(40);
            this.Fiber = new Fiber(60);

            this.spawnInteractableObjects(this.tillable, this.Wood);
            this.spawnInteractableObjects(this.tillable, this.Stone);
            this.spawnInteractableObjects(this.tillable, this.Fiber);
        }

        public override void DrawMap(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.bg, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.water, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.tillable, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.obj, this.tilesetColumnCount);

            this.DrawMapObject(b, this.interactableTiles);

            this.DrawCollisions(b, this.collisions);
        }

        public override void DrawMapFront(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.front, this.tilesetColumnCount);
        }
    }
}