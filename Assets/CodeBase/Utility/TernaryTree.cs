using System;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// Расположения узла относительно родителя
/// </summary>
public enum TernarySide
{
    Left,
    Middle,
    Right
}

/// <summary>
/// Узел бинарного дерева
/// </summary>
/// <typeparam name="T"></typeparam>
public class TernaryTreeNode<T> where T : IComparable<T>
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="data">Данные</param>
    public TernaryTreeNode(T data)
    {
        Data = data;
    }

    /// <summary>
    /// Данные которые хранятся в узле
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Левая ветка
    /// </summary>
    public TernaryTreeNode<T> LeftNode;

    /// <summary>
    /// Центральная ветка
    /// </summary>
    public TernaryTreeNode<T> MiddleNode;

    /// <summary>
    /// Правая ветка
    /// </summary>
    public TernaryTreeNode<T> RightNode;

    /// <summary>
    /// Родитель
    /// </summary>
    public TernaryTreeNode<T> ParentNode;

    /// <summary>
    /// Расположение узла относительно его родителя
    /// </summary>
    public TernarySide? NodeSide =>
        ParentNode == null
            ? null
            : ParentNode.MiddleNode == this
                ? TernarySide.Middle
                : ParentNode.LeftNode == this
                    ? TernarySide.Left
                    : TernarySide.Right;

    /// <summary>
    /// Преобразование экземпляра класса в строку
    /// </summary>
    /// <returns>Данные узла дерева</returns>
    public override string ToString() => Data.ToString();
}
    
/// <summary>
/// Бинарное дерево
/// </summary>
/// <typeparam name="T">Тип данных хранящихся в узлах</typeparam>
public class TernaryTree<T> where T : IComparable<T>
{
    /// <summary>
    /// Корень бинарного дерева
    /// </summary>
    protected TernaryTreeNode<T> RootNode;
    
    /// <summary>
    /// Добавление данных в бинарное дерево
    /// </summary>
    /// <param name="data">Данные</param>
    /// <returns>Узел</returns>
    public void Add(T data)
    {
        Add(new TernaryTreeNode<T>(data), null);
    }

    /// <summary>
    /// Добавление данных в бинарное дерево с возвратом добавленной ноды
    /// </summary>
    /// <param name="data">Данные</param>
    /// <returns>Узел</returns>
    public TernaryTreeNode<T> AddAndReturnNode(T data)
    {
        return Add(new TernaryTreeNode<T>(data), null);
    }

    /// <summary>
    /// Добавление нового узла в бинарное дерево
    /// </summary>
    /// <param name="node">Новый узел</param>
    /// <param name="currentNode">Текущий узел</param>
    /// <returns>Узел</returns>
    public TernaryTreeNode<T> Add(TernaryTreeNode<T> node, TernaryTreeNode<T> currentNode)
    {
        if (RootNode == null)
        {
            node.ParentNode = null;
            return RootNode = node;
        }

        currentNode ??= RootNode;
        node.ParentNode = currentNode;
        int result = node.Data.CompareTo(currentNode.Data);
        return result == 0
            ? AddChildren(node, ref currentNode.MiddleNode)
            : result < 0
                ? AddChildren(node, ref currentNode.LeftNode)
                : AddChildren(node, ref currentNode.RightNode);
    }

    /// <summary>
    /// Поиск узла по значению
    /// </summary>
    /// <param name="data">Искомое значение</param>
    /// <returns>Найденный узел</returns>
    public TernaryTreeNode<T> FindNode(T data)
    {
        return FindNode(data, null);
    }

    /// <summary>
    /// Поиск узла по значению
    /// </summary>
    /// <param name="data">Искомое значение</param>
    /// <param name="startWithNode">Узел начала поиска</param>
    /// <returns>Найденный узел</returns>
    public TernaryTreeNode<T> FindNode(T data, TernaryTreeNode<T> startWithNode)
    {
        if (RootNode == null)
            return null;

        startWithNode ??= RootNode;
        int result;
        return (result = data.CompareTo(startWithNode.Data)) == 0
            ? data.Equals(startWithNode.Data)
                ? startWithNode
                : FindChildren(data, ref startWithNode.MiddleNode)
            : result < 0
                ? FindChildren(data, ref startWithNode.LeftNode)
                : FindChildren(data, ref startWithNode.RightNode);
    }

    /// <summary>
    /// Удаление узла дерева
    /// </summary>
    /// <param name="data">Данные для удаления</param>
    public void Remove(T data)
    {
        var foundNode = FindNode(data);
        Remove(ref foundNode);
    }

