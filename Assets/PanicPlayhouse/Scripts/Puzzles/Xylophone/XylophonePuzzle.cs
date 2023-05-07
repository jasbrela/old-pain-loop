using System.Collections.Generic;
using NaughtyAttributes;
using PanicPlayhouse.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace PanicPlayhouse.Scripts.Puzzles.Xylophone
{
    public class XylophonePuzzle : MonoBehaviour
    {
        [SerializeField] private float insanityPenalty;
        [SerializeField] private FloatVariable insanity;
        [SerializeField] private List<XylophoneButton> order;
        [ReadOnly] [SerializeField] private int currentButton;

        private void Start()
        {
            var buttons = FindObjectsOfType<XylophoneButton>();
            
            if (order.Count == 0)
            {
                gameObject.SetActive(false);
                Debug.Log(name + "has been deactivated.");
                return;
            }
            
            foreach (XylophoneButton button in buttons)
            {
                button.Puzzle = this;
            }
        }

        public void OnPressButton(XylophoneButton button)
        {
            if (order[currentButton] != button)
            {
                currentButton++;
                // positive feedback
            }
            else
            {
                currentButton = 0;
                insanity.Increase(insanityPenalty);
                // negative feedback
            }
        }
    }
}
