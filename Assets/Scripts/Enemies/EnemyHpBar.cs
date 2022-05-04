using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHpBar : MonoBehaviour
{
    private Slider hpBar;
    private Color32 high;
    private Color32 low;

    private float timeHiden = 2f;
    private float count;
    private void Awake()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        hpBar = GetComponentInChildren<Slider>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.worldCamera = Camera.main;
        }
        if (hpBar != null)
        {
            Vector3 offset = new Vector3(0f, 0.7f, 0f);
            hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);
        }
        high = new Color32(104, 219, 142,255);
        low = new Color32(217, 69, 44, 255);
        count = timeHiden;
    }
    public void configMaxHP(int Hp) {
        hpBar.maxValue = Hp;
        setHp(Hp);
    }

    public void setHp(int Hp) {
        hpBar.value = Hp;
        hpBar.fillRect.GetComponent<Image>().color = Color.Lerp(low, high, hpBar.normalizedValue);
        hpBar.gameObject.SetActive(true);
        count = timeHiden;

    }

    private void Update()
    {
        if (count >= 0 && hpBar != null)
        {
            Vector3 offset = new Vector3(0f, 0.7f, 0f);
            hpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + offset);

        }
        count -= Time.deltaTime;
        if (count < 0) {
            hpBar.gameObject.SetActive(false);
        }
        
    }
}
