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
    public class Bomb
    {
        Rectangle _dest;

        int _distance;

        float _timer;

        public bool _Exploded { get; set; }
        public int _OwnerId { get; set; }

        int _gx;
        int _gy;

        private Texture2D _texBomb;

        public Bomb(int x, int y, int distance, int ownerId)
        {
            _gx = x;
            _gy = y;

            _dest = new Rectangle((_gx * Game1._GridSizeX), (_gy * Game1._GridSizeY), Game1._GridSizeX, Game1._GridSizeY);
            _timer = 1.5f;
            _Exploded = false;

            _distance = distance;

            _OwnerId = ownerId;

            _texBomb = Game1._Content.Load<Texture2D>("Bomb");
        }

        public void Update(ref Level level, ref List<BombFire> bombFire)
        {
            _timer -= Game1._DeltaTime;

            if (_timer <= 0.0f)
            {
                float delayTime = 0.05f;

                bombFire.Add(new BombFire(_gx, _gy, 0.0f));

                bool doN = true;
                bool doS = true;
                bool doE = true;
                bool doW = true;

                for (int i = 1; i < _distance + 1; ++i)
                {
                    if (doN)
                    {
                        if (level.GetBlock(_gx, _gy - i)._Block == BlockData.BlockType.Rock)
                        {
                            bombFire.Add(new BombFire(_gx, _gy - i, delayTime * i));

                            doN = false;
                        }
                        else if (level.GetBlock(_gx, _gy - i)._Block == BlockData.BlockType.Wall)
                        {
                            doN = false;
                        }
                        if (doN)
                        {
                            bombFire.Add(new BombFire(_gx, _gy - i, delayTime * i));
                        }
                    }



                    if (doS)
                    {
                        if (level.GetBlock(_gx, _gy + i)._Block == BlockData.BlockType.Rock)
                        {
                            bombFire.Add(new BombFire(_gx, _gy + i, delayTime * i));
                            doS = false;
                        }
                        else if (level.GetBlock(_gx, _gy + i)._Block == BlockData.BlockType.Wall)
                        {
                            doS = false;
                        }
                        if (doS)
                        {
                            bombFire.Add(new BombFire(_gx, _gy + i, delayTime * i));
                        }
                    }


                    if (doE)
                    {
                        if (level.GetBlock(_gx + i, _gy)._Block == BlockData.BlockType.Rock)
                        {
                            bombFire.Add(new BombFire(_gx + i, _gy, delayTime * i));
                            doE = false;
                        }
                        else if (level.GetBlock(_gx + i, _gy)._Block == BlockData.BlockType.Wall)
                        {
                            doE = false;
                        }

                        if (doE)
                        {
                            bombFire.Add(new BombFire(_gx + i, _gy, delayTime * i));
                        }
                    }

                    if (doW)
                    {
                        if (level.GetBlock(_gx - i, _gy)._Block == BlockData.BlockType.Rock)
                        {
                            bombFire.Add(new BombFire(_gx - i, _gy, delayTime * i));
                            doW = false;
                        }
                        else if (level.GetBlock(_gx - i, _gy)._Block == BlockData.BlockType.Wall)
                        {
                            doW = false;
                        }
                        
                        if (doW)
                        {
                            bombFire.Add(new BombFire(_gx - i, _gy, delayTime * i));
                        }
                    }
                }
                BlockData ground;
                ground._Block = BlockData.BlockType.Ground;
                ground._Texture = BlockData.TextureType.Grass;
                ground._HasBomb = false;
                ground._Solid = false;

                level.SetBlock(_gx, _gy, ground);

                _Exploded = true;
            }
        }

        public void Draw()
        {
            Game1.spriteBatch.Draw(
                _texBomb,
                _dest,
                Color.Lerp(Color.Red, Color.White, _timer));
        }

        public void Explode()
        {
            if (_Exploded == false)
            {
                if (_timer > 0.1f)
                    _timer = 0.1f;
            }
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
