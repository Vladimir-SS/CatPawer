using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairReloadUI : GunEventHandler
{
    private Image img;
    private float time = 0.0f;
    private float duration = -1;

    override protected void Start()
    {
        base.Start();
        img = GetComponent<Image>();
    }

    IEnumerator FillImg()
    {
        while (time < duration)
        {
            img.fillAmount = time / duration;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        img.fillAmount = 1;
    }

    private void OnEnable()
    {
        if(duration > 0)
            StartCoroutine(FillImg());
    }

    override public void OnGunReloadEvent(object sender, GunReloadEventArgs e)
    {
        time = 0;
        duration = e.ReloadTime;
        StartCoroutine(FillImg());
    }
}
