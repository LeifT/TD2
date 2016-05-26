using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public abstract class ISpell : ScriptableObject {
    public Sprite Icon;
    public string Name;
    public string Description;

    public abstract void Cast();
    //public abstract void OnPointerClick(PointerEventData eventData);
}
