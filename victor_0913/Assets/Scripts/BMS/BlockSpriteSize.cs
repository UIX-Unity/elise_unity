using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpriteSize : MonoBehaviour
{
    public List<UISprite> line;
    public List<UISprite> key;
    public List<UISprite> note;

    Vector3 noteBasicPos = new Vector3(0, -2157.5f, 0);
    Vector3 lineBasicPos = new Vector3(0, -2157.5f, 0);

    float totalSize;
    float noteSize;

	// Use this for initialization
	void Start ()
    {
        switch (Screen.width)
        {
            // 16 WIDTH SCREEN ANCHOR RESET
            case 1920:
            case 2560:
            case 1280:
            case 1600:
                {
                    switch (Screen.height)
                    {
                        // 9 HEIGHT SCREEN ACHOR SET
                        case 1080:
                        case 1440:
                        case 720:
                        case 900:
                            totalSize = 1240f;
                            break;

                        // 10 HEIGHT SCREEN ACHOR SET
                        case 1600:
                        case 800:
                        case 1200:
                        case 1000:
                            totalSize = 1120f;
                            break;
                    }
                    noteSize = totalSize / 6f;
                    for (int i = 0; i < key.Count; i++)
                    {
                        key[i].width = (int)noteSize;
                        note[i].width = key[i].width;
                    }
                    key[2].transform.localPosition = new Vector3(-(noteSize / 2), noteBasicPos.y, 0);
                    key[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, noteBasicPos.y, 0);
                    key[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, noteBasicPos.y, 0);
                    key[3].transform.localPosition = new Vector3(noteSize / 2 - 1, noteBasicPos.y, 0);
                    key[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, noteBasicPos.y, 0);
                    key[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, noteBasicPos.y, 0);

                    line[2].transform.localPosition = new Vector3(-(noteSize / 2), lineBasicPos.y, 0);
                    line[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, lineBasicPos.y, 0);
                    line[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, lineBasicPos.y, 0);
                    line[3].transform.localPosition = new Vector3(noteSize / 2 - 1, lineBasicPos.y, 0);
                    line[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, lineBasicPos.y, 0);
                    line[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, lineBasicPos.y, 0);

                    note[2].transform.localPosition = new Vector3(-(noteSize / 2), note[2].transform.localPosition.y, 0);
                    note[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, note[1].transform.localPosition.y, 0);
                    note[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, note[0].transform.localPosition.y, 0);
                    note[3].transform.localPosition = new Vector3(noteSize / 2, note[3].transform.localPosition.y, 0);
                    note[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, note[4].transform.localPosition.y, 0);
                    note[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, note[5].transform.localPosition.y, 0);

                    break;
                }

            // 18 WIDTH SCREEN ANCHOR RESET
            case 2880:
            case 1800:
                totalSize = 1400f;
                noteSize = totalSize / 6f;
                for (int i = 0; i < key.Count; i++)
                {
                    key[i].width = (int)noteSize;
                    note[i].width = key[i].width;
                }
                key[2].transform.localPosition = new Vector3(-(noteSize / 2), noteBasicPos.y, 0);
                key[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, noteBasicPos.y, 0);
                key[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, noteBasicPos.y, 0);
                key[3].transform.localPosition = new Vector3(noteSize / 2, noteBasicPos.y, 0);
                key[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, noteBasicPos.y, 0);
                key[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, noteBasicPos.y, 0);

                line[2].transform.localPosition = new Vector3(-(noteSize / 2), lineBasicPos.y, 0);
                line[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, lineBasicPos.y, 0);
                line[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, lineBasicPos.y, 0);
                line[3].transform.localPosition = new Vector3(noteSize / 2, lineBasicPos.y, 0);
                line[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, lineBasicPos.y, 0);
                line[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, lineBasicPos.y, 0);

                note[2].transform.localPosition = new Vector3(-(noteSize / 2), note[2].transform.localPosition.y, 0);
                note[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, note[1].transform.localPosition.y, 0);
                note[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, note[0].transform.localPosition.y, 0);
                note[3].transform.localPosition = new Vector3(noteSize / 2, note[3].transform.localPosition.y, 0);
                note[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, note[4].transform.localPosition.y, 0);
                note[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, note[5].transform.localPosition.y, 0);

                break;

            // 18.5 WIDTH SCREEN ANCHOR RESET
            case 2960:
            case 2220:
            case 1850:
                totalSize = 1430f;
                noteSize = totalSize / 6f;
                for (int i = 0; i < key.Count; i++)
                {
                    key[i].width = (int)noteSize;
                    note[i].width = key[i].width;
                }
                key[2].transform.localPosition = new Vector3(-(noteSize / 2), noteBasicPos.y, 0);
                key[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, noteBasicPos.y, 0);
                key[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, noteBasicPos.y, 0);
                key[3].transform.localPosition = new Vector3(noteSize / 2, noteBasicPos.y, 0);
                key[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, noteBasicPos.y, 0);
                key[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, noteBasicPos.y, 0);

                line[2].transform.localPosition = new Vector3(-(noteSize / 2), lineBasicPos.y, 0);
                line[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, lineBasicPos.y, 0);
                line[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, lineBasicPos.y, 0);
                line[3].transform.localPosition = new Vector3(noteSize / 2, lineBasicPos.y, 0);
                line[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, lineBasicPos.y, 0);
                line[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, lineBasicPos.y, 0);

                note[2].transform.localPosition = new Vector3(-(noteSize / 2), note[2].transform.localPosition.y, 0);
                note[1].transform.localPosition = new Vector3(key[2].transform.localPosition.x - noteSize, note[1].transform.localPosition.y, 0);
                note[0].transform.localPosition = new Vector3(key[1].transform.localPosition.x - noteSize, note[0].transform.localPosition.y, 0);
                note[3].transform.localPosition = new Vector3(noteSize / 2, note[3].transform.localPosition.y, 0);
                note[4].transform.localPosition = new Vector3(key[3].transform.localPosition.x + noteSize, note[4].transform.localPosition.y, 0);
                note[5].transform.localPosition = new Vector3(key[4].transform.localPosition.x + noteSize, note[5].transform.localPosition.y, 0);

                break;
        }
    }
}