
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite open;
    [SerializeField] private SpriteRenderer spriteRenderer;
        
    public void Unlock()
    {
        mesh.SetActive(false);
        spriteRenderer.sprite = open;
    }

    public void Lock()
    {
        mesh.SetActive(true);
        spriteRenderer.sprite = closed;
    }
}
