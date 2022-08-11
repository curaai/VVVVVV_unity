using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.UI
{
    public class HorizontalPanelSelector : MonoBehaviour
    {
        public void ChangePanel(int idx)
        {
            foreach (Transform childPanel in transform)
                childPanel.gameObject.SetActive(false);

            transform.GetChild(idx).gameObject.SetActive(true);
        }
    }
}