using Microsoft.Xna.Framework.Graphics;

namespace MonoFarming.Map.MapObjects {
    public class Stone : MapObject {

        public Stone(int spawnCount) {

            this.name = "Stone";
            this.id = 1;
            this.spawnCount = spawnCount;
        }

        public override int Spawn() {

            this.shapeCount = 2;
            this.hitPoints = 1;

            this.chosenShape = Main.random.Next(0, this.shapeCount);

            switch (this.chosenShape) {

                case 0:
                    return 5;

                case 1:
                    return 6;
            }

            return -1;
        }
    }
}