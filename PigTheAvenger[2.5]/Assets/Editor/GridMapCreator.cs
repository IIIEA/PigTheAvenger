using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridMapCreator : EditorWindow
{
    private Vector2 _offset;
    private Vector2 _drag;
    private Vector2 _nodePosition;
    private Rect _menuBarRect;
    private GUIStyle _emptyStyle;
    private GUIStyle _currentStyle;
    private List<List<Node>> _nodes;
    private List<List<Part>> _parts;
    private StyleHolder _stylesHolder;
    private GameObject Map;
    private bool _isErasing;

    private const int Width = 20;
    private const int Height = 20;
    private const int NodesCountX = 20;
    private const int NodesCountY = 10;

    [MenuItem("Window/Grid map creator", priority = 51)]
    private static void OpenWindow()
    {
        GridMapCreator window = GetWindow<GridMapCreator>();
        window.titleContent = new GUIContent("Grid Map Creator");
    }

    private void OnEnable()
    {
        SetupStyles();
        SetupNodes();
        SetupMap();
    }

    private void SetupMap()
    {
        try
        {
            Map = GameObject.FindGameObjectWithTag("Map");
            RestoreMap(Map);
        }
        catch(Exception exception)
        {

        }

        if(Map == null)
        {
            Map = new GameObject("Map");
            Map.tag = "Map";
        }
    }

    private void RestoreMap(GameObject map)
    {
        if(Map.transform.childCount > 0)
        {
            for (int i = 0; i < Map.transform.childCount; i++)
            {
                int ii = Map.transform.GetChild(i).GetComponent<Part>().Row;
                int jj = Map.transform.GetChild(i).GetComponent<Part>().Column;
                GUIStyle guiStyle = Map.transform.GetChild(i).GetComponent<Part>().Style;
                _nodes[ii][jj].SetStyle(guiStyle);
                _parts[ii][jj] = Map.transform.GetChild(i).GetComponent<Part>();
                _parts[ii][jj].Init(Map.transform.GetChild(i).gameObject, Map.transform.GetChild(i).name, ii, jj);
            }
        }
    }

    private void SetupStyles()
    {
        try
        {
            _stylesHolder = GameObject.FindGameObjectWithTag("StyleHolder").GetComponent<StyleHolder>();

            for (int i = 0; i < _stylesHolder.ButtonStyles.Length; i++)
            {
                _stylesHolder.ButtonStyles[i].NodeStyle = new GUIStyle();
                _stylesHolder.ButtonStyles[i].NodeStyle.normal.background = _stylesHolder.ButtonStyles[i].Icon;
            }
        }
        catch (Exception exception) { }

        _emptyStyle = _stylesHolder.ButtonStyles[0].NodeStyle;

        _currentStyle = _stylesHolder.ButtonStyles[1].NodeStyle;
    }

    private void OnGUI()
    {
        DrawGrid();
        DrawNodes();
        DrawMenuBar();
        ProcessNodes(Event.current);
        ProcessGrid(Event.current);

        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawMenuBar()
    {
        _menuBarRect = new Rect(0, 0, position.width, 20);

        GUILayout.BeginArea(_menuBarRect, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        for (int i = 0; i < _stylesHolder.ButtonStyles.Length; i++) 
        {
            if (GUILayout.Toggle((_currentStyle == _stylesHolder.ButtonStyles[i].NodeStyle), new GUIContent(_stylesHolder.ButtonStyles[i].ButtonText), EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                _currentStyle = _stylesHolder.ButtonStyles[i].NodeStyle;
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void ProcessNodes(Event eventType)
    {
        int row = (int)((eventType.mousePosition.x - _offset.x) / Width);
        int column = (int)((eventType.mousePosition.y - _offset.y) / Height);

        if ((eventType.mousePosition.x - _offset.x) < 0 || (eventType.mousePosition.x - _offset.x) > 600 
            || (eventType.mousePosition.y - _offset.y) < 0 || (eventType.mousePosition.y - _offset.y) > 200)
        {

        }
        else
        {
            if (eventType.type == EventType.MouseDown)
            {
                if (_nodes[row][column].GUIStyle.normal.background.name == "Empty")
                {
                    _isErasing = false;
                }
                else
                {
                    _isErasing = true;
                }

                PaintNodes(row, column);
            }

            if (eventType.type == EventType.MouseDrag)
            {
                PaintNodes(row, column);
                eventType.Use();
            }
        }
    }

    private void PaintNodes(int row, int column)
    {
        if (_isErasing)
        {
            if (_parts[row][column] != null)
            {
                _nodes[row][column].SetStyle(_emptyStyle);
                DestroyImmediate(_parts[row][column].gameObject);
                GUI.changed = true;
            }

            _parts[row][column] = null;
        }
        else
        {
            if (_parts[row][column] == null)
            {
                _nodes[row][column].SetStyle(_currentStyle);

                GameObject gameObject = Instantiate(Resources.Load("MapParts/" + _currentStyle.normal.background.name)) as GameObject ;
                gameObject.name = _currentStyle.normal.background.name;
                gameObject.transform.position = new Vector3(column * 2, 0, row * 2) + Vector3.forward * 5 + Vector3.right * 5;
                gameObject.transform.parent = Map.transform;

                _parts[row][column] = gameObject.GetComponent<Part>();
                _parts[row][column].Init(gameObject, gameObject.name, _currentStyle, row, column);

                GUI.changed = true;
            }
            
        }
    }

    private void ProcessGrid(Event eventType)
    {
        _drag = Vector2.zero;

        switch (eventType.type)
        {
            case EventType.MouseDrag:
                if (eventType.button == 0)
                {
                    OnMouseDrag(eventType.delta);
                }
                break;
            default:
                break;
        }
    }

    private void DrawNodes()
    {
        for (int i = 0; i < NodesCountX; i++)
        {
            for (int j = 0; j < NodesCountY; j++)
            {
                _nodes[i][j].Draw();
            }
        }
    }

    private void SetupNodes()
    {
        _parts = new List<List<Part>>();
        _nodes = new List<List<Node>>();

        for (int i = 0; i < 20; i++)
        {
            _nodes.Add(new List<Node>());
            _parts.Add(new List<Part>());

            for (int j = 0; j < 20; j++)
            {
                _nodePosition.Set(i * Width, j * Height);
                _nodes[i].Add(new Node(_nodePosition, Width, Height, _emptyStyle));
                _parts[i].Add(null);
            }
        }
    }

    private void OnMouseDrag(Vector2 delta)
    {
        _drag = delta;

        for (int i = 0; i < NodesCountX; i++)
        {
            for (int j = 0; j < NodesCountY; j++)
            {
                _nodes[i][j].Drag(delta);
            }
        }
        GUI.changed = true;
    }

    private void DrawGrid()
    {
        int widthDivider = Mathf.CeilToInt(position.width / 20);
        int heightDivider = Mathf.CeilToInt(position.height / 20);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);

        _offset += _drag;

        Vector3 newOffset = new Vector3(_offset.x % 20, _offset.y % 20, 0);

        for (int i = 0; i < widthDivider; i++)
        {
            Handles.DrawLine(new Vector3(20 * i, -20, 0) + newOffset, new Vector3(20 * i, position.height, 0) + newOffset);
        }

        for (int i = 0; i < heightDivider; i++)
        {
            Handles.DrawLine(new Vector3(-20, 20 * i, 0) + newOffset, new Vector3(position.width, 20 * i, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }
}
