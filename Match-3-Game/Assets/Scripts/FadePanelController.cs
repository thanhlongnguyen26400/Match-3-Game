using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{
    public Animator panelAnim;

    public Animator gameInfoAnim;


    private void Start()
    {
        if (panelAnim != null && gameInfoAnim != null)
        {
            panelAnim.SetBool("Out", false);
            gameInfoAnim.SetBool("Out", false);
        }
    }

    public void OK()
    {
        if(panelAnim != null && gameInfoAnim != null)
        {

            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
        }
        
    }
}
