using UnityEngine;

public class ItemAnimation : MonoBehaviour , IIGetItem
{ 
    private float floatSpeed = 1.0f;
    private float floatAmplitude = 0.1f;
    private Vector3 rotateSpeed = new Vector3(0f, 0f, 60f); 

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }
    
}
