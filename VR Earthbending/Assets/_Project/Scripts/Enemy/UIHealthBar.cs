using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public Image foregroundImage;
    public Image backgroundImage;

    private void LateUpdate()
    {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehindCamera = Vector3.Dot(direction, Camera.main.transform.forward) <= 0f;
        foregroundImage.enabled = !isBehindCamera;
        backgroundImage.enabled = !isBehindCamera;


        transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }

    public void SetHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        // Debug.Log("parent: " + parentWidth);
        // Debug.Log("width: " + width);

        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }
}
