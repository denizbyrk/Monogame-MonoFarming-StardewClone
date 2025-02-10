using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoFarming.Map.MapObjects;
using System.Threading;
using MonoFarming.Scene.Scenes;

namespace MonoFarming.Map.Maps {
    public class ForestMap : MapLoader {

        private Texture2D tileMap;
        private Dictionary<Tile, int> bg;
        private Dictionary<Tile, int> tillable;
        private Dictionary<Tile, int> obj;
        private Dictionary<Tile, int> hoed;
        private Dictionary<Tile, int> cl;
        private Dictionary<Tile, int> clDetails;
        private Dictionary<Tile, int> front;
        private Dictionary<Tile, int> collisions;

        private MapObject Wood;
        private MapObject Stone;
        private MapObject Fiber;

        public ForestMap() {

            this.name = "Forest";
            this.defaultX = 31;
            this.defaultY = 0;
            this.X = 64;
            this.Y = 40;
            this.boundaryY = 80;
            this.isInterior = false;
            this.maxZoomLevel = 1.25f;

            this.tileMap = Main.contentManager.Load<Texture2D>("Tiles\\Tileset");
            this.tilesetColumnCount = this.tileMap.Width / Main.targetTileSize;

            string path = "Content\\Map Data\\Forest\\forest_";

            this.bg = this.LoadLayer(path + "Background.csv");
            this.tillable = this.LoadLayer(path + "Tillable.csv");
            this.obj = this.LoadLayer(path + "Objects.csv");
            this.cl = this.LoadLayer(path + "Cliffs.csv");
            this.clDetails = this.LoadLayer(path + "CliffsDetails.csv");
            this.front = this.LoadLayer(path + "Front.csv");
            this.collisions = this.LoadLayer(path + "Collisions.csv");

            this.collisionTiles = this.collisions;

            this.transitionTiles.Add(new Vector2(31, -1));
            this.transitionTiles.Add(new Vector2(32, -1));

            this.Wood = new Wood(80);
            this.Stone = new Stone(80);
            this.Fiber = new Fiber(100);

            this.spawnInteractableObjects(this.tillable, this.Wood);
            this.spawnInteractableObjects(this.tillable, this.Stone);
            this.spawnInteractableObjects(this.tillable, this.Fiber);

            this.hoed = new Dictionary<Tile, int>();
        }

        public override void DrawMap(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.bg, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.tillable, this.tilesetColumnCount);

            this.DrawMapObject(b, this.interactableTiles);

            this.DrawLayer(b, this.tileMap, this.obj, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.clDetails, this.tilesetColumnCount);

            this.DrawCollisions(b, this.collisions);
        }

        public override void DrawMapFront(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.front, this.tilesetColumnCount);
        }
    }
}