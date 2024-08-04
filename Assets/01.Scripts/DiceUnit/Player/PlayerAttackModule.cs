using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttackModule : MonoBehaviour
{
    // 플레이어 기본 공격
    private PlayerDiceUnit _player = null;

    [SerializeField]
    private float _attackDelay = 0.5f;
    private float _attackTimer = 0f;
    private void Awake()
    {
        _player = GetComponent<PlayerDiceUnit>();
    }

    public List<DiceUnit> GetAttackTarget() // 공격 반경 내 들어온 녀석들 걸러내기
    {

        return null;
    }

    public void Attack()
    {
        // 사정거리 내 적이 있다면 자동공격
        _attackTimer += Time.deltaTime;

        if (_player.moveModule.isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            if (_player.diceGrid.diceUnitGrid.ContainsKey(_player.positionKey + Vector2Int.right))
            {
                _player.diceGrid.diceUnitGrid[_player.positionKey + Vector2Int.right].Damage(_player.diceGrid.grid[_player.positionKey].dicePip);

                Debug.Log("Right Attack");
                _attackTimer = 0f;
                _player.animator.Play("Move");
                _player.animator.Play("NormalAttack");
            }
        }
    }

    public void AttackInput()
    {

    }
}
