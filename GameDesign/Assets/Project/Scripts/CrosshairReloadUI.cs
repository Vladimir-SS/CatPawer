using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairReloadUI : GunEventHandler
{
    private Image img;

    override protected void Start()
    {
        base.Start();
        img = GetComponent<Image>();
    }

    IEnumerator FillImg(float duration)
    {
        Debug.Log("COOOL");
        float time = 0.0f;
        while (time < duration)
        {
            img.fillAmount = time / duration;
            Debug.Log(img.fillAmount);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    override public void OnGunReloadEvent(object sender, GunReloadEventArgs e)
    {
        StartCoroutine(FillImg(e.ReloadTime));
    }
}
