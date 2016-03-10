using System;
using UnityEngine;
using System.Collections;

using Flashunity.Logs;

namespace Flashunity.AtlasUVs
{
    public class BCubesUVsFromJSON : MonoBehaviour
    {
        public TextAsset text;


        public void UpdateDic()
        {
            //      Log.Add(text.text);

            JSONTexture texture = JsonUtility.FromJson<JSONTexture>(text.text);

            var cubesUVs = new CubeUVs[texture.frames.Length];

            var frames = texture.frames;

            //foreach (JSONFrame frame in frames.frames)
            for (int i = 0; i < frames.Length; i++)
            {
                var frame = frames [i];

                var name = frame.name;

                var underscoreIndex = name.LastIndexOf("_");

                if (underscoreIndex <= 0)
                {
                    //    throw new Exception("Wrong texture file name in texture atlas. You must use somefilename_t, somefilename_s, somefilename_b. You use: " + filename);
                    continue;
                }

                var textureIndex = GetTextureIndex(name, underscoreIndex);
                var sideName = GetSideName(name, underscoreIndex);

                CubeUVs cubeUVs = cubesUVs [textureIndex];

                if (cubeUVs != null)
                {
                    SetSide(cubeUVs, sideName, frame);//, texture.meta.size);
                } else
                {
                    cubeUVs = new CubeUVs();
                    SetSide(cubeUVs, sideName, frame);//.frame, texture.meta.size);
                    cubesUVs [textureIndex] = cubeUVs;
                }
            }
            CubesUVs.cubesUVs = cubesUVs;
        }

        void SetSide(CubeUVs cubeUVs, char sideName, JSONFrame frame)
        {
            switch (sideName)
            {
                case 't':
                    cubeUVs.top = GetUV(frame.x, 1 - frame.y - frame.h, frame.w, frame.h);
                    break;
                case 's':
                    cubeUVs.side = GetUV(frame.x, 1 - frame.y - frame.h, frame.w, frame.h);
                    break;
                case 'b':
                    cubeUVs.bottom = GetUV(frame.x, 1 - frame.y - frame.h, frame.w, frame.h);
                    break;
            }
        }

        Vector2[] GetUV(float x, float y, float w, float h)
        {
            return new Vector2[]
            {
                new Vector2(x, y),
                new Vector2(x, y + h),
                new Vector2(x + w, y + h),
                new Vector2(x + w, y)
            };
        }

        char GetSideName(string filename, int underscoreIndex)
        {
            return filename.Substring(underscoreIndex + 1, 1) [0];
        }

        int GetTextureIndex(string filename, int underscoreIndex)
        {
            var cubeName = GetCubeName(filename, underscoreIndex);

            int index = int.Parse(cubeName);
            
            return index;
        }

        string GetCubeName(string filename, int underscoreIndex)
        {
            return filename.Substring(0, underscoreIndex);
        }

        void NewExceptionWrongFileName(string filename)
        {
            throw new Exception("Wrong texture file name in atlas. You must use somefilename_t.png, somefilename_s.png, somefilename_b.png. You use: " + filename);
        }
    }


    [Serializable]
    public class JSONTexture
    {
        public JSONFrame[] frames;
        public JSONMeta meta;
    }

    [Serializable]
    public class JSONFrame
    {
        public string name;

        public float x;
        public float y;
        public float w;
        public float h;
    }


    [Serializable]
    public class JSONMeta
    {
        public JSONSize size;
    }

    [Serializable]
    public class JSONSize
    {
        public int w;
        public int h;
    }
}