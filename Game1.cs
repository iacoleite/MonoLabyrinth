using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


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
            _player = new Rectangle(10, 10, 30, 30);
            Random random= new Random();

            // Inizializzazione dell'ostacolo
            _exit = new Rectangle(random.Next(1600,_graphics.PreferredBackBufferWidth-50), random.Next(1,_graphics.PreferredBackBufferHeight-50), 30, 30);
            int _nOstacolli = 50;

            for(int I=0;I<_nOstacolli;I++){
                
                int xx=random.Next(100,_graphics.PreferredBackBufferWidth-100);
                
                int yy=random.Next(1,_graphics.PreferredBackBufferHeight);
                var ob=new Rectangle(xx, yy, 80, 20);
                if(ob.Intersects(_exit) || obs.Any(ele => ob.Intersects(ele))){
                    --I;
                    continue;
                }
                obs.Add(ob);
            }

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
            Initialize();
           }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Disegna il rettangolo del giocatore
            _spriteBatch.Draw(_playerTexture, _player, Color.White);

            // Disegna l'ostacolo
            foreach(var ob in obs){
              _spriteBatch.Draw(_obstacleTexture, ob, Color.White);
            }
            
            // _spriteBatch.Draw(ExitTexture,_exit,Color.White);
            _spriteBatch.Draw(ExitTexture, new Vector2(_exit.X,_exit.Y), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}