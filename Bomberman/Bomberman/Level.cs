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
    public struct BlockData
    {
        public bool _Solid;
        public bool _HasBomb;

        public BlockType _Block;
        public TextureType _Texture;

        public enum BlockType
        {
            Wall,
            Rock,
            Ground,
        }

        public enum TextureType
        {
            Grass,
            Rock,
            Wall_Top,
            Wall_Side,
            Wall
        }
    }

    public class Level
    {
        BlockData[,] _blocks;

        int w;
        int h;

        Random random;

        List<Powerup> _powerups;

        private Texture2D _texGrass;
        private Texture2D _texRock;
        private Texture2D _texWallTop;
        private Texture2D _texWallSide;
        private Texture2D _texWall;
        

        public Level()
        {
            w = Game1._ScreenWidth / Game1._GridSizeX;
            h = Game1._ScreenHeight / Game1._GridSizeY;
            
            random = new Random(DateTime.Now.Millisecond);

            _powerups = new List<Powerup>();

            _blocks = new BlockData[w, h];

            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    _blocks[i, j]._HasBomb = false;

                    if (j == 0 ||
                        j == h - 1 ||
                        i == 0 ||
                        i == w - 1)
                    {
                        _blocks[i, j]._Block = BlockData.BlockType.Wall;
                        _blocks[i, j]._Solid = true;

                        if ( (j == 0 && i == 0) ||
                             (j == 0 && i == w - 1) ||
                             (j == h - 1 && i == 0) ||
                             (j == h - 1 && i == w - 1))

                        {
                            // corner
                            _blocks[i, j]._Texture = BlockData.TextureType.Wall_Top;
                        }
                        else if (j == 0 || j == h - 1)
                        {
                            _blocks[i, j]._Texture = BlockData.TextureType.Wall_Top;
                        }
                        else if (i == 0 || i == w - 1)
                        {
                            _blocks[i, j]._Texture = BlockData.TextureType.Wall_Side;
                        }
                    }
                    else if (j % 2 == 0 && i % 2 == 0)
                    {
                        _blocks[i, j]._Block = BlockData.BlockType.Wall;
                        _blocks[i, j]._Texture = BlockData.TextureType.Wall;
                        _blocks[i, j]._Solid = true;
                    }
                    else if (random.NextDouble() < 0.75)
                    {
                        _blocks[i, j]._Block = BlockData.BlockType.Rock;
                        _blocks[i, j]._Texture = BlockData.TextureType.Rock;
                        _blocks[i, j]._Solid = true;
                    }
                    else
                    {
                        _blocks[i, j]._Block = BlockData.BlockType.Ground;
                        _blocks[i, j]._Texture = BlockData.TextureType.Grass;
                        _blocks[i, j]._Solid = false;
                    }
                    
                }
            }

            // player 1
            _blocks[1, 1]._Block = BlockData.BlockType.Ground;
            _blocks[1, 1]._Texture = BlockData.TextureType.Grass;
            _blocks[1, 1]._Solid = false;

            _blocks[1, 2]._Block = BlockData.BlockType.Ground;
            _blocks[1, 2]._Texture = BlockData.TextureType.Grass;
            _blocks[1, 2]._Solid = false;

            _blocks[2, 1]._Block = BlockData.BlockType.Ground;
            _blocks[2, 1]._Texture = BlockData.TextureType.Grass;
            _blocks[2, 1]._Solid = false;

            // player 2
            _blocks[w - 2, h - 2]._Block = BlockData.BlockType.Ground;
            _blocks[w - 2, h - 2]._Texture = BlockData.TextureType.Grass;
            _blocks[w - 2, h - 2]._Solid = false;

            _blocks[w - 3, h - 2]._Block = BlockData.BlockType.Ground;
            _blocks[w - 3, h - 2]._Texture = BlockData.TextureType.Grass;
            _blocks[w - 3, h - 2]._Solid = false;

            _blocks[w - 2, h - 3]._Block = BlockData.BlockType.Ground;
            _blocks[w - 2, h - 3]._Texture = BlockData.TextureType.Grass;
            _blocks[w - 2, h - 3]._Solid = false;

            _texGrass = Game1._Content.Load<Texture2D>("GrassTile");
            _texRock = Game1._Content.Load<Texture2D>("Boulder");
            _texWall = Game1._Content.Load<Texture2D>("BrickWall");
            _texWallSide = Game1._Content.Load<Texture2D>("BrickWallSkirting_V");
            _texWallTop = Game1._Content.Load<Texture2D>("BrickWallSkirting_H");
        }

        public void Update(ref Player playerA, ref Player playerB)
        {
            for (int i = 0; i < _powerups.Count; ++i)
            {
                if (playerA.GetX() == _powerups[i].GetX() &&
                    playerA.GetY() == _powerups[i].GetY())
                {
                    _powerups[i].OnPickup(ref playerA);

                    _powerups.RemoveAt(i--);
                }
            }

            for (int i = 0; i < _powerups.Count; ++i)
            {
                if (playerB.GetX() == _powerups[i].GetX() &&
                    playerB.GetY() == _powerups[i].GetY())
                {
                    _powerups[i].OnPickup(ref playerB);

                    _powerups.RemoveAt(i--);
                }
            }
        }

        public void Draw()
        {
            Rectangle dest = new Rectangle(
                0,
                0,
                Game1._GridSizeX,
                Game1._GridSizeY);

            for (int i = 0; i < h; ++i)
            {
                for (int j = 0; j < w; ++j)
                {
                    switch (_blocks[j, i]._Texture)
                    {
                        case BlockData.TextureType.Grass:
                            Game1.spriteBatch.Draw(
                                _texGrass,
                                dest,
                                Color.White);
                            break;
                        case BlockData.TextureType.Rock:
                            Game1.spriteBatch.Draw(
                                 _texGrass,
                                 dest,
                                 Color.White);
                            Game1.spriteBatch.Draw(
                                _texRock,
                                dest,
                                Color.White);
                            break;
                        case BlockData.TextureType.Wall:
                            Game1.spriteBatch.Draw(
                                _texWall,
                                dest,
                                Color.White);
                            break;
                        case BlockData.TextureType.Wall_Side:
                            Game1.spriteBatch.Draw(
                                _texWallSide,
                                dest,
                                Color.White);
                            break;
                        case BlockData.TextureType.Wall_Top:
                            Game1.spriteBatch.Draw(
                                _texWallTop,
                                dest,
                                Color.White);
                            break;
                        default:
                            Console.WriteLine("No type allocated to block");
                            break;
                    }

                    dest.X += Game1._GridSizeX;

                }

                dest.X = 0;
                dest.Y += Game1._GridSizeY;
            }

            for (int i = 0; i < _powerups.Count; ++i)
            {
                _powerups[i].Draw();
            }
        }

        public BlockData GetBlock(int x, int y)
        {
            return _blocks[x, y];
        }

        public void SetBlock(int x, int y, BlockData data)
        {
            if (_blocks[x, y]._Block == BlockData.BlockType.Rock)
            {
                if (random.NextDouble() < 0.3)
                {
                    if (random.NextDouble() < 0.6)
                    {
                        _powerups.Add(new Powerup_BombPower(x, y));
                    }
                    else
                    {
                        _powerups.Add(new Powerup_BombCount(x, y));
                    }
                }
            }

            _blocks[x, y]._Block = data._Block;
            _blocks[x, y]._Solid = data._Solid;
            _blocks[x, y]._Texture = data._Texture;
            _blocks[x, y]._HasBomb = data._HasBomb;
        }
    }
}
