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

        public static Texture2D agop;

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
           /* agop = Content.Load<Texture2D>("agop");

            for(int i = 0; i < 35; i++)
            {
                var panel = new View(GeeUI.GeeUI.RootView) {Width = 300, Height = 300};
                for(int j = 0; j < 7; j++)
                {
                    var newImage = new ImageView(panel, agop);
                    newImage.ScaleVector = new Vector2(.75f);
                }
                panel.ChildrenLayout = new SpinViewLayout(130, 1.5f);
            }

            GeeUI.GeeUI.RootView.ChildrenLayout = new SpinViewLayout(350, -1.5f);

            */
            var panel = new PanelView(GeeUI.GeeUI.RootView, new Vector2(5, 5)) { Width = 600, Height = 400 };
            
            var tabs = new TabHost(panel, new Vector2(0, 0), font) { Width = 570, Height = 300 };

            var panel1 = new PanelView(null, Vector2.Zero);
            var panel2 = new PanelView(null, Vector2.Zero);

            tabs.AddTab("Tab 1", panel1);
            tabs.AddTab("Tab 2", panel2);
            new TextFieldView(panel1, Vector2.Zero, font) { Text = "This is tab 1.", Width = 380, Height = 230 };

            for (int i = 0; i < 50; i++)
            {
                var button = new ButtonView(panel2, "Button" + (i + 1), Vector2.Zero, font);
                button.Width = 70;
                button.OnMouseClick += (sender, e) => panel2.RemoveChild(button);
            }

            ButtonView switchLayouts = new ButtonView(panel, "Switch to Spinning Layout", Vector2.Zero, font);
            switchLayouts.OnMouseClick += (sender, e) =>
                                              {
                                                  if (panel2.ChildrenLayout is VerticalViewLayout)
                                                  {
                                                      panel2.ChildrenLayout = new SpinViewLayout(115);
                                                      switchLayouts.Text = "Switch to Vertical Layout";
                                                  }
                                                  else if (panel2.ChildrenLayout is SpinViewLayout)
                                                  {
                                                      panel2.ChildrenLayout = new VerticalViewLayout(2, true);
                                                      switchLayouts.Text = "Switch to Spinning Layout";
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
