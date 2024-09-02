using UnityEngine;

public class TestArrc : MonoBehaviour
{
    public int segments = 50;         // 호를 그릴 때 사용할 점의 개수
    public float maxRadius = 5f;      // 호의 최대 반지름 (샷건의 최대 퍼짐)
    public float spreadAngle = 30f;   // 탄 퍼짐 각도
    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRenderer 컴포넌트 가져오기
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1; // 점의 개수 설정
        lineRenderer.useWorldSpace = true;         // 월드 좌표계를 사용하도록 설정
    }

    void Update()
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        mousePosition.z = 0; // z 축을 0으로 설정 (2D 환경에서의 사용을 가정)

        // 플레이어 위치
        Vector3 playerPosition = transform.position;

        // 마우스와 플레이어 위치 간의 각도 계산
        float angleToMouse = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x) * Mathf.Rad2Deg;

        // 탄 퍼짐을 그리기
        DrawArcWithLineRenderer(playerPosition, angleToMouse);
    }

    void DrawArcWithLineRenderer(Vector3 center, float angleToMouse)
    {
        float angleStep = spreadAngle / segments; // 각도 단위 (퍼짐 각도를 점 개수로 나누기)

        for (int i = 0; i <= segments; i++)
        {
            // 현재 각도 계산
            float currentAngle = angleToMouse - (spreadAngle / 2) + (i * angleStep);
            float currentAngleRad = Mathf.Deg2Rad * currentAngle; // 도를 라디안으로 변환

            // 점 위치 계산
            float x = center.x + Mathf.Cos(currentAngleRad) * maxRadius;
            float y = center.y + Mathf.Sin(currentAngleRad) * maxRadius;

            // LineRenderer에 점 추가
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}
