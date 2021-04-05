using UnityEngine;

public class Arrow : MonoBehaviour
{
    public bool DynamicLength;

    public Transform player;
    public Transform host;

    private Vector3 scaleChange;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Awake()
    {
        scaleChange = new Vector3(2.5f, 1f, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        transform.LookAt(host);
        if (DynamicLength)
        {
            distance = Vector3.Distance(player.position, host.position);
            scaleChange.z = distance / 8 + 2.5f;
            transform.localScale = scaleChange;
        }
    }
}
