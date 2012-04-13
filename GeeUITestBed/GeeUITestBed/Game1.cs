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
            graphics.PreferMultiSampling = true;
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

            SpriteFont font = Content.Load<SpriteFont>("testFont");


            WindowView window = new WindowView(GeeUI.GeeUI.rootView, new Vector2(5, 5), font);
            window.windowText = "Switching a view's parent";

            PanelView panel2 = new PanelView(window, new Vector2(0, 0));
            panel2.width = 350;
            panel2.height = 400;

            WindowView secondWindow = new WindowView(GeeUI.GeeUI.rootView, new Vector2(400, 5), font);
            secondWindow.windowText = "Second window";

            PanelView panel3 = new PanelView(secondWindow, new Vector2(0, 0));
            panel3.width = 350;
            panel3.height = 400;

            WindowView childWindow = new WindowView(panel2, new Vector2(10, 10), font);
            childWindow.windowText = "Child window";

            PanelView childPanel = new PanelView(childWindow, new Vector2(0, 0));
            childPanel.width = 210;
            childPanel.height = 150;

            TextFieldView textField = new TextFieldView(childPanel, new Vector2(0, 0), font);
            textField.width = 203;
            textField.height = 100;


            ButtonView switchingButton = new ButtonView(childPanel, "Switch parents", new Vector2(0, 110), font);

            switchingButton.onMouseClick += new View.MouseClickEventHandler((object sender, EventArgs e) =>
            {
                if (childWindow.parentView == panel2)
                    childWindow.setParent(panel3);
                else
                    childWindow.setParent(panel2);
            });


            /*CheckBoxView check = new CheckBoxView(childPanel, new Vector2(0, 135), "Enable button", font);
            check.onMouseClick += new View.MouseClickEventHandler((object sender, EventArgs e) =>
            {
                switchingButton.active = check.isChecked;
            });*/

            SliderView slider = new SliderView(childPanel, new Vector2(0, 135), 0, 10);
            slider.width = 100;

            slider.onSliderValueChanged += new SliderView.SliderValueChangedHandler((object sender, EventArgs e) =>
            {
                textField.text = slider.currentValue.ToString();
            });

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

            GeeUI.GeeUI.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
