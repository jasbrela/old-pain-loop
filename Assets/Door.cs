using NaughtyAttributes;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool startOpen;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite open;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (startOpen) Unlock();
    }

    [Button]
    public void Unlock()
    {
        mesh.SetActive(false);
        spriteRenderer.sprite = open;
    }
    
    [Button]
    public void Lock()
    {
        mesh.SetActive(true);
        spriteRenderer.sprite = closed;
    }
}
