using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct EUIButtonEvent
{
    public static EUIButtonEvent _e;
    public EUIButton button;
    
    public static void Clicked(EUIButton button)
    {
        _e.button = button;
        EventBus.TriggerEvent(_e);
    }
}


[RequireComponent(typeof(Button))]
public class EUIButton : MonoBehaviour
{
    [SerializeField] private Button button;

    protected virtual void Awake()
    {
        button ??= GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }

    
    private void OnButtonClicked()
    {
        EUIButtonEvent.Clicked(this);
    }

}
