using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform followTarget;
    private Image hpBar;
    private RectTransform recTransform;
    public float percent {
        get => hpBar.fillAmount;
        set => hpBar.fillAmount = value;
    }

    void Awake()
    {
        hpBar = transform.GetChild(0).GetComponent<Image>();
        recTransform = transform.GetComponent<RectTransform>();
        if (hpBar == null)
            Debug.LogError("HPBarManager: hpBar is null");
    }

    void OnEnable() {
        follow();
    }

    void Update()
    {
        follow();
    }
    
    void follow() {
        if (followTarget) {
            Vector2 pos2D = Camera.main.WorldToScreenPoint(followTarget.position);
            recTransform.position = pos2D;
    
            if (pos2D.x > Screen.width || pos2D.x < 0 || pos2D.y > Screen.height || pos2D.y < 0)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        } else {
        }
    }

}
