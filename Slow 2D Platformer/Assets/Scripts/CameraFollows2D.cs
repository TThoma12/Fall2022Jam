using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollows2D : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    public float timeOffset;

    [SerializeField]
    Vector2 posOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This is for the camera's current position
        Vector3 startPos = transform.position;

        // This is for the player's current position
        Vector3 endPos = player.transform.position;
        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -10;

        transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);
    }
}
