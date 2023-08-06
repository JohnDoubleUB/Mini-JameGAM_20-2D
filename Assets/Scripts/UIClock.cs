using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClock : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private Animator animator;

    int currentTime = 0;

    public void SetTime(int time)
    {
        int validTime = Mathf.Clamp(time - 1, 0, sprites.Length - 1);
        if (validTime == currentTime) return;

        currentTime = validTime;
        animator.Play("Tick");
        image.sprite = sprites[validTime];
    }

    private void Update()
    {
        SetTime(Mathf.CeilToInt(GameManager.current.CurrentGameTimer));
    }
}
