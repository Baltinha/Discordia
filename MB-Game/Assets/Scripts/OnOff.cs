using System.Collections;
using UnityEngine;

public class OnOff : MonoBehaviour
{
    [SerializeField]private float m_temp;
    private Light m_light;
    private bool m_lights;

    void Start()
    {
        m_light = GetComponentInChildren<Light>();
    }

    void FixedUpdate()
    {
        StartCoroutine(LightBroken(m_temp));
    }
    private IEnumerator LightBroken(float value)
    {
        if (m_lights)
        {
            m_light.enabled = false;
            m_lights = false;
        }
        yield return new WaitForSeconds(value);
        if (!m_lights)
        {
            m_light.enabled = true;
            m_lights = true;
        }
        yield return null;

    }
}
