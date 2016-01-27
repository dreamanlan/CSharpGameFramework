using UnityEngine;
using System.Collections;

public class PrintCurve : MonoBehaviour
{
  public AnimationCurve m_Curve;
  // Use this for initialization
  void Start()
  {
    try {
      if (m_Curve == null) {
        return;
      }
      string result = "";
      foreach (Keyframe key in m_Curve.keys) {
        result = result + "keyframe(" + key.time + ", " + key.value + ", " + key.inTangent + ", " + key.outTangent + ");\n";
      }
      Debug.Log(result);
    } catch (System.Exception ex) {
      GameFramework.LogSystem.Error("[Error]:Exception:{0}\n{1}", ex.Message, ex.StackTrace);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
