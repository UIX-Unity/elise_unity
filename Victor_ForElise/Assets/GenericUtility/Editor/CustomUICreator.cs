using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class CustomUICreator : Editor {

    #region < Lagacy >

    //[MenuItem("GameObject/CustomUI/Text", false, 0)]
    //private static void CreateCustomUIText()
    //{
    //    // 텍스트 오브젝트 생성 및 설정
    //    GameObject txtObj = new GameObject();
    //    SetTextComponent(txtObj);

    //    txtObj.name = "CustomText";
    //    txtObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

    //    // 버튼 오브젝트 하위에 텍스트 오브젝트 넣은 후 마무리
    //    if (Selection.activeGameObject != null) txtObj.transform.SetParent(Selection.activeGameObject.transform);
    //    txtObj.transform.localPosition = Vector3.zero;
    //    txtObj.transform.localScale = Vector3.one;
    //}

    //[MenuItem("GameObject/CustomUI/TextEx", false, 0)]
    //private static void CreateCustomUITextEx()
    //{
    //    // 텍스트 오브젝트 생성 및 설정
    //    GameObject txtObj = new GameObject();
    //    SetTextComponentEx(txtObj);

    //    txtObj.name = "CustomText";
    //    txtObj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 100);

    //    // 버튼 오브젝트 하위에 텍스트 오브젝트 넣은 후 마무리
    //    if (Selection.activeGameObject != null) txtObj.transform.SetParent(Selection.activeGameObject.transform);
    //    txtObj.transform.localPosition = Vector3.zero;
    //    txtObj.transform.localScale = Vector3.one;
    //}

    //[MenuItem("GameObject/CustomUI/Scroll", false, 0)]
    //private static void CreateCustomUIScroll()
    //{
    //    if (Selection.activeGameObject == null)
    //    {
    //        Debug.LogError("선택한 오브젝트가 없습니다.");
    //        return;
    //    }

    //    var canvas = Selection.activeGameObject.GetComponentInParent<Canvas>();
    //    if (canvas == null)
    //    {
    //        Debug.LogError("Canvas를 선택한 후에 생성해주세요.");
    //        return;
    //    }

    //    // 스크롤뷰 생성 및 설정
    //    GameObject scrollViewGobj = new GameObject("CustomScrollView");
    //    scrollViewGobj.transform.SetParent(Selection.activeGameObject.transform);
    //    scrollViewGobj.transform.localScale = Vector3.one;
    //   var scrollRect = scrollViewGobj.AddComponent<ScrollRect>();
    //    var scrollViewImage = scrollViewGobj.AddComponent<Image>();
    //    scrollViewImage.raycastTarget = true; 
    //    scrollViewImage.color = new Color(1, 1, 1, 0.005f);
    //    var customScroll = scrollViewGobj.AddComponent<CustomScroll>();
    //    var itemCreator = scrollViewGobj.AddComponent<CustomScrollItemCreator>();
    //    var scrollViewRtf = scrollViewGobj.GetComponent<RectTransform>();
    //    scrollViewRtf.sizeDelta = new Vector2(400, 200);
    //    scrollViewRtf.anchoredPosition3D = Vector3.zero;

    //    // 뷰포트 생성 및 설정
    //    GameObject viewport = new GameObject("Viewport", typeof(Mask));
    //    viewport.transform.SetParent(scrollViewGobj.transform);
    //    viewport.transform.localScale = Vector3.one;
    //    viewport.AddComponent<Image>().color = new Color(1, 1, 1, 0.005f);
    //    var viewportRtf = viewport.GetComponent<RectTransform>();
    //    viewportRtf.anchorMin = Vector2.zero;
    //    viewportRtf.anchorMax = Vector2.one;
    //    viewportRtf.anchoredPosition3D = Vector3.zero;
    //    viewportRtf.sizeDelta = Vector2.one;

    //    // 컨텐트 생성 및 설정
    //    GameObject content = new GameObject("Content", typeof(GridLayoutGroup));
    //    content.transform.SetParent(viewport.transform);
    //    content.transform.localScale = Vector3.one;
    //    var contentRtf = content.GetComponent<RectTransform>();
    //    contentRtf.sizeDelta = new Vector2(400, 200);
    //    contentRtf.anchoredPosition3D = Vector3.zero;

    //    // 마무리 
    //    scrollRect.horizontal = false;
    //    scrollRect.vertical = false;
    //    scrollRect.viewport = viewportRtf;
    //    scrollRect.content = contentRtf;
    //}

    //[MenuItem("GameObject/CustomUI/TextComponentSet")]
    //private static void CustomUITextCompSetting()
    //{
    //    if(Selection.activeGameObject == null)
    //    {
    //        Debug.LogError("CustomUICreator : 게임 오브젝트를 선택하세요.");
    //        return;
    //    }
    //    SetTextComponent(Selection.activeGameObject);
    //}

    //[MenuItem("GameObject/CustomUI/TextExComponentSet")]
    //private static void CustomUITextExCompSetting()
    //{
    //    if (Selection.activeGameObject == null)
    //    {
    //        Debug.LogError("CustomUICreator : 게임 오브젝트를 선택하세요.");
    //        return;
    //    }
    //    SetTextComponentEx(Selection.activeGameObject);
    //}


    //private static void SetTextComponent(GameObject _obj)
    //{
    //    if (_obj.GetComponent<Text>() == null)
    //    {
    //        var txt = _obj.AddComponent<Text>();
    //        txt.fontSize = 40;
    //        txt.alignment = TextAnchor.MiddleCenter;
    //        txt.text = "Text";
    //        txt.raycastTarget = false;
    //    }

    //    Shadow tshadow = _obj.GetComponent<Shadow>();
    //    if (tshadow == null)
    //    {
    //        tshadow = _obj.AddComponent<Shadow>();
    //    }
    //    tshadow.effectColor = new Color(0, 0, 0, 1);
    //    tshadow.effectDistance = new Vector2(2, -2);

    //    Outline toutline = _obj.GetComponent<Outline>();
    //    if (toutline == null)
    //    {
    //        toutline = _obj.AddComponent<Outline>();
    //    }
    //    toutline.effectColor = new Color(0, 0, 0, 1);
    //    toutline.effectDistance = new Vector2(0.5f, -0.5f);

    //    if (_obj.GetComponent<TextSelector>() == null) _obj.AddComponent<TextSelector>();
    //    return;
    //}

    //private static void SetTextComponentEx(GameObject _obj)
    //{
    //    if (_obj.GetComponent<Text>() != null)
    //    {
    //        Debug.LogError("CustomUICreator : TextSelectorEx를 추가할 수 없습니다.");
    //        return;
    //    }

    //    // TextSelectorEx Setting
    //    if (_obj.GetComponent<TextSelectorEx>() == null)
    //    {
    //        var txt = _obj.AddComponent<TextSelectorEx>();
    //        txt.fontSize = 40;
    //        txt.alignment = TextAnchor.MiddleCenter;
    //        txt.text = "Text";
    //        txt.raycastTarget = false;
    //    }

    //    Shadow shadow = _obj.GetComponent<Shadow>();
    //    if (shadow == null)
    //    {
    //        shadow = _obj.AddComponent<Shadow>();
    //    }
    //    shadow.effectColor = new Color(0, 0, 0, 1);
    //    shadow.effectDistance = new Vector2(2, -2);

    //    Outline outline = _obj.GetComponent<Outline>();
    //    if (outline == null)
    //    {
    //        outline = _obj.AddComponent<Outline>();
    //    }
    //    outline.effectColor = new Color(0, 0, 0, 1);
    //    outline.effectDistance = new Vector2(0.5f, -0.5f);

    //    //if (_obj.GetComponent<TextSelector>() == null) _obj.AddComponent<TextSelector>();
    //}



    #endregion

    [MenuItem("GameObject/CustomUI/Button", false, 0)]
    private static void CreateCustomUIButton()
    {
        Vector2 btnSize = new Vector2(200, 100);

        // 버튼 오브젝트 생성 및 설정
        GameObject btnObj = new GameObject();
        SetButtonComponent(btnObj, btnSize);
        btnObj.name = "CustomButton";

        // 버튼 오브젝트 하위에 텍스트 오브젝트 넣은 후 마무리
        if (Selection.activeGameObject != null) btnObj.transform.SetParent(Selection.activeGameObject.transform);
        btnObj.transform.localPosition = Vector3.zero;
        btnObj.transform.localScale = Vector3.one;
    }


    [MenuItem("GameObject/CustomUI/ButtonComponentSet")]
    private static void CustomUIButtonCompSetting()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("CustomUICreator : 게임 오브젝트를 선택하세요.");
            return;
        }
        SetButtonComponent(Selection.activeGameObject, Selection.activeGameObject.GetComponent<RectTransform>().sizeDelta);
    }


    private static void SetButtonComponent(GameObject _obj, Vector2 _size)
    {
        var image = _obj.GetComponent<Image>();
        if (image == null)
        {
            image = _obj.AddComponent<Image>();
        }
        image.rectTransform.sizeDelta = _size;

        var customButton = _obj.GetComponent<CustomButton>();
        if (customButton == null)
        {
            customButton = _obj.AddComponent<CustomButton>();
        }

        //if (_obj.GetComponentInChildren<Image>() == null)
        //{
        //    // 아이콘 오브젝트 생성 및 설정
        //    GameObject iconObj = new GameObject();
        //    iconObj.AddComponent<Image>();
        //    iconObj.transform.SetParent(_obj.transform);

        //    var iconRt = iconObj.GetComponent<RectTransform>();
        //    iconRt.sizeDelta = _size - new Vector2(10, 10);
        //    iconObj.name = "Icon";
        //}

        var txt = _obj.GetComponentInChildren<TextMeshProUGUI>();
        if (txt == null)
        {
            // 텍스트 오브젝트 생성 및 설정
            GameObject txtObj = new GameObject();
            txt = SetTextComponent(txtObj, _size);

            txtObj.name = "Text";
            txtObj.transform.SetParent(_obj.transform);
            txtObj.transform.position = Vector3.zero;
        }

        customButton.rtf = image.rectTransform;
        customButton.image = image;
        customButton.label = txt;
        customButton.useAnimation = true;
    }


    private static TextMeshProUGUI SetTextComponent(GameObject _obj, Vector2 _size)
    {
        var txt = _obj.GetComponent<TextMeshProUGUI>();
        if (txt == null)
        {
            txt = _obj.AddComponent<TextMeshProUGUI>();
            txt.rectTransform.anchorMin = Vector3.zero;
            txt.rectTransform.anchorMax = Vector3.one;
            txt.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            txt.rectTransform.rect.Set(0, 0, _size.x, _size.y);
            txt.alignment = TextAlignmentOptions.Center | TextAlignmentOptions.Midline;
            txt.fontSize = 40;
            txt.text = "Text";
            txt.color = Color.black;
            txt.raycastTarget = false;
        }
        return txt;
    }

}
