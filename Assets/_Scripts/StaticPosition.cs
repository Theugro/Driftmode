using UnityEngine;
using System.Collections;

public class StaticPosition : MonoBehaviour {

    public Transform playerTransform;

    void FixedUpdate()
    {
        transform.position = playerTransform.position + new Vector3(0, 0, -10);
    }
}
