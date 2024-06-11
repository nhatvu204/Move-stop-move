using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public static CamController Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public float offsetZ = 13f;

    public float offsetY = 13f;

    Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        //Get player transform component
        playerPos = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(playerPos.position.x, playerPos.position.y + offsetY, playerPos.position.z - offsetZ);

        transform.position = targetPosition;
    }
}