    /// <summary>
    /// Удаление узла бинарного дерева
    /// </summary>
    /// <param name="node">Узел для удаления</param>
    public void Remove(ref TernaryTreeNode<T> node)
    {
        if (node == null)
            return;
        
        TernaryTreeNode<T> viceNode = null; // узел-приемник

        // Если есть центральный узел, то передаем ему данные о других узлах
        if (node.MiddleNode != null)
        {
            viceNode = node.MiddleNode;

            // добавление вице-узлу детей
            viceNode.LeftNode = node.LeftNode;
            viceNode.RightNode = node.RightNode;

            // смена у детей родителя
            if(node.LeftNode != null) node.LeftNode.ParentNode = viceNode;
            if(node.RightNode != null) node.RightNode.ParentNode = viceNode;
        }
        // Если есть правый узел, то записываем его вместо удаляемого
        else if (node.RightNode != null)
        {
            viceNode = node.RightNode;

            // Если у удаляемого есть левый узел, то добавляем его в правую ветку
            if(node.LeftNode != null)
            {
                Add(node.LeftNode, viceNode);
            }

            //viceNode.LeftNode = node.LeftNode;
            //// Если узел на замену не является правым узлом, то добавляем ему ссылку на правый узел
            //if (viceNode.Equals(node.RightNode) == false)
            //    viceNode.RightNode = node.RightNode;
        }
        // Если есть левый узел, то записываем его вместо удаляемого
        else if (node.LeftNode != null)
        {
            viceNode = node.LeftNode;
        }
            //// Если есть левый узел, то ищем его максимальный узел и записываем его вместо удаляемого
            //else if (node.LeftNode != null)
            //{
            //    viceNode = FindHighestNode(node.LeftNode);

            //    viceNode.RightNode = node.RightNode;
            //    // Если узел на замену не является левым узлом, то добавляем ему ссылку на левый узел
            //    if (viceNode.Equals(node.LeftNode) == false)
            //        viceNode.LeftNode = node.LeftNode;
            //}

        // Записываем родителю новый узел
        NewNodeToParent(ref node, ref viceNode);

        //var currentNodeSide = node.NodeSide;
        //// если у узла нет подузлов, можно его удалить
        //if (node.LeftNode == null && node.RightNode == null && node.MiddleNode == null)
        //{
        //    NewNodeToParent(node, null);
        //}
        //// если есть центральный, то ставим его на место удаляемого
        //else if(node.MiddleNode != null)
        //{
        //    NewNodeToParent(node, node.MiddleNode);

        //    node.MiddleNode.LeftNode = node.LeftNode;
        //    node.MiddleNode.RightNode = node.RightNode;
        //}
        //// центрального нет, следовательно
        //// если нет левого, то правый ставим на место удаляемого 
        //else if (node.LeftNode == null)
        //{
        //    viceNode = node.MiddleNode ?? node.RightNode;

        //    if (currentNodeSide == TernarySide.Middle) node.ParentNode.MiddleNode = node.MiddleNode;
        //    else if (currentNodeSide == TernarySide.Left) node.ParentNode.LeftNode = viceNode;
        //    else if (currentNodeSide == TernarySide.Right) node.ParentNode.RightNode = viceNode;
        //    else RootNode = viceNode;

        //    if(node.RightNode != null)
        //        node.RightNode.ParentNode = node.MiddleNode ?? node.ParentNode;
        //    if(node.MiddleNode != null)
        //        node.MiddleNode.RightNode = node.RightNode;
        //}
        //// если нет правого, то левый ставим на место удаляемого 
        //else if (node.RightNode == null)
        //{
        //    viceNode = node.MiddleNode ?? node.LeftNode;

        //    if (currentNodeSide == TernarySide.Middle) node.ParentNode.MiddleNode = node.MiddleNode;
        //    else if (currentNodeSide == TernarySide.Left) node.ParentNode.LeftNode = viceNode;
        //    else if (currentNodeSide == TernarySide.Right) node.ParentNode.RightNode = viceNode;
        //    else RootNode = viceNode;

        //    if (node.LeftNode != null)
        //        node.LeftNode.ParentNode = node.MiddleNode ?? node.ParentNode;
        //    if (node.MiddleNode != null)
        //        node.MiddleNode.LeftNode = node.LeftNode;
        //}
        //// если оба дочерних присутствуют, 
        //// то на место удаляемого становится правый или наименьший его потомок,
        //// а левый вставляется в замещаемый
        //else
        //{
        //    viceNode = node.MiddleNode ?? node.RightNode;

        //    switch (currentNodeSide)
        //    {
        //        case TernarySide.Left:
        //            node.ParentNode.LeftNode = viceNode;
        //            viceNode.ParentNode = node.ParentNode;
        //            Add(node.LeftNode, viceNode);
        //            break;
        //        case TernarySide.Right:
        //            node.ParentNode.RightNode = viceNode;
        //            viceNode.ParentNode = node.ParentNode;
        //            Add(node.LeftNode, viceNode);
        //            break;
        //        case TernarySide.Middle:
        //            node.ParentNode.MiddleNode = viceNode;
        //            viceNode.ParentNode = node.ParentNode;
        //            Add(node.LeftNode, viceNode);
        //            break;
        //        default:
        //            RootNode = viceNode;
        //            RootNode.ParentNode = null;
        //            RootNode.LeftNode = node.LeftNode;
        //            node.LeftNode.ParentNode = RootNode;
        //            break;
        //    }

        //    if (node.MiddleNode != null)
        //        Add(node.RightNode, viceNode);
        //}
    }

    public TernaryTreeNode<T> GetLowestNode()
    {
        return FindLowestNode(RootNode);
    }

