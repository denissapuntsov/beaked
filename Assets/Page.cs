using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Page : MonoBehaviour
{
    public List<Selectable> Selectables { get; private set; } = new List<Selectable>();

    private void Awake()
    {
        Selectables = GetComponentsInChildren<Selectable>(true).ToList();
    }
}
