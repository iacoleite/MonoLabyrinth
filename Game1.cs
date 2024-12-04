using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;


namespace MovingRectangleGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Variabili per il rettangolo
        private Rectangle _player;
        private int _playerSpeed = 5;
        private int _maxSpeed = 10;

        // Texture per il rettangolo del giocatore
        private Texture2D _playerTexture;
        private Texture2D CheeseTexture;

        private Rectangle cheese;
        // Ostacoli
        private Rectangle _exit;
        private Texture2D _obstacleTexture;

        private Texture2D ExitTexture;
        private Texture2D PlayerTexture;
        private SpriteEffects PlayerTextureFX = SpriteEffects.None;
        public static Random rand = new Random();

        private Texture2D ObstacleTexture;
        private float rotationAngle = 0;
        // private Vector2 spriteOrigin;
        private string lastHorizontalDirection = "right";
        private string lastVerticalDirection = "up";

        public List<Rectangle> obs = new List<Rectangle>();
        public List<Rectangle> cheeseArray = new List<Rectangle>();
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            // Inizializzazione del rettangolo del giocatore

            // Random random= new Random();

            // Inizializzazione dell'ostacolo
            // int _nOstacolli = 50;

            int screenWidth = _graphics.PreferredBackBufferWidth - 100;
            int screenHeight = _graphics.PreferredBackBufferHeight - 100;
            int gridWidth = Math.Abs(screenWidth / 43);
            int gridHeight = Math.Abs(screenHeight / 39);

            Console.WriteLine(screenHeight);
            Console.WriteLine(screenWidth);
            Console.WriteLine(gridHeight);
            Console.WriteLine(gridWidth);
            // MazeGenerator mazeGenerator = new MazeGenerator();

            // mazeGenerator.GenerateMaze();
            // int[,] teste = mazeGenerator.GetMaze();
            // foreach(int i in teste) {

            //     Console.WriteLine(i);
            // }

            MazeGenerator.width = gridWidth;
            MazeGenerator.height = gridHeight;
            MazeGenerator.GenerateMaze();

            int[,] teste = MazeGenerator.GetMaze();

            for (int i = 0; i < MazeGenerator.width; i++)
            {
                for (int j = 0; j < MazeGenerator.height; j++)
                {
                    if (teste[i, j] == 1)
                    {
                        var ob = new Rectangle(100 + i * 40, 10 + j * 40, 40, 40);
                        if (!ob.Intersects(_exit) || !(i == 0 && j == 0))
                        {
                            obs.Add(ob);
                        }
                    }
                }
            }

            // generate cheeses
            for (int i = 0; i < 5 ; i++) {
                int x = rand.Next(0, MazeGenerator.width-1);
                int y = rand.Next(0, MazeGenerator.height-1);
                if (teste[x, y] == 0)
                    {
                    cheese = new Rectangle(105 + x * 40, 15 + y * 40, 30, 30);
                    cheeseArray.Add(cheese);
                    } else {
                        i--;
                    }   
            }
            _player = new Rectangle(150, 50, 30, 30);
            _exit = new Rectangle(1700, 935, 30, 30);
            
            
            

            // for(int I=0;I<_nOstacolli;I++){

            //     int xx=random.Next(100,_graphics.PreferredBackBufferWidth-100);

            //     int yy=random.Next(1,_graphics.PreferredBackBufferHeight);
            //     var ob=new Rectangle(xx, yy, 80, 20);
            //     if(ob.Intersects(_exit) || obs.Any(ele => ob.Intersects(ele))){
            //         --I;
            //         continue;
            //     }
            //     obs.Add(ob);
            // }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Crea una texture per colorare il rettangolo del giocatore
            // _playerTexture = new Texture2D(GraphicsDevice, 1, 1);
            // _playerTexture.SetData(new[] { Color.Red }); // Rettangolo rosso

            // Crea una texture per colorare l'ostacolo

            //  _obstacleTexture=Content.Load<Texture2D>("obsta1");

            // ExitTexture=new Texture2D(GraphicsDevice, 1, 1);
            ExitTexture = Content.Load<Texture2D>("person");
            PlayerTexture = Content.Load<Texture2D>("mouse");
            ObstacleTexture = Content.Load<Texture2D>("brick22");
            CheeseTexture = Content.Load<Texture2D>("cheese-4946581_640");
            // spriteOrigin.X = PlayerTexture.Height/2 ;
            // spriteOrigin.Y = PlayerTexture.Width/2;
            
            // ExitTexture.SetData(new[] { Color.Black }); 

            // _obstacleTexture = new Texture2D(GraphicsDevice, 1, 1);
            // _obstacleTexture.SetData(new[] { Color.White }); 
        }

        protected override void Update(GameTime gameTime)
        {
            // Esci con Esc
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Gestione movimento
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                _player.Y -= _playerSpeed; 
                // PlayerTextureFX = (lastHorizontalDirection == "left") ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                lastVerticalDirection = "up";
                rotationAngle = MathHelper.ToRadians(180);
                
            }
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) {
                 _player.Y += _playerSpeed;
                //   PlayerTextureFX = (lastHorizontalDirection == "left") ? SpriteEffects.FlipHorizontally| SpriteEffects.FlipVertically : SpriteEffects.FlipVertically;
                   lastVerticalDirection = "down";
                   rotationAngle = MathHelper.ToRadians(0);
            }
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) {
                 _player.X -= _playerSpeed;
                //   PlayerTextureFX = (lastVerticalDirection == "down") ? SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically : SpriteEffects.FlipHorizontally;
                   lastHorizontalDirection = "left";
                   rotationAngle = MathHelper.ToRadians(90);

            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) {
                 _player.X += _playerSpeed;
                //   PlayerTextureFX = (lastVerticalDirection == "down") ? SpriteEffects.FlipVertically : SpriteEffects.None;
                   lastHorizontalDirection = "right";
                   rotationAngle = MathHelper.ToRadians(270);
            }

            // Aumenta velocità con Shift
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                _playerSpeed = _maxSpeed;
            else
                _playerSpeed = 5;

            // Limiti dello schermo
            _player.X = System.Math.Clamp(_player.X, 0, _graphics.PreferredBackBufferWidth - _player.Width);
            _player.Y = System.Math.Clamp(_player.Y, 0, _graphics.PreferredBackBufferHeight - _player.Height);

            // Controllo collisione con l'ostacolo
            foreach (var ob in obs)
            {
                if (_player.Intersects(ob))
                {
                    // Sposta il giocatore indietro
                    if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                        _player.Y += _playerSpeed;
                    if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                        _player.Y -= _playerSpeed;
                    if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                        _player.X += _playerSpeed;
                    if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                        _player.X -= _playerSpeed;
                }
            }


            if (_player.Intersects(_exit) && cheeseArray.Count == 0)
            {
                //  _spriteBatch.DrawString(_font, "Attenzione: qualcosa è andato storto!", new Vector2(100, 100), Color.Red);
                Thread.Sleep(2000);
                obs.Clear();
                Initialize();
            }

        // for (int i = cheeseArray.Count - 1; i >= 0; i--)
        // {
        //     if (_player.Intersects(cheeseArray[i]))
        //     {
        //         cheeseArray.RemoveAt(i);
        //     }
        // }
            
            cheeseArray.RemoveAll(cheese => _player.Intersects(cheese));
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            _graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            _spriteBatch.Begin();
            Vector2 spriteOrigin = new Vector2(PlayerTexture.Width / 2, PlayerTexture.Height /2);

            Vector2 spritePosition = new Vector2(_player.X + _player.Width / 2, _player.Y + _player.Height / 2);
            // Disegna il rettangolo del giocatore
            // _spriteBatch.Draw(PlayerTexture, _player, Color.White);
            _spriteBatch.Draw(PlayerTexture,spritePosition,null,Color.White,rotationAngle,spriteOrigin,0.06f,PlayerTextureFX,0f);

            // Disegna l'ostacolo
            foreach (var ob in obs)
            {
                _spriteBatch.Draw(ObstacleTexture, ob, Color.White);
            }


            // _spriteBatch.Draw(ExitTexture,_exit,Color.White);
            _spriteBatch.Draw(ExitTexture, _exit, Color.White);

            foreach (var cheese in cheeseArray) {
                _spriteBatch.Draw(CheeseTexture, cheese, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}