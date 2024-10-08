using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public FloatValue playerCurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for(int i = 0; i< heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    public void UpdateHearts()
    {
        float tempHealth = playerCurrentHealth.initialValue / 2;
        int fullHearts = Mathf.FloorToInt(tempHealth);
        bool hasHalfHeart = (tempHealth - fullHearts) > 0;

        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            if (i < fullHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (i == fullHearts && hasHalfHeart)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

}
