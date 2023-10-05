using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorScript
{
    public class AudioWaveForm : MonoBehaviour
    {
        [Range(1, 2048)]
        [SerializeField]
        private int width = 1024;

        [Range(1, 2048)]
        [SerializeField]
        private int height = 100;

        [SerializeField]
        private Color backGroundColor = Color.black;

        [SerializeField]
        private Color foreGroundColor = Color.white;

        float[] waveform;
        float[] samples;
        private int sampleSize;

        public void Initialize()
        {
            Texture2D texture = GetWaveForm();
            Rect rect = new Rect(Vector2.zero, new Vector2(width, height));

            UIManaging.Instance.sliderBackGround.sprite = Sprite.Create(texture, rect, Vector2.zero);
        }

        private Texture2D GetWaveForm()
        {
            int halfHeight = height / 2;
            float heightScale = (float)height * 0.75f;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            waveform = new float[width];

            sampleSize = GameManager.GetMusic.clipSample * GameManager.GetMusic.clipChannels;
            samples = new float[sampleSize];
            GameManager.GetMusic.audioSource.clip.GetData(samples, 0);

            int packSize = (sampleSize / width);
            for(int w=0; w<width;w++)
            {
                waveform[w] = Mathf.Abs(samples[w*packSize]);
            }

            // set backGround Color
            for(int x=0;x<width;x++)
            {
                for(int y=0;y<height;y++)
                {
                    tex.SetPixel(x,y, backGroundColor);
                }
            }

            // set foreground Color
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < waveform[x] * heightScale; y++)
                {
                    tex.SetPixel(x, halfHeight + y, foreGroundColor);
                    tex.SetPixel(x, halfHeight - y, foreGroundColor);
                }
            }

            tex.Apply();

            return tex;
        }
    }
}
