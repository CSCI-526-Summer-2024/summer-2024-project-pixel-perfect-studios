using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingCivilian : MonoBehaviour
{
    public float waveSpeed = 1f;
    public float waveAngle = 60f;
    private float waveTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        waveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        waveTime += Time.deltaTime * waveSpeed;
        float angle = Mathf.Sin(waveTime) * waveAngle + 90;
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
