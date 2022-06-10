using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class translateAnimationZ : MonoBehaviour
{
    public GameObject quad;
    void Update()
    {
        float maxValue = 0.31f;
        try
        {
            var vectorUp = Vector3.up * (Time.deltaTime / 28);
            var vectorDown = Vector3.down * maxValue;

            var y = quad.transform.position.y;
            if (y > maxValue)
            {
                quad.transform.Translate(vectorDown);
                Debug.Log(vectorDown);
            }
            else
            {
                quad.transform.Translate(vectorUp);
            }
        }
        catch (Exception e)
        {}
    }
}
