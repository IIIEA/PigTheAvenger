using UnityEngine;

public class Part : MonoBehaviour
{
    private int _row;
    private int _column;
    private string _partName = "Empty";
    private GameObject _part;
    private GUIStyle _style;

    public int Row => _row;
    public int Column => _column;
    public GUIStyle Style => _style;

    public void Init(GameObject gameObject, string name, GUIStyle guiStyle, int row, int column)
    {
        _part = gameObject;
        _partName = name;
        _style = guiStyle;
        _row = row;
        _column = column;
    }

    public void Init(GameObject gameObject, string name, int row, int column)
    {
        _part = gameObject;
        _partName = name;
        _row = row;
        _column = column;
    }
}
