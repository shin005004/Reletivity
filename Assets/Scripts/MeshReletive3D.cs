using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshReletive3D : MonoBehaviour
{
    [Header("Reletive to Player")]
    public GameObject reletiveTarget;
    public Vector3 reletiveVector;
    public Vector3 reletiveSpeed;

    [Tooltip("For Debug purposes")] public bool onMeshUpdate = false;

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private Vector3[] originalVerticies;

    public GameState gameState;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        // Mesh logic
        mesh = GetComponent<MeshFilter>().mesh;
        originalVerticies = mesh.vertices;
        for (var i = 0; i < originalVerticies.Length; i++)
            originalVerticies[i] = Vector3.Scale(originalVerticies[i], transform.localScale) + transform.position;
        mesh.MarkDynamic();

        // By default 
        if (reletiveTarget == null)
        {
            reletiveTarget = GameObject.Find("Player");
            gameState = reletiveTarget.GetComponent<GameState>();
        }
        if (reletiveTarget == null)
            Debug.LogWarning(gameObject.name + "'s Reletive Target Not Found");
    }

    private void Update()
    {
        meshRenderer.enabled = true;
        if (onMeshUpdate)
            UpdateMesh();
    }

    // 로렌츠 인자 계산 함수
    private float EvaluateGamma(float objectSpeed, float lightSpeed)
    {
        float gamma;
        if (objectSpeed > lightSpeed)
            gamma = float.MaxValue;
        else
        {
            gamma = 1 / Mathf.Sqrt(1 - Mathf.Pow(objectSpeed / lightSpeed, 2f));
            gamma = Mathf.Max(gamma, 1.0f);
        }

        return gamma;
    }

    private void UpdateMesh()
    {
        // 초기에는 오류를 피하기 위해서 로딩하지 않는다
        if (gameState.PlayerVelocityVector.magnitude < 0.0001f)
            return;

        // 절대 좌표를 복사해서 가져옴
        Vector3[] newVerticies = new Vector3[originalVerticies.Length];

        // 모든 꼭짓점에 대해서 적용
        for (var i = 0; i < originalVerticies.Length; i++)
        {
            // 플레이어와의 상대 위치를 구한다
            reletiveVector = originalVerticies[i] - reletiveTarget.transform.position;

            // 플레이어의 속도에 대해 상대 위치의 정사영을 잡는다
            // projection of distance vector and horizontal
            Vector3 playerSpeed = gameState.PlayerVelocityVector;
            Vector3 verticalVector = Vector3.Dot(reletiveVector, playerSpeed) / playerSpeed.magnitude * playerSpeed.normalized;
            Vector3 horizontalVector = reletiveVector - verticalVector;

            // 이를 이용하여 길이수축이 적용된 절대위치를 잡고 이를 다시 상대 위치로 움직여준다
            newVerticies[i] = Vector3.Scale(reletiveTarget.transform.position + horizontalVector + verticalVector /
                EvaluateGamma(playerSpeed.magnitude, (float)gameState.SpeedOfLight) - transform.position,
                new Vector3(1.0f / transform.localScale.x, 1.0f / transform.localScale.y, 1.0f / transform.localScale.z));
        }

        // 새로운 상대 위치를 Mesh에 적용시켜 새롭게 만듬
        mesh.vertices = newVerticies;
    }
}
