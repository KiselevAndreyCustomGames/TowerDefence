using GridPathfindingSystem;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Расположения узла относительно родителя
/// </summary>
public enum Side
{
    Left,
    Right
}

/// <summary>
/// Узел бинарного дерева
/// </summary>
/// <typeparam name="T"></typeparam>
public class BinaryTreeNode<T> where T : IComparable<T>
{
    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="data">Данные</param>
    public BinaryTreeNode(T data)
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
    public BinaryTreeNode<T> LeftNode { get; set; }

    /// <summary>
    /// Правая ветка
    /// </summary>
    public BinaryTreeNode<T> RightNode { get; set; }

    /// <summary>
    /// Родитель
    /// </summary>
    public BinaryTreeNode<T> ParentNode { get; set; }

    /// <summary>
    /// Расположение узла относительно его родителя
    /// </summary>
    public Side? NodeSide =>
        ParentNode == null
        ? null
        : ParentNode.LeftNode == this
            ? Side.Left
            : Side.Right;

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
public class BinaryTree<T> where T : IComparable<T>
{
    /// <summary>
    /// Корень бинарного дерева
    /// </summary>
    public BinaryTreeNode<T> RootNode { get; set; }
    
    /// <summary>
    /// Добавление данных в бинарное дерево
    /// </summary>
    /// <param name="data">Данные</param>
    /// <returns>Узел</returns>
    public void Add(T data)
    {
        Add(new BinaryTreeNode<T>(data), null);
    }


    /// <summary>
    /// Добавление данных в бинарное дерево с возвратом добавленной ноды
    /// </summary>
    /// <param name="data">Данные</param>
    /// <returns>Узел</returns>
    public BinaryTreeNode<T> AddAndReturnNode(T data)
    {
        return Add(new BinaryTreeNode<T>(data), null);
    }

    /// <summary>
    /// Добавление нового узла в бинарное дерево
    /// </summary>
    /// <param name="node">Новый узел</param>
    /// <param name="currentNode">Текущий узел</param>
    /// <returns>Узел</returns>
    public BinaryTreeNode<T> Add(BinaryTreeNode<T> node, BinaryTreeNode<T> currentNode)
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
            ? currentNode
            : result < 0
                ? currentNode.LeftNode == null
                    ? (currentNode.LeftNode = node)
                    : Add(node, currentNode.LeftNode)
                : currentNode.RightNode == null
                    ? (currentNode.RightNode = node)
                    : Add(node, currentNode.RightNode);
    }

    /// <summary>
    /// Поиск узла по значению
    /// </summary>
    /// <param name="data">Искомое значение</param>
    /// <returns>Найденный узел</returns>
    public BinaryTreeNode<T> FindNode(T data)
    {
        return FindNode(data, null);
    }

    /// <summary>
    /// Поиск узла по значению
    /// </summary>
    /// <param name="data">Искомое значение</param>
    /// <param name="startWithNode">Узел начала поиска</param>
    /// <returns>Найденный узел</returns>
    public BinaryTreeNode<T> FindNode(T data, BinaryTreeNode<T> startWithNode)
    {
        if (RootNode == null)
            return null;

        startWithNode ??= RootNode;
        int result;
        return (result = data.CompareTo(startWithNode.Data)) == 0
            ? startWithNode
            : result < 0
                ? startWithNode.LeftNode == null
                    ? null
                    : FindNode(data, startWithNode.LeftNode)
                : startWithNode.RightNode == null
                    ? null
                    : FindNode(data, startWithNode.RightNode);
    }

