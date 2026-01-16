using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    private const int PLATFORMS_NUM = 10;

    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private float moveSpeed = 0.2f;

    private GameObject[] platforms;
    private Vector2[] positions;
    private int[] currentTargetIndex;

    private void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector2[PLATFORMS_NUM];
        currentTargetIndex = new int[PLATFORMS_NUM];

        float radius = 1f;

        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            float angle = (2 * Mathf.PI / PLATFORMS_NUM) * i;
            positions[i] = new Vector2(
                Mathf.Cos(angle) * radius + this.transform.position.x,
                Mathf.Sin(angle) * radius + this.transform.position.y
            );

            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
            platforms[i].tag = "MovingPlatform";

            // początkowy cel to następna pozycja na okręgu
            currentTargetIndex[i] = (i + 1) % PLATFORMS_NUM;
        }
    }

    private void Update()
    {
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            // przesuwanie platformy do aktualnego celu
            //UnityEngine.Debug.Log("obracanie" + i);
            platforms[i].transform.position = Vector2.MoveTowards(
                platforms[i].transform.position,
                positions[currentTargetIndex[i]],
                moveSpeed * Time.deltaTime
            );

            // jeśli platforma dotarła do celu, ustaw nowy cel
            if ((Vector2)platforms[i].transform.position == positions[currentTargetIndex[i]])
            {
                currentTargetIndex[i] = (currentTargetIndex[i] + 1) % PLATFORMS_NUM;
            }
        }
    }
}
