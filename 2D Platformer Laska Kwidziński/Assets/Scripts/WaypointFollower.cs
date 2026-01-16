using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;   // a
    private int currentWaypoint = 0;                   // b
    [SerializeField] private float speed = 1.0f;       // c

    void Update()
    {
        // d — obliczanie odleg³oœci do bie¿¹cego waypointu
        float distance = Vector2.Distance(
            transform.position,
            waypoints[currentWaypoint].transform.position
        );

        // e — jeœli platforma dotar³a blisko waypointu, wybierz nastêpny
        if (distance < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        // f — obliczenie nowej pozycji i ustawienie jej
        Vector2 newPos = Vector2.MoveTowards(
            transform.position,
            waypoints[currentWaypoint].transform.position,
            speed * Time.deltaTime
        );

        transform.position = newPos; // ustawienie pozycji
    }
}
