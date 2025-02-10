using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoFarming.Scene {
    public class SceneManager {

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime dt) { }

        public virtual void Draw(SpriteBatch b, GameTime dt) { }
    }
}