    /// <summary>
    /// Вывод бинарного дерева
    /// </summary>
    public override string ToString()
    {
        if (RootNode == null)
            return "Tree is empty";

        return TreeToString(RootNode);
    }

    public bool Contains(T data) => FindNode(data) != null;

    public void Clear()
    {
        if (RootNode == null)
            return;

        DeleteChildrens(ref RootNode);
        RootNode = null;
    }

    public bool IsEmpty() => RootNode == null;

    public int Count()
    {
        return Count(RootNode);
    }

    /// <summary>
    /// Добавление узла в нужную ветвь
    /// </summary>
    /// <param name="addedNode">Добавляемый узел</param>
    /// <param name="childrenNode">В какой узел добалять</param>
    /// <returns></returns>
    private TernaryTreeNode<T> AddChildren(TernaryTreeNode<T> addedNode, ref TernaryTreeNode<T> childrenNode)
    {
        return childrenNode == null
            ? (childrenNode = addedNode)
            : Add(addedNode, childrenNode);
    }

    /// <summary>
    /// Поиск узла в определенной ветви
    /// </summary>
    /// <param name="data">Искомое значение</param>
    /// <param name="childrenNode">Узел ветви</param>
    /// <returns></returns>
    private TernaryTreeNode<T> FindChildren(T data, ref TernaryTreeNode<T> childrenNode)
    {
        return childrenNode == null
            ? null
            : FindNode(data, childrenNode);
    }

    private TernaryTreeNode<T> FindLowestNode(TernaryTreeNode<T> node)
    {
        var lowestNode = node;

        if(node.LeftNode != null)
            lowestNode = FindLowestNode(node.LeftNode);

        return lowestNode;
    }

    private TernaryTreeNode<T> FindHighestNode(TernaryTreeNode<T> node)
    {
        var highestNode = node;

        if(node.RightNode != null)
            highestNode = FindHighestNode(node.RightNode);

        return highestNode;
    }

    private void NewNodeToParent(ref TernaryTreeNode<T> currentNode, ref TernaryTreeNode<T> newChildNode)
    {
        if (currentNode.NodeSide == TernarySide.Middle) currentNode.ParentNode.MiddleNode = newChildNode;
        else if (currentNode.NodeSide == TernarySide.Left) currentNode.ParentNode.LeftNode = newChildNode;
        else if (currentNode.NodeSide == TernarySide.Right) currentNode.ParentNode.RightNode = newChildNode;
        else RootNode = newChildNode;

        if(newChildNode != null)
            newChildNode.ParentNode = currentNode.ParentNode;
    }

    /// <summary>
    /// Вывод бинарного дерева начиная с указанного узла
    /// </summary>
    /// <param name="startNode">Узел с которого начинается печать</param>
    /// <param name="indent">Отступ</param>
    /// <param name="side">Сторона</param>
    private string TreeToString(TernaryTreeNode<T> startNode, string indent = "", TernarySide? side = null)
    {
        if (startNode == null)
            return null;

        var result = new StringBuilder();
        var nodeSide = side == null 
            ? "+"
            : side == TernarySide.Middle
                ? "M"
                : side == TernarySide.Left 
                    ? "L" 
                    : "R";
        result.Append($"{indent} [{nodeSide}] {startNode.Data}, <color=grey>p: {startNode.ParentNode}</color>\n");
        indent += new string(' ', 3);
        //рекурсивный вызов для веток
        result.Append(TreeToString(startNode.LeftNode, indent, TernarySide.Left));
        result.Append(TreeToString(startNode.MiddleNode, indent, TernarySide.Middle));
        result.Append(TreeToString(startNode.RightNode, indent, TernarySide.Right));

        return result.ToString();
    }

    private int Count(TernaryTreeNode<T> startNode)
    {
        if(startNode == null)
            return 0;

        var result = 1;
        //рекурсивный вызов для веток
        result += Count(startNode.LeftNode);
        result += Count(startNode.RightNode);
        result += Count(startNode.MiddleNode);

        return result;
    }

    private void DeleteChildrens(ref TernaryTreeNode<T> node)
    {
        if(node.RightNode != null)
            DeleteChildrens(ref node.RightNode);
        if(node.LeftNode != null)
            DeleteChildrens(ref node.LeftNode);
        if(node.MiddleNode != null)
            DeleteChildrens(ref node.MiddleNode);

        Remove(ref node);
    }
}


public class TernaryTreeDB<T> : TernaryTree<T>, IPathFindDataBase<T> where T : IComparable<T>
{
    private TernaryTreeNode<T> _prevLowest;

    public T GetLowest()
    {
        var lowestNode = RootNode;

        while(lowestNode.LeftNode != null)
            lowestNode = lowestNode.LeftNode;

        while(lowestNode.MiddleNode != null)
            lowestNode = lowestNode.MiddleNode;

        if(lowestNode.Equals(_prevLowest))
            Debug.Log($"<color=yellow>lowest: {lowestNode}</color>\n" +
                $"parent: {lowestNode.ParentNode}");

        _prevLowest = lowestNode;
        return lowestNode.Data;
    }
}
