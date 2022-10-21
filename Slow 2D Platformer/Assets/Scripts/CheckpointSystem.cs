using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    private Collider2D chkptCollider;
    public bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        chkptCollider = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !collected)
        {
            Debug.Log("Set state to collected.");
            collected = true;
            chkptCollider.enabled = !chkptCollider.enabled;
        }
    }
}