    /// <summary>
    /// Удаление узла бинарного дерева
    /// </summary>
    /// <param name="node">Узел для удаления</param>
    public void Remove(BinaryTreeNode<T> node)
    {
        if (node == null)
            return;

        var currentNodeSide = node.NodeSide;
        //если у узла нет подузлов, можно его удалить
        if (node.LeftNode == null && node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
                node.ParentNode.LeftNode = null;
            else if (currentNodeSide == Side.Right)
                node.ParentNode.RightNode = null;
            else
                RootNode = null;
        }
        //если нет левого, то правый ставим на место удаляемого 
        else if (node.LeftNode == null)
        {
            if (currentNodeSide == Side.Left)
                node.ParentNode.LeftNode = node.RightNode;
            else if (currentNodeSide == Side.Right)
                node.ParentNode.RightNode = node.RightNode;
            else
                RootNode = node.RightNode;

            node.RightNode.ParentNode = node.ParentNode;
        }
        //если нет правого, то левый ставим на место удаляемого 
        else if (node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
                node.ParentNode.LeftNode = node.LeftNode;
            else if (currentNodeSide == Side.Right)
                node.ParentNode.RightNode = node.LeftNode;
            else
                RootNode = node.LeftNode;

            node.LeftNode.ParentNode = node.ParentNode;
        }
        //если оба дочерних присутствуют, 
        //то правый становится на место удаляемого,
        //а левый вставляется в правый
        else
        {
            switch (currentNodeSide)
            {
                case Side.Left:
                    node.ParentNode.LeftNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                case Side.Right:
                    node.ParentNode.RightNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                default:
                    RootNode = node.RightNode;
                    RootNode.ParentNode = null;
                    RootNode.LeftNode = node.LeftNode;
                    node.LeftNode.ParentNode = RootNode;
                    break;
            }
        }
    }

    /// <summary>
    /// Удаление узла дерева
    /// </summary>
    /// <param name="data">Данные для удаления</param>
    public void Remove(T data)
    {
        var foundNode = FindNode(data);
        Remove(foundNode);
    }

    /// <summary>
    /// Вывод бинарного дерева
    /// </summary>
    public override string ToString()
    {
        return TreeToString(RootNode);
    }

    public bool Contains(T data) => FindNode(data) != null;

    public void Clear()
    {
        if (RootNode == null)
            return;

        DeleteChildrens(RootNode);
    }

    public bool IsEmpty() => RootNode == null;

    /// <summary>
    /// Вывод бинарного дерева начиная с указанного узла
    /// </summary>
    /// <param name="startNode">Узел с которого начинается печать</param>
    /// <param name="indent">Отступ</param>
    /// <param name="side">Сторона</param>
    private string TreeToString(BinaryTreeNode<T> startNode, string indent = "", Side? side = null)
    {
        if (startNode != null)
        {
            var result = new StringBuilder();
            var nodeSide = side == null ? "+" : side == Side.Left ? "L" : "R";
            result.Append($"{indent} [{nodeSide}]- {startNode.Data}\n");
            indent += new string(' ', 3);
            //рекурсивный вызов для левой и правой веток
            result.Append(TreeToString(startNode.LeftNode, indent, Side.Left));
            result.Append(TreeToString(startNode.RightNode, indent, Side.Right));

            return result.ToString();
        }

        return null;
    }

    private void DeleteChildrens(BinaryTreeNode<T> node)
    {
        if(node.RightNode != null)
            DeleteChildrens(node.RightNode);
        if(node.LeftNode != null)
            DeleteChildrens(node.LeftNode);

        Remove(node);
    }
}


public class BinaryTreeDB<T> : BinaryTree<T>, IPathFindDataBase<T> where T : IComparable<T>
{
    public T GetLowest()
    {
        var lowestFCostTreeNode = RootNode;

        while (lowestFCostTreeNode.LeftNode != null)
        {
            lowestFCostTreeNode = lowestFCostTreeNode.LeftNode;
        }

        return lowestFCostTreeNode.Data;
    }
}

public interface IPathFindDataBase<T>: IPathFindData<T> where T : IComparable<T>
{
    public T GetLowest();
}

public interface IPathFindData<T> where T : IComparable<T>
{
    public void Add(T data);
    public void Remove(T data);
    public void Clear();
    public bool Contains(T data);
    public bool IsEmpty();
}

public class PathList<T>: IPathFindDataBase<T> where T : IComparable<T>
{
    private readonly List<T> _list = new();

    public void Add(T data) => _list.Add(data);
    public void Remove(T data) => _list.Remove(data);
    public void Clear() => _list.Clear();
    public bool Contains(T data) => _list.Contains(data);
    public bool IsEmpty() => _list.Count == 0;

    public T GetLowest()
    {
        T lowestNode = _list[0];

        foreach(T node in _list)
        {
            if(node.CompareTo(lowestNode) < 0)
                lowestNode = node;
        }

        return lowestNode;
    }
}
