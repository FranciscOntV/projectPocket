using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public bool LimitFramerate = true;
    public int framerateCap = 60;
    public Vector2Int targetResolution = new Vector2Int(1280, 720);
    private void Awake()
    {
        if (LimitFramerate)
            Application.targetFrameRate = framerateCap;
        Screen.SetResolution(targetResolution.x, targetResolution.y, false);
    }
}
