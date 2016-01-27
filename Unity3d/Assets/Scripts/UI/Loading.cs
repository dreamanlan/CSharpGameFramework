using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image imgBackground;
    public Slider sldProgress;
    public void SetBackground(string path)
    {
        imgBackground.sprite = Resources.Load<Sprite>(path);
    }
    public void SetProgress(float value)
    {
        sldProgress.value = value;
    }
}
