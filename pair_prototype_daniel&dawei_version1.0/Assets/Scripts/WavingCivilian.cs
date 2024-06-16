using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingCivilian : MonoBehaviour
{
    public float waveSpeed;
    public float minWaveSpeed = 0.5f;
    public float maxWaveSpeed = 2f;
    public float waveAngle = 60f;
    private float waveTime;

    // Start is called before the first frame update
    void Awake()
    {
        waveTime = Random.Range(0f, Mathf.PI * 2f);
        waveSpeed = Random.Range(minWaveSpeed, maxWaveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        waveTime += Time.deltaTime * waveSpeed;
        float angle = Mathf.Sin(waveTime) * waveAngle + 90;
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
}
