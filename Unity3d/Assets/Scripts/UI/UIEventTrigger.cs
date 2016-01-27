using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GameFramework;

public class UIEventTrigger : UnityEngine.EventSystems.EventTrigger 
{
	public delegate void VoidDelegate ();
    public delegate void DelegateWithEventData(PointerEventData eventData);
    public delegate void DelegateWithBaseEventData(BaseEventData eventData);
    public VoidDelegate onClick;
    public DelegateWithEventData onClickWithEvtent;
    public DelegateWithEventData onDown;
    public DelegateWithEventData onEnter;
    public DelegateWithEventData onExit;
    public DelegateWithEventData onUp;
    public DelegateWithEventData onBeginDrag;
    public DelegateWithEventData onDrag;
    public DelegateWithEventData onEndDrag;
    public DelegateWithBaseEventData onSelect;
    public DelegateWithBaseEventData onUpdateSelect;

    static public UIEventTrigger Get(GameObject go)
	{
        UIEventTrigger listener = go.GetComponent<UIEventTrigger>();
        if (listener == null) listener = go.AddComponent<UIEventTrigger>();
		return listener;
	}
    static public UIEventTrigger Get(UIBehaviour control) {
        UIEventTrigger listener = control.gameObject.GetComponent<UIEventTrigger>();
        if (listener == null) listener = control.gameObject.AddComponent<UIEventTrigger>();
        return listener;
    }
    static public UIEventTrigger Find(GameObject root, string name)
    {
        GameObject go = Utility.FindChildObject(root, name);
        if (go) {
            return Get(go);
        }
        return null;
    }
	public override void OnPointerClick(PointerEventData eventData)
	{
        if (onClick != null) onClick();
        if (onClickWithEvtent != null) onClickWithEvtent(eventData);
	}
	public override void OnPointerDown (PointerEventData eventData){
        if (onDown != null) onDown(eventData);
	}
	public override void OnPointerEnter (PointerEventData eventData){
        if (onEnter != null) onEnter(eventData);
	}
	public override void OnPointerExit (PointerEventData eventData){
        if (onExit != null) onExit(eventData);
	}
	public override void OnPointerUp (PointerEventData eventData){
        if (onUp != null) onUp(eventData);
	}
	public override void OnSelect (BaseEventData eventData){
        if (onSelect != null) onSelect(eventData);
	}
	public override void OnUpdateSelected (BaseEventData eventData){
        if (onUpdateSelect != null) onUpdateSelect(eventData);
	}
    public override void OnBeginDrag(PointerEventData eventData) {
        if (onBeginDrag != null) { onBeginDrag(eventData); }
    }
    public override void OnDrag(PointerEventData eventData) {
        if (onDrag != null) { onDrag(eventData); }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) { onEndDrag(eventData); }
    }
}