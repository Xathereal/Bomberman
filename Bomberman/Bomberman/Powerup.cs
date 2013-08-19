using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class Powerup
    {
        protected Texture2D _texture;
        protected Color _color;

        protected int _gx;
        protected int _gy;

        public Powerup(int gx, int gy)
        {
            _gx = gx;
            _gy = gy;
        }

        public virtual void OnPickup(ref Player player)
        {
            // tell player picked up
            // tell level gone
        }

        public void Draw()
        {
            Game1.spriteBatch.Draw(
                _texture,
                new Rectangle(
                    _gx * Game1._GridSizeX,
                    _gy * Game1._GridSizeY,
                    Game1._GridSizeX,
                    Game1._GridSizeY),
                _color);
        }

        public int GetX()
        {
            return _gx;
        }

        public int GetY()
        {
            return _gy;
        }
    }

    public class Powerup_BombPower : Powerup
    {
        public Powerup_BombPower(int gx, int gy)
            : base(gx, gy)
        {
            _texture = Game1._DebugTexture;
            _color = Color.PaleGoldenrod;
        }

        public override void OnPickup(ref Player player)
        {
            player.IncreasePower();
        }
    }

    public class Powerup_BombCount : Powerup
    {
        public Powerup_BombCount(int gx, int gy)
            : base(gx, gy)
        {
            _texture = Game1._Content.Load<Texture2D>("BombPlus");
            _color = Color.White;
        }

        public override void OnPickup(ref Player player)
        {
            player.IncreaseBombs();
        }
    }
}
