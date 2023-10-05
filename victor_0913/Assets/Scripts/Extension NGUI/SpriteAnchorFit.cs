using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnchorFit : MonoBehaviour
{
    UISprite sprite;
    public float landscape;
    public int portait;

    public bool isLandscape;
    public bool isPortait;

	// Use this for initialization
	void Start ()
    {
        sprite = GetComponent<UISprite>();

        if(isLandscape)
        {
            float _landscape;

            switch (Screen.width)
            {
                // 16 WIDTH SCREEN ANCHOR RESET
                case 1920:
                case 2560:
                case 1280:
                case 1600:
                    _landscape = 1600 / landscape;
                    sprite.leftAnchor.relative = 0;
                    sprite.leftAnchor.absolute = -(int)(Screen.width / _landscape) - 1;
                    sprite.rightAnchor.relative = 1;
                    sprite.rightAnchor.absolute = (int)(Screen.width / _landscape) + 1;
                    break;

                // 18 WIDTH SCREEN ANCHOR RESET
                case 2880:
                    sprite.leftAnchor.relative = 0;
                    sprite.leftAnchor.absolute = -25;
                    sprite.rightAnchor.relative = 1;
                    sprite.rightAnchor.absolute = 25;
                    break;

                // 18.5 WIDTH SCREEN ANCHOR RESET
                case 2960:
                    sprite.leftAnchor.relative = 0;
                    sprite.leftAnchor.absolute = 0;
                    sprite.rightAnchor.relative = 1;
                    sprite.rightAnchor.absolute = 0;
                    break;
            }
        }

        if(isPortait)
        {
            float _portait;

            switch (Screen.height)
            {
                // 9 HEIGHT SCREEN ACHOR SET
                case 1080:
                case 1440:
                case 720:
                    _portait = 900 / portait;
                    sprite.bottomAnchor.relative = 0;
                    sprite.bottomAnchor.absolute = -(int)(Screen.height / _portait) - 1;
                    sprite.topAnchor.relative = 1;
                    sprite.topAnchor.absolute = (int)(Screen.height / _portait) + 1;
                    break;

                // 10 HEIGHT SCREEN ACHOR SET
                case 1600:
                case 800:
                case 1200:
                    break;
            }
        }

        sprite.UpdateAnchors();
	}
}
