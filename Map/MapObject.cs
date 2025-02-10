using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoFarming.Map {
    public class MapObject {

        public string name;
        public int id;
        public int tilesetPosition;
        public int shapeCount;
        public int chosenShape;
        public int spawnCount;

        public int hitPoints;
        public float dropSpread;
        public int dropCount;

        public virtual int Spawn() => this.id;

        public virtual void Destroy() { }
    }
}