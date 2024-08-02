using UnityEngine;

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

    public void Attack()
    {
        // 사정거리 내 적이 있다면 자동공격
        _attackTimer += Time.deltaTime;

        if (_player.moveModule.isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            if (_player.diceGrid.diceUnitGrid.ContainsKey(_player.positionKey + Vector2Int.left))
            {
                Debug.Log("Left Attack");
                _attackTimer = 0f;
                _player.animator.Play("NormalAttack");
            }
            else if (_player.diceGrid.diceUnitGrid.ContainsKey(_player.positionKey + Vector2Int.right))
            {
                Debug.Log("Right Attack");
                _attackTimer = 0f;
                _player.animator.Play("Move");
                _player.animator.Play("NormalAttack");
            }
        }
    }
}
