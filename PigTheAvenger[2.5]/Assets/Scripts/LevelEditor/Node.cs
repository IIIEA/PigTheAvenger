using UnityEngine;

public class Node
{
    private Rect _rect;
    private GUIStyle _guiStyle;

    public GUIStyle GUIStyle => _guiStyle;

    public Node(Vector2 position, float width, float height, GUIStyle defaultGUIStyle)
    {
        _rect = new Rect(position.x, position.y, width, height);
        _guiStyle = defaultGUIStyle;
    }

    public void Drag(Vector2 delta)
    {
        _rect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(_rect, "", _guiStyle);
    }

    public void SetStyle(GUIStyle nodeStyle)
    {
        _guiStyle = nodeStyle;
    }
}
