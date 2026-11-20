using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ScriptableFramework;
using System.Collections.Generic;
using System;

public class OverHeadBar : MonoBehaviour
{
	public float SLOW_HEALTH_STEP;      // Slow health bar decreasing speed
    public float SLOW_HEALTH_CD;        // Slow health bar delay

    public Image health;                // Main health bar
    public Image health2;               // Main health bar2
    public Image healthSlow;            // Slow health bar
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
        if (null == content || null == bkg) return;
        content.text = strContent;
        paopaoShowTime = Time.time;
        bkg.gameObject.SetActive(true);
    }
    public void HidePaoPao()
    {
        if (null == bkg) return;
        bkg.gameObject.SetActive(false);
    }
    // Editor domain reloads clear non-serialized private fields but Unity does
    // NOT rerun Start() afterwards. Reconstruct them on demand so a bar keeps
    // working (and stops throwing NullReferenceException every frame) after a
    // script recompile while playing.
    private void EnsureInitialized()
    {
        if (null != generator) return;
        generator = new TextGenerator();
        canvas = this.GetComponentInChildren<Canvas>();
        if (null != bkg) {
            RectTransform r = bkg.gameObject.GetComponent<RectTransform>();
            if (null != r) bkgSize = r.sizeDelta;
        }
    }

    public void Start() 
    {
        EnsureInitialized();
        if (null != health) health.fillAmount = 1f;
		if (null != healthSlow) healthSlow.fillAmount = 1f;
		slowTime = 0f;
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

            //random location
            RectTransform rectTxt = txt.gameObject.transform as RectTransform;
            Vector3 pos = rectTxt.anchoredPosition;
            pos.x = UnityEngine.Random.Range(-20.0f, 20.0f);
            rectTxt.anchoredPosition = pos;

			//animation
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
        EnsureInitialized();
        if (Camera.main != null && null != canvas)
        {
            this.transform.LookAt(this.transform.position - Camera.main.transform.TransformDirection(Vector3.back));
            float dist = (Camera.main.transform.position - this.transform.position).magnitude;
            float degree = Camera.main.fieldOfView * 0.4f;
            float size = Mathf.Tan(degree * Mathf.Deg2Rad / 2) * 2 * dist;
            RectTransform canvasRect = (canvas.transform as RectTransform);
            if (null != canvasRect) {
                float defaultSize = canvasRect.rect.size.y * canvasRect.localScale.y;
                if (defaultSize > 0.001f)
                    this.transform.localScale = new Vector3(1,1,0) * size / defaultSize;
            }
        }

        // Slow health bar
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
        EnsureInitialized();
        float curTime = Time.time;
        if (curTime - paopaoShowTime > paopaoDurationTime && paopaoShowTime > 0) {
            HidePaoPao();
            paopaoShowTime = 0;
        }

        if (null == bkg || null == content || null == generator) return;
        RectTransform bkgRect = bkg.gameObject.GetComponent<RectTransform>();
        RectTransform textBox = content.GetComponent<RectTransform>();
        if (null == bkgRect || null == textBox) return;
        TextGenerationSettings settings = content.GetGenerationSettings(bkgSize);
        float fHeight = generator.GetPreferredHeight(content.text, settings);//content.preferredHeight + 50;
        float fWidth = generator.GetPreferredWidth(content.text, settings);
        if (fWidth > 400)
            fWidth = 400;
        bkgRect.sizeDelta = new Vector2(fWidth + 50.0f, fHeight + 50.0f);
        textBox.sizeDelta = new Vector2(fWidth, fHeight);
    }	
}
