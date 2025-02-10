using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFarming.Map {
    public class Tile {

        public int ID;
        public Vector2 tilePosition;
        public Rectangle tileBounds;
        public SpriteEffects tileFlip;
        public MapObject objectOnTile;
        public bool hasObject = false;

        public Tile(Vector2 tilePosition) {

            this.tilePosition = tilePosition;
            this.tileBounds = new Rectangle((int)this.tilePosition.X * Main.targetTileSize, (int)this.tilePosition.Y * Main.targetTileSize, Main.targetTileSize, Main.targetTileSize);
        
            if (this.objectOnTile != null ) this.hasObject = true;
        }
    }
}