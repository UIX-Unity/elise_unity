using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageNavigatorUI_Item : UIBase
{
    [SerializeField]
    private Image m_NavigateIcon;
    public Image navigateIcon => m_NavigateIcon;

    [SerializeField]
    private Sprite m_UnselectedIcon;
    public Sprite unselectedIcon => m_UnselectedIcon;

    [SerializeField]
    private Color m_UnselectedColor = new Color(1, 1, 1, 1);
    public Color UnselectedColor => m_UnselectedColor;

    [SerializeField]
    private Sprite m_SelectedIcon;
    public Sprite selectedIcon => m_SelectedIcon;

    [SerializeField]
    private Color m_SelectedColor = new Color(1, 1, 1, 1);
    public Color selectedColor => m_SelectedColor;

    public PageNavigatorUI pageNavigatorUI { get; private set; }
    public bool isSelectedPage { get; private set; }

    public void SetData(PageNavigatorUI pageNavigatorUI, bool isSelectedPage)
    {
        this.pageNavigatorUI = pageNavigatorUI;
        this.isSelectedPage = isSelectedPage;
    }

    public void UpdateUI()
    {
        navigateIcon.sprite = (isSelectedPage) ? selectedIcon : unselectedIcon;
        navigateIcon.color = (isSelectedPage) ? selectedColor : UnselectedColor;
    }
}
