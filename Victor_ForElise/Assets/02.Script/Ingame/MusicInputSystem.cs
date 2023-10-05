using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EInputPhase
{
    None,
    PointDown,
    PointUp,
    Drag,
}

public struct InputData
{
    public int touchId;

    public Vector3 curScreenPos; // 현재 포인터의 스크린 좌표
    public Vector3 curWorldPos; // 현재 포인터의 월드 좌표
    public Vector3 startScreenPos;
    public Vector3 startWorldPos; // 처음 터치한 월드 좌표
    public Vector3 deltaPos; // 한 프레임간에 이동한 거리와 방향

    public EInputPhase inputPhase;
}

public class MusicInputSystem : MonoBehaviour
{
    private Dictionary<int, InputData> prevData = new Dictionary<int, InputData>();

    private Camera mainCam;

    public UnityAction<InputData> inputAction;

    public void Initialize()
    {
        GameManager.Instance.OnUpdate += OnUpdate;
        mainCam = Camera.main;
    }

    public void OnUpdate(float deltaTime)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            UpdatePointerDown(Input.mousePosition, 0);
        }
        if (Input.GetMouseButton(0))
        {
            UpdatePointerDrag(Input.mousePosition, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            UpdatePointerUp(Input.mousePosition, 0);
        }

#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount < 1)
        {
            return;
        }

        for (int i=0;i<Input.touchCount;i++)
        {
            Touch touchTmp = Input.GetTouch(i);

            if (touchTmp.phase == TouchPhase.Began)
            {
                UpdatePointerDown(touchTmp.position, touchTmp.fingerId);
            }
            if (touchTmp.phase == TouchPhase.Moved || touchTmp.phase == TouchPhase.Stationary)
            {
                UpdatePointerDrag(touchTmp.position, touchTmp.fingerId);
            }
            if(touchTmp.phase == TouchPhase.Ended)
            {
                UpdatePointerUp(touchTmp.position, touchTmp.fingerId);
            }
        }
#endif

    }

    public void UpdatePointerDown(Vector3 inputPos, int touchId)
    {
        InputData inputData = new InputData();

        inputData.touchId = touchId;
        inputData.curScreenPos = inputPos;
        inputData.curWorldPos = mainCam.ScreenToWorldPoint(inputData.curScreenPos);
        inputData.startScreenPos = inputData.curScreenPos;
        inputData.startWorldPos = inputData.curWorldPos;
        inputData.deltaPos = Vector3.zero;
        inputData.inputPhase = EInputPhase.PointDown;

        prevData.Add(touchId, inputData);

        inputAction?.Invoke(inputData);
    }
    public void UpdatePointerDrag(Vector3 inputPos, int touchId)
    {
        InputData inputData = new InputData();

        inputData.touchId = touchId;
        inputData.curScreenPos = inputPos;
        inputData.curWorldPos = mainCam.ScreenToWorldPoint(inputData.curScreenPos);
        inputData.startScreenPos = prevData[touchId].startScreenPos;
        inputData.startWorldPos = prevData[touchId].startWorldPos;
        inputData.deltaPos = inputData.curWorldPos - inputData.startWorldPos;
        inputData.inputPhase = EInputPhase.Drag;

        inputAction?.Invoke(inputData);
    }
    public void UpdatePointerUp(Vector3 inputPos, int touchId)
    {
        InputData inputData = new InputData();

        inputData.touchId = touchId;
        inputData.curScreenPos = inputPos;
        inputData.curWorldPos = mainCam.ScreenToWorldPoint(inputData.curScreenPos);
        inputData.deltaPos = inputData.curWorldPos - inputData.startWorldPos;
        inputData.inputPhase = EInputPhase.PointUp;

        prevData.Remove(touchId);

        inputAction?.Invoke(inputData);
    }
}
