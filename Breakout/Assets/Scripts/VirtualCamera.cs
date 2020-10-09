using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCamera : MonoBehaviour
{

    public CinemachineVirtualCamera cam { get; private set; }
    PlayerMovement player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) player = FindObjectOfType<PlayerMovement>();
        if (cam.Follow == null) cam.Follow = player.transform;
    }
}
