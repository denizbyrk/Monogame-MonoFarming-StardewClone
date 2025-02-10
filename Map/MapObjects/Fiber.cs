using Microsoft.Xna.Framework.Graphics;

namespace MonoFarming.Map.MapObjects {
    public class Fiber : MapObject {

        public Fiber(int spawnCount) {

            this.name = "Fiber";
            this.id = 2;
            this.spawnCount = spawnCount;
        }

        public override int Spawn() {

            this.shapeCount = 3;
            this.hitPoints = 1;

            this.chosenShape = Main.random.Next(0, this.shapeCount);

            switch (this.chosenShape) {

                case 0:
                    return 0;

                case 1:
                    return 1;

                case 2:
                    return 2;
            }

            return -1;
        }
    }
}