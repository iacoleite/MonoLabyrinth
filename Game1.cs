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

       
        // Ostacoli
        private Rectangle _exit;
        private Texture2D _obstacleTexture;

        private Texture2D ExitTexture;

        public List<Rectangle> obs= new List<Rectangle>();
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

            Random random= new Random();

            // Inizializzazione dell'ostacolo
            int _nOstacolli = 50;

            int screenWidth = _graphics.PreferredBackBufferWidth-100;
            int screenHeight = _graphics.PreferredBackBufferHeight-100;
            int gridWidth = Math.Abs(screenWidth/37);
            int gridHeight = Math.Abs(screenHeight/37);

            // Console.WriteLine(screenHeight);
            // Console.WriteLine(screenWidth);
            // Console.WriteLine(gridHeight);
            // Console.WriteLine(gridWidth);
            MazeGenerator mazeGenerator = new MazeGenerator();
            
            // mazeGenerator.GenerateMaze();
            // int[,] teste = mazeGenerator.GetMaze();
            // foreach(int i in teste) {

            //     Console.WriteLine(i);
            // }
            MazeGenerator.GenerateMaze();
            int [,] teste = MazeGenerator.PrintMaze();

            for (int i = 0; i < MazeGenerator.width; i++) {
                for (int j = 0; j < MazeGenerator.height; j++) {
                    if (teste[i,j] == 1) {
                        var ob=new Rectangle(100 + i * 40, 10 + j * 40, 40, 40);
                        if(!ob.Intersects(_exit) || !(i == 0 && j == 0)) {
                        obs.Add(ob);
                        }
                    }
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
            _playerTexture = new Texture2D(GraphicsDevice, 1, 1);
            _playerTexture.SetData(new[] { Color.Red }); // Rettangolo rosso

            // Crea una texture per colorare l'ostacolo

            //  _obstacleTexture=Content.Load<Texture2D>("obsta1");

            // ExitTexture=new Texture2D(GraphicsDevice, 1, 1);
            ExitTexture=Content.Load<Texture2D>("person");


            // ExitTexture.SetData(new[] { Color.Black }); 

            _obstacleTexture = new Texture2D(GraphicsDevice, 1, 1);
            _obstacleTexture.SetData(new[] { Color.White }); 
        }

        protected override void Update(GameTime gameTime)
        {
            // Esci con Esc
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Gestione movimento
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) )
                _player.Y -= _playerSpeed;
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) )
                _player.Y += _playerSpeed;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) )
                _player.X -= _playerSpeed;
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D) )
                _player.X += _playerSpeed;

            // Aumenta velocità con Shift
            if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                _playerSpeed = _maxSpeed;
            else
                _playerSpeed = 5;

            // Limiti dello schermo
            _player.X = System.Math.Clamp(_player.X, 0, _graphics.PreferredBackBufferWidth - _player.Width);
            _player.Y = System.Math.Clamp(_player.Y, 0, _graphics.PreferredBackBufferHeight - _player.Height);

            // Controllo collisione con l'ostacolo
            foreach(var ob in obs){

                 if (_player.Intersects(ob))
            {
                // Sposta il giocatore indietro
                if (keyboardState.IsKeyDown(Keys.Up)|| keyboardState.IsKeyDown(Keys.W) )
                    _player.Y += _playerSpeed;
                if (keyboardState.IsKeyDown(Keys.Down)|| keyboardState.IsKeyDown(Keys.S) )
                    _player.Y -= _playerSpeed;
                if (keyboardState.IsKeyDown(Keys.Left)|| keyboardState.IsKeyDown(Keys.A) )
                    _player.X += _playerSpeed;
                if (keyboardState.IsKeyDown(Keys.Right)|| keyboardState.IsKeyDown(Keys.D) )
                    _player.X -= _playerSpeed;
            }
            }
           
           
           if (_player.Intersects(_exit)){
            //  _spriteBatch.DrawString(_font, "Attenzione: qualcosa è andato storto!", new Vector2(100, 100), Color.Red);
            Thread.Sleep(2000);
            obs.Clear();
            Initialize();
           }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
                    
            _graphics.GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin();

            // Disegna il rettangolo del giocatore
            _spriteBatch.Draw(_playerTexture, _player, Color.White);

            // Disegna l'ostacolo
            foreach(var ob in obs){
              _spriteBatch.Draw(_obstacleTexture, ob, Color.White);
            }
            
            // _spriteBatch.Draw(ExitTexture,_exit,Color.White);
            _spriteBatch.Draw(ExitTexture, _exit, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}