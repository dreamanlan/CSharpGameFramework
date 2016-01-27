using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AlphaChange : MonoBehaviour 
{
	public Text gTextRed;
    public Text gTextGreen;
    public Text gTextYellow;
    public Text gTextWhite;
	public float gStartTime;
	public float gFinishTime;
	
	private float mLifeTime;
	//=======================================================
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		mLifeTime += Time.deltaTime;
		if(mLifeTime >= gFinishTime)
		{
			Destroy(gameObject);
			return;
		}
		
		if(mLifeTime >= gStartTime)
		{
			float subValue = 1/(gFinishTime - gStartTime) * Time.deltaTime;

            if (gTextRed != null)
			{
                if (gTextRed.color.a > 0f)
				{
                    Color newColor = gTextRed.color;
					newColor.a -= subValue;
                    gTextRed.color = newColor;
				}
			}
            if (gTextGreen != null) {
                if (gTextGreen.color.a > 0f) {
                    Color newColor = gTextGreen.color;
                    newColor.a -= subValue;
                    gTextGreen.color = newColor;
                }
            }
            if (gTextYellow != null) {
                if (gTextYellow.color.a > 0f) {
                    Color newColor = gTextYellow.color;
                    newColor.a -= subValue;
                    gTextYellow.color = newColor;
                }
            }
            if (gTextWhite != null) {
                if (gTextWhite.color.a > 0f) {
                    Color newColor = gTextWhite.color;
                    newColor.a -= subValue;
                    gTextWhite.color = newColor;
                }
            }
		}		
	}
}
