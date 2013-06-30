using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle
{
    static public class Tiler
    {
        static public void Tile(bool[,] bits, Action tileHandler, bool edges = true)
        {
            for (TileX = 0; TileX < bits.GetLength(0); TileX++)
            {
                for (TileY = 0; TileY < bits.GetLength(1); TileY++)
                {
                    if (bits[TileX, TileY])
                    {
                        Left = TileX == 0 ? edges : bits[TileX - 1, TileY];
                        Right = TileX == 31 ? edges : bits[TileX + 1, TileY];
                        Up = TileY == 0 ? edges : bits[TileX, TileY - 1];
                        Down = TileY == 23 ? edges : bits[TileX, TileY + 1];

                        UpLeft = (TileX == 0 || TileY == 0) ? edges : bits[TileX - 1, TileY - 1];
                        UpRight = (TileX == 31 || TileY == 0) ? edges : bits[TileX + 1, TileY - 1];
                        DownLeft = (TileX == 0 || TileY == 23) ? edges : bits[TileX - 1, TileY + 1];
                        DownRight = (TileX == 31 || TileY == 23) ? edges : bits[TileX + 1, TileY + 1];

                        tileHandler();
                    }
                }
            }
        }

        static public void Tile(bool[,] bits, Func<int> tileHandler, Tilemap tilemap, int tileWidth, int tileHeight, bool edges = true)
        {
            for (TileX = 0; TileX < bits.GetLength(0); TileX++)
            {
                for (TileY = 0; TileY < bits.GetLength(1); TileY++)
                {
                    if (bits[TileX, TileY])
                    {
                        Left = TileX == 0 ? edges : bits[TileX - 1, TileY];
                        Right = TileX == 31 ? edges : bits[TileX + 1, TileY];
                        Up = TileY == 0 ? edges : bits[TileX, TileY - 1];
                        Down = TileY == 23 ? edges : bits[TileX, TileY + 1];

                        UpLeft = (TileX == 0 || TileY == 0) ? edges : bits[TileX - 1, TileY - 1];
                        UpRight = (TileX == 31 || TileY == 0) ? edges : bits[TileX + 1, TileY - 1];
                        DownLeft = (TileX == 0 || TileY == 23) ? edges : bits[TileX - 1, TileY + 1];
                        DownRight = (TileX == 31 || TileY == 23) ? edges : bits[TileX + 1, TileY + 1];

                        tilemap.DrawTile(tileHandler(), TileX * tileWidth, TileY * tileHeight);
                    }
                }
            }
        }

        static public int TileX { get; private set; }
        static public int TileY { get; private set; }
        static public bool Left { get; private set; }
        static public bool Right { get; private set; }
        static public bool Up { get; private set; }
        static public bool Down { get; private set; }
        static public bool UpLeft { get; private set; }
        static public bool UpRight { get; private set; }
        static public bool DownLeft { get; private set; }
        static public bool DownRight { get; private set; }
    }
}
