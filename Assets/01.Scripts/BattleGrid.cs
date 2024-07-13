using System;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    [SerializeField]
    private Vector2Int _gridSize = Vector2Int.zero;

    private void Start()
    {
        GenerateDice();
    }

    private void GenerateDice()
    {

    }
}
