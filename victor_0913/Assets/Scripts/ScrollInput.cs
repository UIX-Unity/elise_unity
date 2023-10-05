using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollInput : MonoBehaviour
{
    [SerializeField] CircularMenu menu;

    [SerializeField] SongListen song;
    [SerializeField] UnityEngine.GameObject customziePanel;

    [SerializeField] Camera guiCamera;

    Vector3 oldPos;
    Vector3 downPos;

    bool isDrag;

    public int indexTemp;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;
            oldPos = Input.mousePosition;
            downPos = Input.mousePosition;

            indexTemp = menu.GetCurrentIndex();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            menu.SetCenter();

            Ray ray = guiCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                int l = hitInfo.transform.gameObject.layer;

                if (l < 9)
                {
                    return;
                }
            }

            if (Mathf.Abs(Input.mousePosition.y - downPos.y) < 50)
            {
                if (!customziePanel.activeSelf)
                {
                    customziePanel.SetActive(true);
                }
            }

            if (indexTemp == menu.GetCurrentIndex())
                return;

            song.PlayMusic();
        }

        if (isDrag)
        {
            float distance = (Input.mousePosition - oldPos).y;
            
            menu.Percentage = Mathf.Clamp01(menu.Percentage + distance / Screen.height);
            oldPos = Input.mousePosition;
            song.PauseMusic();
        }

    }
}