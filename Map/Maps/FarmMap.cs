using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using MonoFarming.Map.MapObjects;
using MonoFarming.Entity;

namespace MonoFarming.Map.Maps {
    public class FarmMap : MapLoader {

        private Texture2D tileMap;
        private Dictionary<Tile, int> bg;
        private Dictionary<Tile, int> tillable;
        private Dictionary<Tile, int> obj;
        private Dictionary<Tile, int> water;
        private Dictionary<Tile, int> cl;
        private Dictionary<Tile, int> cl2;
        private Dictionary<Tile, int> clDetails;
        private Dictionary<Tile, int> front;
        private Dictionary<Tile, int> collisions;

        private MapObject Wood;
        private MapObject Stone;
        private MapObject Fiber;

        public NPC FarmVillager;

        public FarmMap() {

            this.name = "Farm";
            this.defaultX = 63;
            this.defaultY = 14;
            this.X = 80;
            this.Y = 45;
            this.isInterior = false;
            this.maxZoomLevel = 1f;

            this.tileMap = Main.contentManager.Load<Texture2D>("Tiles\\Tileset");
            this.tilesetColumnCount = this.tileMap.Width / Main.targetTileSize;

            string path = "Content\\Map Data\\Farm\\farmMap_";

            this.bg = this.LoadLayer(path + "Background.csv");
            this.tillable = this.LoadLayer(path + "Tillable.csv");
            this.obj = this.LoadLayer(path + "Objects.csv");
            this.water = this.LoadLayer(path + "Water.csv");
            this.cl = this.LoadLayer(path + "Cliffs.csv");
            this.cl2 = this.LoadLayer(path + "Cliffs2.csv");
            this.clDetails = this.LoadLayer(path + "CliffsDetails.csv");
            this.front = this.LoadLayer(path + "Front.csv");
            this.collisions = this.LoadLayer(path + "Collisions.csv");

            this.collisionTiles = this.collisions;

            this.transitionTiles.Add(new Vector2(80, 11));
            this.transitionTiles.Add(new Vector2(80, 12));
            this.transitionTiles.Add(new Vector2(80, 13));

            this.transitionTiles.Add(new Vector2(40, -1));
            this.transitionTiles.Add(new Vector2(41, -1));

            this.transitionTiles.Add(new Vector2(41, 45));
            this.transitionTiles.Add(new Vector2(42, 45));
            this.transitionTiles.Add(new Vector2(43, 45));
            this.transitionTiles.Add(new Vector2(44, 45));

            this.transitionTiles.Add(new Vector2(63, 12));

            this.Wood = new Wood(180);
            this.Stone = new Stone(160);
            this.Fiber = new Fiber(140);

            this.spawnInteractableObjects(this.tillable, this.Wood);
            this.spawnInteractableObjects(this.tillable, this.Stone);
            this.spawnInteractableObjects(this.tillable, this.Fiber);

            this.FarmVillager = new Villager();
        }

        public override void UpdateMap(GameTime dt) {

            if (this.FarmVillager.Indicator.drawIndicator == true) this.FarmVillager.Update(dt);
        }

        public override void DrawMap(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.bg, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.tillable, this.tilesetColumnCount);

            this.DrawMapObject(b, this.interactableTiles);

            this.DrawLayer(b, this.tileMap, this.water, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.obj, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.cl2, this.tilesetColumnCount);

            this.DrawLayer(b, this.tileMap, this.clDetails, this.tilesetColumnCount);

            this.DrawCollisions(b, this.collisionTiles);
        }

        public override void DrawNPC(SpriteBatch b) {

            this.FarmVillager.Draw(b);
        }

        public override void DrawMapFront(SpriteBatch b) {

            this.DrawLayer(b, this.tileMap, this.front, this.tilesetColumnCount);
        }

        public List<Vector2> getTransitionTiles() => this.transitionTiles;

        public void setSpawningX(int X) => this.defaultX = X;

        public void setSpawningY(int Y) => this.defaultY = Y;

        public void DrawIndicator(bool draw) => this.FarmVillager.Indicator.drawIndicator = draw;

        public Vector2 getVillagerPosition() => new Vector2(this.FarmVillager.Sprite.Position.X + 4, this.FarmVillager.Sprite.Position.Y + 8);
    }
}