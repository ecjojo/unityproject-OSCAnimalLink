using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class joystickScript : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    private Image joystick_bg;
    private Image joystick;
    public float offset;

    public Vector2 InputDir { set; get; }

    public bool Havemove;

    private void Start()
    {
        joystick_bg = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
        InputDir = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        Vector2 pos = Vector2.zero;
        float joystick_bgSizeX = joystick_bg.rectTransform.sizeDelta.x;
        float joystick_bgSizeY = joystick_bg.rectTransform.sizeDelta.y;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(joystick_bg.rectTransform,eventData.position,eventData.pressEventCamera,out pos))
        {
            pos.x /= joystick_bgSizeX;
            pos.y /= joystick_bgSizeY;
            if (pos.x < -1)
            {
                GameObject.Find("GameController").GetComponent<GameController>().x_vec = -1;
            }
            else if(pos.x > 1)
            {
                GameObject.Find("GameController").GetComponent<GameController>().x_vec = 1;
            }
            else
            {
                GameObject.Find("GameController").GetComponent<GameController>().x_vec = pos.x;
            }
            if (pos.y < -1)
            {
                GameObject.Find("GameController").GetComponent<GameController>().z_vec = -1;
            }
            else if (pos.y > 1)
            {
                GameObject.Find("GameController").GetComponent<GameController>().z_vec = 1;
            }
            else
            {
                GameObject.Find("GameController").GetComponent<GameController>().z_vec = pos.y;
            }

            InputDir = new Vector2(pos.x, pos.y);
            InputDir = InputDir.magnitude > 1 ? InputDir.normalized : InputDir;

            joystick.rectTransform.anchoredPosition = new Vector2(InputDir.x * (joystick_bgSizeX/offset), InputDir.y * (joystick_bgSizeY/offset));
            //Debug.Log(pos.x);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerOSCScript.instance.GameStart)
        {
            OnDrag(eventData);
            Havemove = true;
        }
        /*OnDrag(eventData);
        Havemove = true;*/
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputDir = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
        Havemove = false;
        GameObject.Find("GameController").GetComponent<GameController>().x_vec = 0.0f;
        GameObject.Find("GameController").GetComponent<GameController>().z_vec = 0.0f;
    }
}
