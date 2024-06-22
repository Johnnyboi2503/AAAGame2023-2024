using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnim : MonoBehaviour
{
    [SerializeField] float amplitude;
    [SerializeField] float ocilationSpeed;
    float timer = 0;
    Vector3 initalPos;
    // Start is called before the first frame update
    void Start()
    {
        initalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime*ocilationSpeed;
        transform.position = initalPos + (Vector3.up * amplitude * Mathf.Sin(timer));
    }
}
