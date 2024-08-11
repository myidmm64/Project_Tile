using System;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoSingleTon<MainUI>
{
    private Dictionary<Type, MainUIElement> _uiElements = new Dictionary<Type, MainUIElement>();

    private void Awake()
    {
        BindUI();
    }

    private void BindUI()
    {
        var uiElements = GetComponentsInChildren<MainUIElement>();
        foreach(var uiElement in uiElements)
        {
            uiElement.BindUI();
            _uiElements.Add(uiElement.GetType(), uiElement);
        }
    }

    public T GetUIElement<T>() where T : MainUIElement
    {
        if (_uiElements.ContainsKey(typeof(T)))
        {
            return _uiElements[typeof(T)] as T;
        }

        return null;
    }
}
