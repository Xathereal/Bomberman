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
    public class Player
    {
        Rectangle _dest;

        int _gx;
        int _gy;

        Keys _keyUp;
        Keys _keyDown;
        Keys _keyLeft;
        Keys _keyRight;
        Keys _keyBomb;

        Color _drawColor;
        Color _deadColor;

        int _bombPower;
        int _bombCount;
        int _bombCountMax;

        bool _dead;
        int _score;
        float _deadTimer;


        public int _Id
        {
            get;
            set;
        }

        public Player(Color drawColor, Keys up, Keys down, Keys left, Keys right, Keys bomb, int x, int y)
        {
            _keyUp = up;
            _keyDown = down;
            _keyLeft = left;
            _keyRight = right;
            _keyBomb = bomb;

            _drawColor = drawColor;
            _deadColor = drawColor;
            
            _gx = x;
            _gy = y;

            _dest = new Rectangle(x * Game1._GridSizeX, y * Game1._GridSizeX, Game1._GridSizeX, Game1._GridSizeY);

            _bombPower = 1;

            _bombCount = 0;
            _bombCountMax = 1;

            _deadTimer = 0.0f;
        }

        public void Update(ref Level level)
        {
            if (_dead == false)
            {
                UpdateMovement(level);
                UpdateBomb(level);

                _dest.X = _gx * Game1._GridSizeX;
                _dest.Y = _gy * Game1._GridSizeY;
            }
            else if (_deadTimer <= 1.0f)
            {
                _deadTimer += Game1._DeltaTime;
                _deadColor = Color.Lerp(_drawColor, Color.Black, _deadTimer);

                _dest.Width = (int)((1 - _deadTimer) * Game1._GridSizeX);
                _dest.Height = (int)((1 - _deadTimer) * Game1._GridSizeY);

                _dest.X = _gx * Game1._GridSizeX + (Game1._GridSizeX - _dest.Width) / 2;
                _dest.Y = _gy * Game1._GridSizeY + (Game1._GridSizeY - _dest.Height) / 2;

            }
        }

        private void UpdateBomb(Level level)
        {
            if (_bombCount < _bombCountMax)
            {
                if (Game1._CurrKState.IsKeyDown(_keyBomb) &&
                    Game1._PrevKState.IsKeyUp(_keyBomb))
                {
                    BlockData bd = level.GetBlock(_gx, _gy);

                    if (bd._HasBomb == false)
                    {
                        // create bomb
                        Bomb bomb = new Bomb(_gx, _gy, _bombPower, _Id);
                        Game1._Bombs.Add(bomb);

                        // tell block it has bomb
                        bd._HasBomb = true;
                        bd._Solid = true;

                        level.SetBlock(_gx, _gy, bd);

                        _bombCount++;
                    }
                }
            }
        }

        private void UpdateMovement(Level level)
        {
            int gxTemp = _gx;
            int gyTemp = _gy;

            if (Game1._CurrKState.IsKeyDown(_keyUp) && Game1._PrevKState.IsKeyUp(_keyUp))
            {
                _gy--;
            }
            if (Game1._CurrKState.IsKeyDown(_keyDown) && Game1._PrevKState.IsKeyUp(_keyDown))
            {
                _gy++;
            }

            if (level.GetBlock(_gx, _gy)._Solid == true)
            {
                _gx = gxTemp;
                _gy = gyTemp;
            }

            if (Game1._CurrKState.IsKeyDown(_keyLeft) && Game1._PrevKState.IsKeyUp(_keyLeft))
            {
                _gx--;
            }
            if (Game1._CurrKState.IsKeyDown(_keyRight) && Game1._PrevKState.IsKeyUp(_keyRight))
            {
                _gx++;
            }

            if (level.GetBlock(_gx, _gy)._Solid == true)
            {
                _gx = gxTemp;
                _gy = gyTemp;
            }
        }

        public void Draw()
        {
            Game1.spriteBatch.Draw(
                Game1._DebugTexture,
                _dest,
                _dead ? _deadColor : _drawColor);
               
        }

        public void Kill()
        {
            _dead = true;
        }

        public void SetBombCount(int x)
        {
            _bombCount = x;
        }

        public void IncreasePower()
        {
            _bombPower++;
        }

        public void IncreaseBombs()
        {
            _bombCountMax++;
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
}
