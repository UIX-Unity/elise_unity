using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Helper
{
    public static class Image
    {
        public static Vector2 CalSizeBaseOnHeight(Sprite sprite, float frameHeight)
        {
            float ratio = frameHeight / sprite.rect.height;
            return new Vector2(sprite.rect.width * ratio, sprite.rect.height * ratio);
        }

        /// <summary>
        /// <para>Image RectTransform의 높이를 기준으로 가로너비를 리사이즈한다.</para>
        /// </summary>
        public static UnityEngine.UI.Image SetWidthBasedOnHeight(UnityEngine.UI.Image image, Sprite sprite)
        {
            var size = CalSizeBaseOnHeight(sprite, image.rectTransform.rect.height);
            image.sprite = sprite;
            image.rectTransform.sizeDelta = size;
            return image;
        }
    }
}
