using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFullscreen : MonoBehaviour
{
    private RectTransform panelRectTransform;
    void Start()
    {
        panelRectTransform = GetComponent<RectTransform>();
        panelRectTransform.anchorMin = new Vector2(1, 0);
        panelRectTransform.anchorMax = new Vector2(0, 1);
        panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
}
