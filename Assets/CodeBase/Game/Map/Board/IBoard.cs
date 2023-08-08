using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public interface IBoard
    {
        public void Initialize(Vector2Int size);
    }
}