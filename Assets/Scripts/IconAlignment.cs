using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAlignment : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;
    private void Update()
    {
        print(rectTransform.sizeDelta.x);
    }
}
