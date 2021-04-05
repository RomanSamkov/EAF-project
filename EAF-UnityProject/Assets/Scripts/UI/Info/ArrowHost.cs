using UnityEngine;

public class ArrowHost : MonoBehaviour
{
    public float detect_distance;

    public Transform player;
    public GameObject arrow;

    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        arrow.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance < detect_distance)
        {
            arrow.SetActive(true);
        }
        else
        {
            arrow.SetActive(false);
        }
    }
}
