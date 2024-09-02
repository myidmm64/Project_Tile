using System;
using UnityEngine;

public class TestMouseJKJ : MonoBehaviour
{
    [SerializeField]
    private DiceGrid _grid = null;

    [SerializeField]
    private Transform centerTrm = null;
    [SerializeField]
    private float _backScale = 0.7f;

    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _grid.diceUnitGrid[new Vector2Int(2, 2)].GetComponent<IDamagable>().Damage(5);
            Debug.DrawLine(transform.position, _grid.diceUnitGrid[new Vector2Int(2, 2)].transform.position, Color.red, 0.5f);
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3 targetVector = (mouseWorldPos - centerTrm.position).normalized;
        transform.position = centerTrm.transform.position + (-targetVector * _backScale);
    }
}
