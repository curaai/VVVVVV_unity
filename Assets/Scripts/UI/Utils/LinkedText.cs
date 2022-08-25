using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.UI.Utils
{
    // ! This class only used in TextBoxFrame w/ Content Size Filter
    [RequireComponent(typeof(Text))]
    public class LinkedText : MonoBehaviour
    {
        [SerializeField] Text original;
        private string lastText;

        void Update()
        {
            original.text = GetComponent<Text>().text;
        }
    }
}