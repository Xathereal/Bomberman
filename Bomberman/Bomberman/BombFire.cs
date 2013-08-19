using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    public class BombFire
    {
        private int _gx;
        private int _gy;

        private float _timer;
        private float _delay;

        private Color _startColor;
        private Color _endColor;

        private float _timerStart;

        public BombFire(int gx, int gy, float delay)
        {
            _gx = gx;
            _gy = gy;

            _startColor = Color.OrangeRed;
            _endColor = Color.Yellow;
            _endColor.A = 0;

            _delay = 0.5f;
            _timer = _delay;

            _timerStart = delay;
        }

        public void Update(ref Player playerA, ref Player playerB, ref Level level)
        {
            if (_timerStart > 0.0f)
            {
                _timerStart -= Game1._DeltaTime;
                return;
            }

            BlockData ground;
            ground._Block = BlockData.BlockType.Ground;
            ground._Texture = BlockData.TextureType.Grass;
            ground._HasBomb = false;
            ground._Solid = false;

            if (level.GetBlock(_gx, _gy)._Block == BlockData.BlockType.Rock)
            {
                level.SetBlock(_gx, _gy, ground);
            }

            _timer -= Game1._DeltaTime;

            if (playerA.GetX() == _gx &&
                playerA.GetY() == _gy)
            {
                playerA.Kill();
            }

            if (playerB.GetX() == _gx &&
                playerB.GetY() == _gy)
            {
                playerB.Kill();
            }

            for (int i = 0; i < Game1._Bombs.Count; ++i)
            {
                if (Game1._Bombs[i].GetX() == _gx &&
                    Game1._Bombs[i].GetY() == _gy)
                {
                    Game1._Bombs[i].Explode();
                }
            }
        }

        public void Draw()
        {
            if (_timerStart > 0.0f)
            {
                return;
            }

            Game1.spriteBatch.Draw(
                Game1._DebugTexture,
                new Rectangle(
                    _gx * Game1._GridSizeX,
                    _gy * Game1._GridSizeY,
                    Game1._GridSizeX,
                    Game1._GridSizeY),
                Color.Lerp(_startColor, _endColor, (_delay - _timer) / _delay));
        }

        public bool ShouldDelete()
        {
            return (_timer < 0.0f);
        }
    }
}
