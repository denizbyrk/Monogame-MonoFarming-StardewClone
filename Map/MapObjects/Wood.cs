using Microsoft.Xna.Framework.Graphics;

namespace MonoFarming.Map.MapObjects {
    public class Wood : MapObject {

        public Wood(int spawnCount) {

            this.name = "Wood";
            this.id = 0;
            this.spawnCount = spawnCount;
        }

        public override int Spawn() {

            this.shapeCount = 2;
            this.hitPoints = 1;

            this.chosenShape = Main.random.Next(0, this.shapeCount);

            switch (this.chosenShape) {

                case 0:
                    return 3;

                case 1:
                    return 4;
            }

            return -1;
        }
    }
}