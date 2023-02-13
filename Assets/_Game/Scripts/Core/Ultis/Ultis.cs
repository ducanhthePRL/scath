using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Ultis
{
    private static System.Diagnostics.Stopwatch stopwatch = null;
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static long GetCurrentTimeStamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public static string sha256_hash(long timestamp)
    {
        //string value = $"{BuildCode.buildCode}{timestamp}{Application.genuine}";
        StringBuilder Sb = new StringBuilder();

        //using (SHA256 hash = SHA256Managed.Create())
        //{
        //    Encoding enc = Encoding.UTF8;
        //    byte[] result = hash.ComputeHash(enc.GetBytes(value));

        //    foreach (byte b in result)
        //        Sb.Append(b.ToString("x2"));
        //}

        return Sb.ToString();
    }
    public static void SetActiveCursor(bool active)
    {
        if (active)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public static void StopWatchStart()
    {
        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
    }

    public static void StopWatchStop()
    {
        if (stopwatch == null) return;
        stopwatch.Stop();
        Debug.LogError(stopwatch.ElapsedMilliseconds);
        stopwatch = null;
    }

    public static float GetScreenRate()
    {
        float width = Screen.width;
        float height = Screen.height;
        float rate = width / height;
        return rate;
    }

    public static void ShowFade(bool is_show, Action action)
    {
        PopupFade popupFade = PanelManager.Show<PopupFade>();
        if (is_show)
            popupFade.Show(action);
        else
            popupFade.Hide(action);
    }

    private void SetActive(GameObject ob, bool active)
    {
        if (ob != null)
            ob.SetActive(active);
    }

    private void SetEventBt(ButtonCustom bt, UnityAction action)
    {
        if (bt != null)
            bt.onClick = action;
    }

    private void SetText(TextMeshProUGUI text, string content)
    {
        if (text != null)
            text.text = content;
    }

    private void SetImage(Image img, string path)
    {
        if (img != null)
            img.sprite = TexturesManager.Instance.GetSprites(path); ;
    }
}
