using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingObject : MonoBehaviour
{
    public Transform object1; // Reference to the first object
    public Transform object2; // Reference to the second object
    private float speed = 2f; // Speed of the movement

    private float initialDistance; // Initial distance between the objects
    private GameSetting info;
    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        speed = info.speed * 4f;
    }
    void Start()
    {
        initialDistance = Vector3.Distance(object1.position, object2.position); // Calculate initial distance
    }

    void Update()
    {
        float moveAmount = speed * Time.deltaTime; // Amount to move the objects

        // Move the objects
        object1.Translate(Vector3.back * moveAmount);
        object2.Translate(Vector3.back * moveAmount);

        // Check if objects have moved beyond visible area
        if (object1.position.z < 0f)
        {
            object1.position += new Vector3(0f, 0f, object2.position.z + initialDistance);
        }

        if (object2.position.z < 0f)
        {
            object2.position += new Vector3(0f, 0f, object1.position.z + initialDistance);
        }
    }
}
