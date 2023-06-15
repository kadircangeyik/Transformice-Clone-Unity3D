using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    GameObject player;
    float helpCameraFollowFaster = 3;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x, player.transform.position.x, Time.deltaTime * helpCameraFollowFaster),
                                               Mathf.Lerp(this.transform.position.y, player.transform.position.y, Time.deltaTime * helpCameraFollowFaster), 0);
    }
}