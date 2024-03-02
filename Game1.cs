using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ShootingGame.Scripts;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace ShootingGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardManager keyboardManager;

        int virtualGameWidth = 1080;
        int virtualGameHeight = 720;
        RenderTarget2D virtualRenderTarget;
        Rectangle renderTargetDestination;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.HardwareModeSwitch = false;

            graphics.PreferredBackBufferWidth = virtualGameWidth;
            graphics.PreferredBackBufferHeight = virtualGameHeight;
        }

        protected override void Initialize()
        {
            virtualRenderTarget = new RenderTarget2D(GraphicsDevice, virtualGameWidth, virtualGameHeight);
            renderTargetDestination = GetRenderTargetDestination(new Point(virtualGameWidth, virtualGameHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            keyboardManager = new KeyboardManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            ManageInput();


            // TODO: Add your update logic here


            base.Update(gameTime);
        }


        private void ManageInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            keyboardManager.UpdateState();


            if (gamepadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardManager.HasBeenPressed(Keys.F11) || keyboardManager.HasBeenPressed(Keys.F4))
                ToggleFullScreen();
        }


        private void DrawSceneToTexture(RenderTarget2D renderTarget)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);



            // Put drawing code here.



            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
        }





        protected override void Draw(GameTime gameTime)
        {
            DrawSceneToTexture(virtualRenderTarget);


            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);


            spriteBatch.Draw(virtualRenderTarget, renderTargetDestination, Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }


        void ToggleFullScreen()
        {
            if (!graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = virtualGameWidth;
                graphics.PreferredBackBufferHeight = virtualGameHeight;
            }
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();

            renderTargetDestination = GetRenderTargetDestination(new Point(virtualGameWidth, virtualGameHeight), graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }


        Rectangle GetRenderTargetDestination(Point resolution, int preferredBackBufferWidth, int preferredBackBufferHeight)
        {
            float resolutionRatio = (float)resolution.X / resolution.Y;
            float screenRatio;
            Point bounds = new Point(preferredBackBufferWidth, preferredBackBufferHeight);
            screenRatio = (float)bounds.X / bounds.Y;
            float scale;
            Rectangle rectangle = new Rectangle();

            if (resolutionRatio < screenRatio)
                scale = (float)bounds.Y / resolution.Y;
            else if (resolutionRatio > screenRatio)
                scale = (float)bounds.X / resolution.X;
            else
            {
                // Resolution and window/screen share aspect ratio
                rectangle.Size = bounds;
                return rectangle;
            }
            rectangle.Width = (int)(resolution.X * scale);
            rectangle.Height = (int)(resolution.Y * scale);
            return CenterRectangle(new Rectangle(Point.Zero, bounds), rectangle);
        }

        static Rectangle CenterRectangle(Rectangle outerRectangle, Rectangle innerRectangle)
        {
            Point delta = outerRectangle.Center - innerRectangle.Center;
            innerRectangle.Offset(delta);
            return innerRectangle;
        }
    }
}
