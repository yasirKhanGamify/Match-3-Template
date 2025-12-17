using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasSizeChangeNotifier : UIBehaviour
{
    public static Action OnCanvasSizeChanged;

    private RectTransform rectTransform;
    private Vector2 lastSize;

    protected override void Awake()
    {
        base.Awake();
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
            lastSize = rectTransform.rect.size;
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (rectTransform == null) return;

        Vector2 newSize = rectTransform.rect.size;
        if (newSize != lastSize)
        {
            lastSize = newSize;
            OnCanvasSizeChanged?.Invoke();
            Debug.Log("[CanvasSizeChangeNotifier] Canvas size changed: " + newSize);
        }
    }
}