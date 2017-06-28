using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    public Color colorNormal, colorHighlighted, colorSelected;
    Stack stack;
    public Image image;
    public Text counter;
    bool highlighted;
    bool selected;
    [System.Serializable]
    public class setActiveEvent : UnityEvent<EquipmentSlot> { }
    [SerializeField]
    public setActiveEvent onSetActive;

    int id;
    int count;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Select();
        }
    }

    public void Select()
    {
        if (count > 0)
        {
            onSetActive.Invoke(this);
            GetComponent<Image>().color = colorSelected;
            selected = true;
        }
    }

    public void Deselect()
    {
        GetComponent<Image>().color = highlighted ? colorHighlighted : colorNormal;
        selected = false;
    }

    public void PointerEnter()
    {
        highlighted = true;
        GetComponent<Image>().color = selected ? colorSelected : colorHighlighted;
    }

    public void PointerExit()
    {
        highlighted = false;
        GetComponent<Image>().color = selected ? colorSelected : colorNormal;
    }

    private void OnDisable()
    {
        PointerExit();
    }

    public void SetId(int id)
    {
        this.id = id;
        Texture2D tex = ItemStore.getItem(id).texture;
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
    }

    public void SetCount(int count)
    {
        this.count = count;
        image.enabled = count > 0;
        counter.text = count > 1 ? count.ToString() : "";
    }

    public int GetId()
    {
        return id;
    }

    public int GetCount()
    {
        return count;
    }
}