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

namespace Bomberman
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static int _ScreenWidth;
        public static int _ScreenHeight;
        public static Texture2D _DebugTexture;
        public static ContentManager _Content;
        public static KeyboardState _CurrKState;
        public static KeyboardState _PrevKState;
        public static float _DeltaTime;
        public static int _GridSizeX;
        public static int _GridSizeY;

        Player _playerA;
        Player _playerB;
        Level _level;

        public static List<Bomb> _Bombs;
        List<BombFire> _bombFire;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            _GridSizeX = 48;
            _GridSizeY = 48;

            graphics.PreferredBackBufferWidth = _ScreenWidth = _GridSizeX * 27;
            graphics.PreferredBackBufferHeight = _ScreenHeight = _GridSizeY * 15;

            graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _Bombs = new List<Bomb>();
            _bombFire = new List<BombFire>();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _Content = Content;

            _DebugTexture = Content.Load<Texture2D>("blank");

            _playerA = new Player(Color.Red, Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, 1, 1);
            _playerA._Id = 0;

            _playerB = new Player(Color.Blue, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.RightControl, 25, 13);
            _playerB._Id = 1;

            _level = new Level();
        }

        protected void UpdatePre(GameTime gameTime)
        {
            _CurrKState = Keyboard.GetState();
            _DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        protected override void Update(GameTime gameTime)
        {
            UpdatePre(gameTime);

            int[] counters = new int[2];

            for (int i = 0; i < 2; ++i)
            { 
                counters[i] = 0; 
            }

            for (int i = 0; i < _Bombs.Count; ++i)
            {
                _Bombs[i].Update(ref _level, ref _bombFire);

                if (_Bombs[i]._Exploded == true)
                {
                    _Bombs.RemoveAt(i--);
                }
                else
                {
                    counters[_Bombs[i]._OwnerId]++;
                }
            }

            for (int i = 0; i < _bombFire.Count; ++i)
            {
                _bombFire[i].Update(ref _playerA, ref _playerB, ref _level);

                if (_bombFire[i].ShouldDelete())
                {
                    _bombFire.RemoveAt(i--);
                }
            }

            _playerA.SetBombCount(counters[0]);
            _playerB.SetBombCount(counters[1]);

            _level.Update(ref _playerA, ref _playerB);

            _playerA.Update(ref _level);
            _playerB.Update(ref _level);


            UpdatePost(gameTime);
        }

        protected void UpdatePost(GameTime gameTime)
        {
            _PrevKState = _CurrKState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone);

            _level.Draw();
            
            for (int i = 0; i < _Bombs.Count; ++i)
            {
                _Bombs[i].Draw();
            }

            for (int i = 0; i < _bombFire.Count; ++i)
            {
                _bombFire[i].Draw();
            }

            _playerA.Draw();
            _playerB.Draw();


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
