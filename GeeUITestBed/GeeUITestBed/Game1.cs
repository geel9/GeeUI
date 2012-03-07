using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GeeUI;
using GeeUI.Views;
namespace GeeUITestBed
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GeeUI.GeeUI.Initialize(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //View parentView = new View(GeeUI.GeeUI.rootView);
            //parentView.position = new Vector2(0, 0);
            //parentView.width = parentView.height = 200;

            SpriteFont font = Content.Load<SpriteFont>("testFont");

            ButtonView button_depth0_container = new ButtonView(GeeUI.GeeUI.rootView, "HelloHelloHelloHelloHelloHelloHelloHelloHello", new Vector2(10, 10), font);
            button_depth0_container.height = 400;

            ButtonView button_depth1_container = new ButtonView(button_depth0_container, "Hello4544542545423542355", new Vector2(20, 20), font);
            button_depth1_container.height = 200;

            ButtonView button_depth1_otherContainer = new ButtonView(button_depth0_container, "Hello454455", new Vector2(270, 10), font);
            button_depth1_otherContainer.height = 200;

            ButtonView button_depth2 = new ButtonView(button_depth1_container, "H", new Vector2(20, 20), font);
            button_depth2.height = 60;

            ButtonView button_depth2_inside_othercontainer = new ButtonView(button_depth1_otherContainer, "Hello!", new Vector2(10, 10), font);
            button_depth2_inside_othercontainer.height = 30;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GeeUI.GeeUI.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // TODO: Add your drawing code here
            GeeUI.GeeUI.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
