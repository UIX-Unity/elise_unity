using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPanel : MonoBehaviour
{
    GameSetting info;
    UISlider slider;
    public UILabel speedLabel;

	// Use this for initialization
	void Start ()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        slider = GetComponent<UISlider>();
        Refresh();
    }

    void Refresh()
    {
        slider.value = info.speed / 10f;
        speedLabel.text = string.Format("{0:f1}", info.speed);
    }

    public void speedUp()
    {
        if (info.speed < 10f)
            info.speed += 0.5f;

        Refresh();
    }

    public void speedDown()
    {
        if (info.speed > 0.5f)
            info.speed -= 0.5f;
        
        Refresh();
    }
}
