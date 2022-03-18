using System;
using UnityEngine;

public class StyleHolder : MonoBehaviour
{
    [SerializeField] private ButtonStyle[] _buttonStyles;

    public ButtonStyle[] ButtonStyles { get => _buttonStyles; }
}

[Serializable]
public struct ButtonStyle
{
    public Texture2D Icon;
    public string ButtonText;
    public GameObject Prefab;

    [HideInInspector]
    public GUIStyle NodeStyle;
}
