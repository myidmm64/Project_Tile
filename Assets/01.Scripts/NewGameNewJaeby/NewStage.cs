using System.Collections.Generic;
using UnityEngine;

public class NewStage : MonoBehaviour
{
    public Vector2Int playerPosKey = Vector2Int.zero;
    public NewPlayer player; // 임ㅅㅣ
    public List<StageUnit> stageUnits = new List<StageUnit>();

    public NewDice dice; // 임시
    [SerializeField]
    private Transform _diceParent = null;
    [SerializeField, TextArea]
    private string _map = null;
    public Dictionary<Vector2Int, NewDice> Grid { get; protected set; }

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    public virtual void InitializeStage()
    {
        foreach (StageUnit unit in stageUnits)
        {
            StageUnit spawned = Instantiate(unit, transform);
            spawned.StartStage();
        }

        player.StartStage();

        // player.ChangeDice();
    }

    [ContextMenu("그리드 설정")]
    public void SetDiceGrid()
    {
        if (Grid != null) Grid.Clear();

        List<NewDice> dices = new List<NewDice>();
        dices.AddRange(_diceParent.GetComponentsInChildren<NewDice>());
        foreach(NewDice dice in dices)
        {
            DestroyImmediate(dice.gameObject);
        }

        Grid = new Dictionary<Vector2Int, NewDice>();

        // string parsing
        string[] rows = _map.Split('\n'); // 줄 단위로 나누기
        for (int y = 0; y < rows.Length; y++)
        {
            string row = rows[y].Trim(); // 공백 제거
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] == '1') // '1'이면 오브젝트 생성
                {
                    Vector3 position = new Vector3(x * 1f, -y * 1f);
                    NewDice spawnedDice = Instantiate(dice, position, Quaternion.identity, _diceParent);

                    Vector2Int posKey = new Vector2Int(x + 1, rows.Length - y);
                    spawnedDice.name = $"{posKey.x} {posKey.y}";
                    spawnedDice.posKey = posKey;

                    Grid.Add(posKey, spawnedDice);
                }
            }
        }

        foreach(var dice in Grid.Values)
        {
            Debug.Log($"Dice : {dice.posKey}");
        }
    }
}
