using UnityEngine;
using UnityEngine.InputSystem;

namespace PanicPlayhouse.Scripts.Chunk
{
    public class ContextItem : Interactable
    {
        [SerializeField] private PlayerInput input;
        [SerializeField] private GameObject toShow;
        
        public override void OnInteract()
        {
            base.OnInteract();
            toShow.SetActive(!toShow.activeSelf);

            if (toShow.activeSelf)
            {
                input.actions["Movement"].Disable();
            }
            else
            {
                input.actions["Movement"].Enable();
            }
        }

        public void OnPlayerGoInsane()
        {
            toShow.gameObject.SetActive(false);
        }
    }
}