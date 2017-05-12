using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameFramework;
using System.Collections.Generic;
using System;

public class OverHeadBar : MonoBehaviour
{
	public float SLOW_HEALTH_STEP;		// 慢血条递减速度
	public float SLOW_HEALTH_CD;		// 慢血条延时
	
    public Image health;				// 主血条
	public Image health2;				// 主血条2
	public Image healthSlow;			// 慢血条
	public Image mp;
	
    public GameObject panelBloodNums;

    public Image bkg;
    public Text content;

    private Canvas canvas;
    private float slowTime;
    private TextGenerator generator;
    private Vector2 bkgSize;
    private float paopaoShowTime = 0;
    private readonly float paopaoDurationTime = 4;//s
 
    //==============================================================================
    public void SetHealth(float hpRate) 
    { 
		health.fillAmount = hpRate;
		health2.fillAmount = hpRate;
		if(slowTime <= 0f)
		{
			slowTime = SLOW_HEALTH_CD;
		}		
	}
    public float GetHealth()
    {
        return health.fillAmount;
    }	
	public void SetMp(float fmp) 
	{
		mp.fillAmount = fmp;
	}
    public void ShowPaoPao(string strContent)
    {
        content.text = strContent;
        paopaoShowTime = Time.time;
        bkg.gameObject.SetActive(true);
    }
    public void HidePaoPao()
    {
        bkg.gameObject.SetActive(false);
    }
    public void Start() 
    {
        generator = new TextGenerator();
        health.fillAmount = 1f;
		healthSlow.fillAmount = 1f;
		slowTime = 0f;
        canvas = this.GetComponentInChildren<Canvas>();
        RectTransform bkgRect = bkg.gameObject.GetComponent<RectTransform>();
        bkgSize = bkgRect.sizeDelta;
	}	
    public void SetHealthColor(bool isGreen) 
    {
    	health.gameObject.SetActive(isGreen);
    	health2.gameObject.SetActive(!isGreen);        
    }
    public void ShowFloatNum(int num,Color color,float duration)
    {
    	if(num != 0)
    	{
			GameObject obj = (GameObject)ResourceSystem.Instance.NewObject("UI/FlyBloodNum");
            AlphaChange txt = obj.GetComponent<AlphaChange>();
			txt.gTextGreen.text = num.ToString();
            txt.gTextGreen.gameObject.SetActive(color == Color.green);
            txt.gTextRed.text = num.ToString();
            txt.gTextRed.gameObject.SetActive(color == Color.red);
            txt.gTextWhite.text = num.ToString();
            txt.gTextWhite.gameObject.SetActive(color == Color.white);

			RectTransform rect = (RectTransform)obj.transform;
			rect.SetParent(panelBloodNums.transform, false);

            //随机位置
            RectTransform rectTxt = txt.gameObject.transform as RectTransform;
            Vector3 pos = rectTxt.anchoredPosition;
            pos.x = UnityEngine.Random.Range(-20.0f, 20.0f);
            rectTxt.anchoredPosition = pos;

			// 动画
			Animation anim = obj.GetComponent<Animation> ();
			if (anim != null) 
			{
				anim.Play();
			}
    	}
        
    }
    internal void Reset()
    {
        if (null != panelBloodNums) {
            panelBloodNums.transform.DetachChildren();
        }
    }
    internal void Update() 
    {
        if (Camera.main != null) 
        {
            this.transform.LookAt(this.transform.position - Camera.main.transform.TransformDirection(Vector3.back));
            float dist = (Camera.main.transform.position - this.transform.position).magnitude;
            float degree = Camera.main.fieldOfView * 0.4f;
            float size = Mathf.Tan(degree * Mathf.Deg2Rad / 2) * 2 * dist;
            RectTransform canvasRect = (canvas.transform as RectTransform);
            float defaultSize = canvasRect.rect.size.y * canvasRect.localScale.y;
            this.transform.localScale = new Vector3(1,1,0) * size / defaultSize;
        }
        
        // 慢血条
		slowTime -= Time.deltaTime;
		if(slowTime <= 0f)
		{
			slowTime = 0;
			if(healthSlow.fillAmount > health.fillAmount)
			{
				if((healthSlow.fillAmount - health.fillAmount) > SLOW_HEALTH_STEP)
				{
					healthSlow.fillAmount -= SLOW_HEALTH_STEP;
				}
				else
				{
					healthSlow.fillAmount = health.fillAmount;
				}
			}
			else if(healthSlow.fillAmount < health.fillAmount)
			{
				healthSlow.fillAmount = health.fillAmount;
			}
		}        
    }
    internal void LateUpdate()
    {
        float curTime = Time.time;
        if (curTime - paopaoShowTime > paopaoDurationTime && paopaoShowTime > 0) {
            HidePaoPao();
            paopaoShowTime = 0;
        }

        RectTransform bkgRect = bkg.gameObject.GetComponent<RectTransform>();
        RectTransform textBox = content.GetComponent<RectTransform>();
        TextGenerationSettings settings = content.GetGenerationSettings(bkgSize);
        float fHeight = generator.GetPreferredHeight(content.text, settings);//content.preferredHeight + 50;
        float fWidth = generator.GetPreferredWidth(content.text, settings);
        if (fWidth > 400)
            fWidth = 400;
        bkgRect.sizeDelta = new Vector2(fWidth + 50.0f, fHeight + 50.0f);
        textBox.sizeDelta = new Vector2(fWidth, fHeight);
    }	
}
