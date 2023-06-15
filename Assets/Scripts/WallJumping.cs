using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumping : MonoBehaviour {

    PlayerControlScript player;

	// Use this for initialization
	void Start () {
        player = GetComponent<PlayerControlScript>();
	}

    // Update is called once per frame
    

    void FixedUpdate () {

        Vector2 vel = player.rb2.velocity;

        if (player.onWall && !player.grounded && Input.GetButtonDown("Jump"))
        {
            player.lastWJumpTime = Time.time;
            player.wJump = true;
            player.falling = true;
            vel.x = player.facingRight ? -15f : 15f;
            vel.y = 10.2f;
            player.rb2.velocity = vel;
            player.Flip();
        }

	}
}
