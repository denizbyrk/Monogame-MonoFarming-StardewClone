using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using MonoFarming.Util;
using MonoFarming.Entity;
using MonoFarming.Scene.Scenes;

namespace MonoFarming.Map {
    public class MapLoader {

        public string name;
        public int defaultX;
        public int defaultY;
        public int X;
        public int Y;
        public int boundaryX;
        public int boundaryY;
        public bool isInterior;
        public float maxZoomLevel;
        public int tilesetColumnCount;
        public int objectTilesetColumnCount;
        public Dictionary<Tile, int> collisionTiles = new Dictionary<Tile, int>();
        public Dictionary<Tile, int> interactableTiles = new Dictionary<Tile, int>();
        public Dictionary<Tile, int> tillableTiles = new Dictionary<Tile, int>();
        public Dictionary<Tile, int> hoedTiles = new Dictionary<Tile, int>();
        private Dictionary<Vector2, Tile> occupiedTiles = new Dictionary<Vector2, Tile>();
        public List<Particle> Particles = new List<Particle>();
        public List<ItemDrop> DroppedItems = new List<ItemDrop>();
        public List<Vector2> transitionTiles = new List<Vector2>();
        public Texture2D objectTileset = Main.contentManager.Load<Texture2D>("Tiles\\Ground Objects");
        public List<MapObject> objects = new List<MapObject>();

        protected Dictionary<Tile, int> LoadLayer(string path) {

            Dictionary<Tile, int> layerData = new Dictionary<Tile, int>();

            StreamReader reader = new StreamReader(path);

            int y = 0;
            string line;

            while ((line = reader.ReadLine()) != null) {

                string[] tiles = line.Split(",");

                for (int x = 0; x < tiles.Length; x++) {

                    if (int.TryParse(tiles[x], out int tileID)) {

                        if (tileID > -1) {

                            Tile t = new Tile(new Vector2(x, y));
                            t.ID = tileID;

                            layerData[t] = tileID;
                        }
                    }
                }

                y++;
            }

            return layerData;
        }

        public virtual void UpdateMap(GameTime dt) { }

        public virtual void DrawMap(SpriteBatch b) { }

        public virtual void DrawMapFront(SpriteBatch b) { }

        public virtual void DrawNPC(SpriteBatch b) { }

        protected void DrawLayer(SpriteBatch b, Texture2D tileMap, Dictionary<Tile, int> t, int tilesetColumnCount) {

            foreach (var r in t) {

                Rectangle dest = new Rectangle(((int)r.Key.tilePosition.X * (Main.targetTileSize * 1)),
                                               ((int)r.Key.tilePosition.Y * (Main.targetTileSize * 1)),
                                               Main.targetTileSize * 1,
                                               Main.targetTileSize * 1);

                int x = r.Value % tilesetColumnCount;
                int y = r.Value / tilesetColumnCount;

                Rectangle src = new Rectangle(x * Main.targetTileSize, y * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize);

                b.Draw(tileMap, dest, src, Color.White);
            }
        }

        protected void DrawHoedTiles(SpriteBatch b, Texture2D tilemap, Dictionary<Tile, int> t, int tilesetColumnCount) {

            this.hoedTiles = t;

            foreach (var r in t) {

                Rectangle dest = new Rectangle(((int)r.Key.tilePosition.X * Main.targetTileSize),
                    ((int)r.Key.tilePosition.Y * Main.targetTileSize), Main.targetTileSize, Main.targetTileSize);

                int x = r.Value % tilesetColumnCount;
                int y = r.Value / tilesetColumnCount;

                Rectangle src = new Rectangle(x * Main.targetTileSize, y * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize);

                b.Draw(tilemap, dest, src, Color.White, 0f, Vector2.Zero, r.Key.tileFlip, 0f);
            }
        }

        public virtual void DrawMapObject(SpriteBatch b, Dictionary<Tile, int> t) {

            this.objectTilesetColumnCount = this.objectTileset.Width / Main.targetTileSize;

            foreach (var r in t) {

                Rectangle dest = new Rectangle(((int)r.Key.tilePosition.X * Main.targetTileSize),
                    ((int)r.Key.tilePosition.Y * Main.targetTileSize), Main.targetTileSize, Main.targetTileSize);

                int x = r.Value % this.objectTilesetColumnCount;
                int y = r.Value / this.objectTilesetColumnCount;

                Rectangle src = new Rectangle(x * Main.targetTileSize, y * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize);

                b.Draw(this.objectTileset, dest, src, Color.White, 0f, Vector2.Zero, r.Key.tileFlip, 0f);
            }
        }

        protected void DrawCollisions(SpriteBatch b, Dictionary<Tile, int> collisions) {

            if (Main.debugMode == true) {

                foreach (var r in collisions) {

                    Rectangle dest = new Rectangle(((int)r.Key.tilePosition.X * Main.targetTileSize),
                        (int)r.Key.tilePosition.Y * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize);

                    int x = r.Value;
                    int y = r.Value;

                    Rectangle src = new Rectangle(x * Main.targetTileSize, y * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize);

                    b.Draw(Main.debugTile, dest, src, Color.White);
                }
            }
        }

        protected void spawnInteractableObjects(Dictionary<Tile, int> spawnOnTiles, MapObject mapObject) {

            Dictionary<Tile, int> temp = new Dictionary<Tile, int>();
            temp = spawnOnTiles;

            tillableTiles = temp;

            int itemID = -1;

            Dictionary<Tile, int> chosenTiles = new Dictionary<Tile, int>();

            while (chosenTiles.Count < mapObject.spawnCount) {

                int randomIndex = Main.random.Next(spawnOnTiles.Count);
                Tile key = spawnOnTiles.Keys.ElementAt(randomIndex);

                itemID = mapObject.Spawn();

                if (!this.occupiedTiles.ContainsKey(key.tilePosition) && !chosenTiles.ContainsKey(key)) {

                    chosenTiles.Add(key, itemID);

                    this.occupiedTiles[key.tilePosition] = key;

                } else {

                    System.Diagnostics.Debug.WriteLine("The objects spawned on the same tile.");
                }
            }

            foreach (var tile in chosenTiles) {

                try {

                    Tile t = new Tile((new Vector2(tile.Key.tilePosition.X, tile.Key.tilePosition.Y)));
                    int randomFlip = Main.random.Next(2);
                    t.tileFlip = randomFlip == 0 ? t.tileFlip = SpriteEffects.FlipHorizontally : randomFlip == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
                    t.objectOnTile = mapObject;

                    this.objects.Add(mapObject);
                    this.collisionTiles.Add(t, 0);
                    this.interactableTiles.Add(t, tile.Value);

                } catch (ArgumentException) {

                    System.Diagnostics.Debug.WriteLine("An item with the same key has already been added. Key: X: " + tile.Key.tilePosition.X + ", Y: " + tile.Key.tilePosition.Y);
                }
            }
        }

        public void destroyInteractableObjects(KeyValuePair<Tile, int> tileToDestroy, MapObject mapObject) {

            this.objects.Remove(mapObject);
            this.dropObjectItem(mapObject, new Vector2(tileToDestroy.Key.tilePosition.X * Main.targetTileSize, tileToDestroy.Key.tilePosition.Y * Main.targetTileSize));
            this.collisionTiles.Remove(tileToDestroy.Key);
            this.interactableTiles.Remove(tileToDestroy.Key);
        }

        public void dropObjectItem(MapObject mapObject, Vector2 position) {

            ItemDrop drop = new ItemDrop(mapObject.id, position);

            Overworld.currentMap.DroppedItems.Add(drop);
        }
    }
}