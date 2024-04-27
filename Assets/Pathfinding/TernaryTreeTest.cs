using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class TernaryTreeTest : MonoBehaviour
{
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _removeButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField, Range(1, 100)] private int width = 20;
    [SerializeField, Range(1, 100)] private int height = 20;
    [SerializeField] private TestPathfindGridVisual _visual;

    private readonly TernaryTree<int> _tree = new();

    private TestPathfinding _pathfinding;

    private void Start()
    {
        InitGrid();
        //InitTree();
        //TestTree();
    }

    private void OnEnable()
    {
        _addButton.onClick.AddListener(AddNode);
        _removeButton.onClick.AddListener(RemoveNode);
    }

    private void OnDisable()
    {
        _addButton.onClick.RemoveListener(AddNode);
        _removeButton.onClick.RemoveListener(RemoveNode);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Equals)) 
            InitGrid();

        if (Input.GetMouseButtonUp(0))
            _pathfinding.ChangeTree();

        if (Input.GetMouseButtonUp(1))
            _pathfinding.SetDestination();
    }

    private void AddNode()
    {
        DoActionWithTree(_tree.Add);
    }

    private void RemoveNode()
    {
        DoActionWithTree(_tree.Remove);
    }

    private void DoActionWithTree(Action<int> treeAction)
    {
        var text = _inputField.text;
        if (string.IsNullOrEmpty(text))
            return;

        if (int.TryParse(text, out var num))
        {
            treeAction(num);
        }

        Debug.Log(_tree.ToString());
    }

    private void InitGrid()
    {
        _pathfinding = new(width, height, _visual);
    }

    private void InitTree()
    {
        _tree.Add(5);
        _tree.Add(6);
        _tree.Add(4);
        _tree.Add(5);
        _tree.Add(6);

        Debug.Log(_tree.ToString());
    }

    private void TestTree()
    {
        Debug.Log("TernaryTreeNull with Middle");
        var TernaryTreeNull = new TernaryTree<int>();
        TernaryTreeNull.AddAndReturnNode(5);
        TernaryTreeNull.AddAndReturnNode(4);
        TernaryTreeNull.AddAndReturnNode(5);
        TernaryTreeNull.AddAndReturnNode(5);
        TernaryTreeNull.AddAndReturnNode(6);
        Debug.Log(TernaryTreeNull.ToString());
        TernaryTreeNull.Remove(5);
        Debug.Log(TernaryTreeNull.ToString());

        Debug.Log("TernaryTreeLeft with Middle");
        var TernaryTreeLeft = new TernaryTree<int>();
        TernaryTreeLeft.AddAndReturnNode(3);
        TernaryTreeLeft.AddAndReturnNode(5);
        TernaryTreeLeft.AddAndReturnNode(4);
        TernaryTreeLeft.AddAndReturnNode(6);
        TernaryTreeLeft.AddAndReturnNode(6);
        TernaryTreeLeft.AddAndReturnNode(5);
        TernaryTreeLeft.AddAndReturnNode(5);
        TernaryTreeLeft.AddAndReturnNode(8);
        Debug.Log(TernaryTreeLeft.ToString());
        TernaryTreeLeft.Remove(5);
        Debug.Log(TernaryTreeLeft.ToString());

        Debug.Log("TernaryTreeRigth with Middle");
        var TernaryTreeRigth = new TernaryTree<int>();
        TernaryTreeRigth.AddAndReturnNode(7);
        TernaryTreeRigth.AddAndReturnNode(5);
        TernaryTreeRigth.AddAndReturnNode(5);
        TernaryTreeRigth.AddAndReturnNode(5);
        TernaryTreeRigth.AddAndReturnNode(6);
        TernaryTreeRigth.AddAndReturnNode(6);
        TernaryTreeRigth.AddAndReturnNode(4);
        Debug.Log(TernaryTreeRigth.ToString());
        TernaryTreeRigth.Remove(5);
        Debug.Log(TernaryTreeRigth.ToString());

        Debug.Log("TernaryTreeNull");
        TernaryTreeNull = new TernaryTree<int>();
        TernaryTreeNull.AddAndReturnNode(5);
        TernaryTreeNull.AddAndReturnNode(4);
        TernaryTreeNull.AddAndReturnNode(6);
        Debug.Log(TernaryTreeNull.ToString());
        TernaryTreeNull.Remove(5);
        Debug.Log(TernaryTreeNull.ToString());

        Debug.Log("TernaryTreeLeft");
        TernaryTreeLeft = new TernaryTree<int>();
        TernaryTreeLeft.AddAndReturnNode(3);
        TernaryTreeLeft.AddAndReturnNode(5);
        TernaryTreeLeft.AddAndReturnNode(4);
        TernaryTreeLeft.AddAndReturnNode(6);
        Debug.Log(TernaryTreeLeft.ToString());
        TernaryTreeLeft.Remove(5);
        Debug.Log(TernaryTreeLeft.ToString());

        Debug.Log("TernaryTreeRigth");
        TernaryTreeRigth = new TernaryTree<int>();
        TernaryTreeRigth.AddAndReturnNode(7);
        TernaryTreeRigth.AddAndReturnNode(5);
        TernaryTreeRigth.AddAndReturnNode(6);
        TernaryTreeRigth.AddAndReturnNode(4);
        Debug.Log(TernaryTreeRigth.ToString());
        TernaryTreeRigth.Remove(5);
        Debug.Log(TernaryTreeRigth.ToString());
    }
}
