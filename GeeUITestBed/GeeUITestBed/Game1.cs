using GeeUI.ViewLayouts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GeeUI.Views;
namespace GeeUITestBed
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferMultiSampling = true;
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var font = Content.Load<SpriteFont>("testFont");

            /*  var north = new PanelView(GeeUI.GeeUI.RootView, Vector2.Zero);
              var south = new ButtonView(GeeUI.GeeUI.RootView, "South", new Vector2(10, 10), font);
              var east = new ButtonView(GeeUI.GeeUI.RootView, "EastEastEastEastEastEast", new Vector2(10, 10), font);
              var west = new PanelView(GeeUI.GeeUI.RootView, Vector2.Zero);
              var middle = new ButtonView(GeeUI.GeeUI.RootView, "Middle", new Vector2(10, 10), font);
              GeeUI.GeeUI.RootView.ChildrenLayout = new BorderViewLayout(north, south, west, east, middle, 20, 20);*/


           // var window = new WindowView(GeeUI.GeeUI.RootView, new Vector2(5, 5), font) { WindowText = "Tab Views", Width = 500, Height = 400 };
            var panel = new PanelView(GeeUI.GeeUI.RootView, new Vector2(5, 5)) { Width = 600, Height = 400 };

            var tabs = new TabHost(panel, new Vector2(0, 0), font) { Width = 570, Height = 300 };

            var panel1 = new PanelView(null, Vector2.Zero);
            var panel2 = new PanelView(null, Vector2.Zero);

            tabs.AddTab("Tab 1", panel1);
            tabs.AddTab("Tab 2", panel2);

            for (int i = 0; i < 10; i++)
            {
                tabs.AddTab("Tab " + (3 + i), new PanelView(null, Vector2.Zero));
            }

            new TextFieldView(panel1, Vector2.Zero, font) { Text = "This is tab 1.", Width = 380, Height = 230 };

            for (int i = 0; i < 50; i++)
            {
                var button = new ButtonView(panel2, "Button" + (i + 1), Vector2.Zero, font);
                button.Width = 70;
                button.OnMouseClick += (sender, e) => panel2.RemoveChild(button);
            }

            ButtonView switchLayouts = new ButtonView(panel, "Switch to Horizontal Layout", Vector2.Zero, font);
            switchLayouts.OnMouseClick += (sender, e) =>
                                              {
                                                  if (panel2.ChildrenLayout is VerticalViewLayout)
                                                  {
                                                      panel2.ChildrenLayout = new HorizontalViewLayout(2, true);
                                                      switchLayouts.Text = "Switch to Vertical Layout";
                                                  }
                                                  else if (panel2.ChildrenLayout is HorizontalViewLayout)
                                                  {
                                                      panel2.ChildrenLayout = new VerticalViewLayout(2, true);
                                                      switchLayouts.Text = "Switch to Horizontal Layout";
                                                  }
                                              };
            panel.ChildrenLayout = new VerticalViewLayout(4, true);
            panel2.ChildrenLayout = new VerticalViewLayout(2, true);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
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
                Exit();

            GeeUI.GeeUI.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            GeeUI.GeeUI.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
