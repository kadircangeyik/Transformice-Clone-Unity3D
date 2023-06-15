using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecking : MonoBehaviour {

    LayerMask isWall = default;
    [HideInInspector]
    public bool touching = false;
    Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWall == (isWall | 1 << collision.gameObject.layer))
        {
            touching = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isWall == (isWall | 1 << collision.gameObject.layer))
        {
            if (col.IsTouchingLayers(isWall))
            {
                return;
            }
            touching = false;
        }
    }

}
