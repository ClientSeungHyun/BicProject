using UnityEngine;
using System.Collections.Generic;

public class Flock : MonoBehaviour {

    // 박쥐 프리팹
    public GameObject batPrefab;

    // 박쥐 개체 수
    public int flockSize = 20;

    // 박쥐 이동 속도
    public float speed = 5.0f;

    // 박쥐 회전 속도
    public float rotationSpeed = 4.0f;

    // 박쥐 간 최소 거리
    public float neighborDistance = 3.0f;

    // 박쥐 개체 리스트
    private List<GameObject> bats = new List<GameObject>();

    private GameManagers gameManagerScript;    //게임 매니저 스크립트

    // Start is called before the first frame update
    void Start() {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagers>();
        // 박쥐 개체 생성
        for (int i = 0; i < flockSize; i++) {
            Vector3 position = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
            GameObject bat = Instantiate(batPrefab, transform.position + position, Quaternion.identity);
            bat.transform.parent = transform;
            bats.Add(bat);
        }
    }

    // Update is called once per frame
    void Update() {

        if (gameManagerScript.IsPlaying())
        {
            foreach (GameObject bat in bats)
            {
                // 주변 박쥐 개체 리스트
                List<GameObject> neighbors = new List<GameObject>();
                foreach (GameObject other in bats)
                {
                    if (other != bat && Vector3.Distance(other.transform.position, bat.transform.position) <= neighborDistance)
                    {
                        neighbors.Add(other);
                    }
                }

                // 규칙 적용
                Vector3 separation = SeparationRule(bat, neighbors);
                Vector3 alignment = AlignmentRule(bat, neighbors);
                Vector3 cohesion = CohesionRule(bat, neighbors);

                // 박쥐 속도 조정
                Vector3 velocity = separation + alignment + cohesion;
                velocity = Vector3.ClampMagnitude(velocity, speed);
                bat.GetComponent<Rigidbody>().velocity = velocity;

                // 박쥐 방향 조정
                if (velocity != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(velocity);
                    bat.transform.rotation = Quaternion.Slerp(bat.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                }

                // 박쥐 위치 조정
                bat.transform.position += velocity * Time.deltaTime;
            }
        }
    }

    // 분리 규칙
    Vector3 SeparationRule(GameObject bat, List<GameObject> neighbors) {
        Vector3 separation = Vector3.zero;
        if (neighbors.Count == 0) {
            return separation;
        }
        foreach (GameObject neighbor in neighbors) {
            Vector3 direction = bat.transform.position - neighbor.transform.position;
            float distance = Vector3.Distance(bat.transform.position, neighbor.transform.position);
            separation += direction / distance;
        }
        separation /= neighbors.Count;
        separation = separation.normalized;
        return separation;
    }

    // 집합 규칙
    Vector3 CohesionRule(GameObject bat, List<GameObject> neighbors) {
        Vector3 cohesion = Vector3.zero;
        if (neighbors.Count == 0) {
            return cohesion;
        }
        foreach (GameObject neighbor in neighbors) {
            cohesion += neighbor.transform.position;
        }
        cohesion /= neighbors.Count;
        cohesion = cohesion - bat.transform.position;
        cohesion = Vector3.ClampMagnitude(cohesion, speed);
        return cohesion;
    }
    // 일치 규칙
    Vector3 AlignmentRule(GameObject bat, List<GameObject> neighbors) {
        Vector3 alignment = Vector3.zero;
        if (neighbors.Count == 0) {
            return alignment;
        }
        foreach (GameObject neighbor in neighbors) {
            alignment += neighbor.GetComponent<Rigidbody>().velocity;
        }
        alignment /= neighbors.Count;
        alignment = Vector3.ClampMagnitude(alignment, speed);
        return alignment;
    }
}