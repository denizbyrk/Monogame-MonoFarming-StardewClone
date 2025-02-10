using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoFarming.Util;
using MonoFarming.Audio;
using MonoFarming.Scene;
using MonoFarming.Scene.Scenes;

namespace MonoFarming {
    public class Main : Game {

        public static bool debugMode = false;
        public static Texture2D debugTile;

        public static ContentManager contentManager;
        private SpriteBatch b;
        public const int targetTileSize = 16;
        public static Texture2D pixel;
        public static SpriteFont defaultFont;

        public static GraphicsDeviceManager graphics;
        public const int defaultScreenSize = 1;
        public static int currentScreenSize = 1;
        public static int[,] screenDimensions = { { 640, 360 }, { 1280, 720 }, { 1920, 1080 }, { 2560, 1440 }, { 3200, 1800 }, { 3840, 2160 } };
        private string gameTitle = "MonoFarm";

        private RenderTarget2D renderTarget;
        private Rectangle renderScaleRectangle;
        private float aspectRatio = screenDimensions[Main.defaultScreenSize, 0] / (float)screenDimensions[Main.defaultScreenSize, 1];

        public static Random random = new Random();
        public static bool exitGame = false;
        private Texture2D mouseCursor;
        
        public static SceneManager currentScene;
        public enum Scenes {

            Splash,
            Menu,
            Overworld
        }
        public static Scenes currentGameScene;

        public Main() {

            Main.graphics = new GraphicsDeviceManager(this);
            Main.graphics.SynchronizeWithVerticalRetrace = false;

            this.Window.AllowAltF4 = true;
            this.Window.AllowUserResizing = false;

            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true;
            this.IsMouseVisible = false;
        }

