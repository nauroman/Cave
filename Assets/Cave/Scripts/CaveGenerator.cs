using System.Collections.Generic;
using UnityEngine;

namespace Flashunity.Cave
{
    public class CaveGenerator
    {


        public static int width;
        public static int height;

        public static int[,] map;

        public static void Generate(int width, int height, int percent)
        {
            CaveGenerator.width = width;
            CaveGenerator.height = height;

            map = new int[width, height];
            RandomFillMap(percent);

            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
            }
        }


        static void RandomFillMap(int randomFillPercent)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map [x, y] = 1;
                    } else
                    {
                        map [x, y] = (Random.Range(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
            }
        }

        public static void SmoothMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        map [x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        map [x, y] = 0;

                }
            }
        }

        static int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map [neighbourX, neighbourY];
                        }
                    } else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        public static int GetFacesCount()
        {
            int facesCount = 0;

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (map [x, y] == 1)
                        facesCount++;
                }

            return facesCount;
        }
    }
}