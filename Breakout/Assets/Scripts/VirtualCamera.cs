using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamera : MonoBehaviour
{

    public CinemachineVirtualCamera cam { get; private set; }
    PlayerMovement player;
    //Animator anim;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        cam = GetComponent<CinemachineVirtualCamera>();
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        if (cam.Follow == null) cam.Follow = player.transform;

        Animations();
    }

    void Animations()
    {

    }
}