        protected override void Initialize() {

            this.Window.Title = this.gameTitle;

            Main.graphics.PreferredBackBufferWidth = Main.screenDimensions[defaultScreenSize, 0];
            Main.graphics.PreferredBackBufferHeight = Main.screenDimensions[defaultScreenSize, 1];
            Main.graphics.ApplyChanges();

            Main.contentManager = new ContentManager(this.Content.ServiceProvider, "Content");
            Main.defaultFont = Main.contentManager.Load<SpriteFont>("Font\\Font");

            this.renderTarget = new RenderTarget2D(graphics.GraphicsDevice, Main.screenDimensions[defaultScreenSize, 0], Main.screenDimensions[defaultScreenSize, 1], false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            this.renderScaleRectangle = GetScaleRectangle();

            Main.currentScene = new Overworld();
            Main.currentGameScene = Main.Scenes.Overworld;

            base.Initialize();
        }

        protected override void LoadContent() {

            this.b = new SpriteBatch(this.GraphicsDevice);

            Main.pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            Main.pixel.SetData(new Color[] { Color.White });

            Main.debugTile = Main.contentManager.Load<Texture2D>("Tiles\\collisionTile");
            this.mouseCursor = Main.contentManager.Load<Texture2D>("Sprites\\Mouse Cursor");

            Main.currentScene.LoadContent();
        }

        public static void LoadSceneContent(SceneManager s) {

            s.LoadContent();
        }

        //method for setting rendertarget rectangle
        private Rectangle GetScaleRectangle() {

            var variance = 0;
            var actualAspectRatio = this.Window.ClientBounds.Width / (float)this.Window.ClientBounds.Height;

            Rectangle scaleRectangle;

            if (actualAspectRatio <= this.aspectRatio) {

                var presentHeight = (int)(this.Window.ClientBounds.Width / this.aspectRatio + variance);
                var barHeight = (this.Window.ClientBounds.Height - presentHeight) / 2;

                scaleRectangle = new Rectangle(0, barHeight, this.Window.ClientBounds.Width, presentHeight);

            } else {

                var presentWidth = (int)(this.Window.ClientBounds.Height * this.aspectRatio + variance);
                var barWidth = (this.Window.ClientBounds.Width - presentWidth) / 2;

                scaleRectangle = new Rectangle(barWidth, 0, presentWidth, this.Window.ClientBounds.Height);
            }

            return scaleRectangle;
        }

        //method for changing screen resolution
        private void ChangeScreenResolution() {

            try {

                //check if the game is fullscreen
                if (Main.graphics.IsFullScreen == false) {

                    Main.graphics.PreferredBackBufferWidth = Main.screenDimensions[0 + Main.currentScreenSize, 0];
                    Main.graphics.PreferredBackBufferHeight = Main.screenDimensions[0 + Main.currentScreenSize, 1];
                }

            } catch (IndexOutOfRangeException) {

                throw new Exception("Out of bounds");

            } finally {

                Main.graphics.ApplyChanges();
                this.renderScaleRectangle = this.GetScaleRectangle();
            }
        }

        //method for getting next available screen resolution
        private int GetNextScreenResolution() {

            //check if the next screen dimension is bigger than the user's screen display size (i.e. if nextScreenSize = 2560x1440 and user's display size is 1920x1080, than set the size to 640)
            if (Main.screenDimensions[Main.currentScreenSize + 1, 0] > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) {

                Main.currentScreenSize = 0;

            } else if (!Main.graphics.IsFullScreen) {

                int xDim = Main.screenDimensions.GetLength(0) - 1;

                if (Main.currentScreenSize == xDim) {

                    Main.currentScreenSize = 0;

                } else Main.currentScreenSize++;

                return currentScreenSize;
            }

            return 1;
        }

        private void SetFullScreen() {

            if (Main.graphics.IsFullScreen == true) {

                Main.graphics.IsFullScreen = false;

                Main.currentScreenSize = 1;
                this.ChangeScreenResolution();

            } else {

                Main.graphics.IsFullScreen = true;

                Main.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Main.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                int screenDim = 0;

                for (int i = 0; i < Main.screenDimensions.Length - 1; i++) {

                    if (Main.screenDimensions[i, 0] == GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width) {

                        screenDim = i;

                        break;
                    }
                }

                Main.currentScreenSize = screenDim;

                Main.graphics.ApplyChanges();
            }
        }

        //update method
        protected override void Update(GameTime dt) {

            if (this.IsActive == true) {

                //unload the assets from content manager and exit game
                if (Main.exitGame == true) {

                    Main.contentManager.Unload();
                    this.Exit();

                } else {

                    Input.Update(); //update Inputs

                    SoundEffectAudio.Update(); //update audio

                    //change screen resolution
                    if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1) && Main.graphics.IsFullScreen == false) {

                        this.GetNextScreenResolution();
                        this.ChangeScreenResolution();
                    }

                    //switch to/from fullscreen
                    if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F2)) {

                        this.SetFullScreen();
                    }

                    //switch to/from debug mode
                    if (Input.IsKeyHold(Microsoft.Xna.Framework.Input.Keys.LeftControl)) {

                        if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P)) {

                            Main.debugMode = !Main.debugMode;
                        }
                    }

                    //update current scene
                    Main.currentScene.Update(dt);

                    //set camera follow to be instant exclusively for the first frame of the overworld state
                    if (Main.currentGameScene == Main.Scenes.Overworld) {

                        if (Overworld.Camera.firstFrame == true) {

                            Overworld.Camera.isInstant = false;
                            Overworld.Camera.firstFrame = false;
                        }
                    }

                    //set render target rectangle
                    this.renderScaleRectangle = this.GetScaleRectangle();
                }
            }

            base.Update(dt);
        }

        protected override void Draw(GameTime dt) {

            this.GraphicsDevice.SetRenderTarget(this.renderTarget);
            this.GraphicsDevice.Clear(Color.Black);

            //draw current scene
            Main.currentScene.Draw(this.b, dt); 

            this.GraphicsDevice.SetRenderTarget(null);
            this.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            this.b.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
            this.b.Draw(this.renderTarget, this.renderScaleRectangle, Color.White);
            this.b.End();

            //draw the mouse cursor
            this.b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            Helper.DrawMouseCursor(b, this.mouseCursor);
            this.b.End();

            base.Draw(dt);
        }
    }
}