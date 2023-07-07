using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private bool startOpen;
    [SerializeField] private GameObject mesh;
    [SerializeField] private Sprite closed;
    [SerializeField] private Sprite open;
    [SerializeField] private SpriteRenderer spriteRenderer;

    bool locked = true;

    private void Start()
    {
        if (startOpen) Unlock();
    }

    [Button]
    public void Unlock()
    {
        if (!locked)
            return;

        locked = false;
        mesh.SetActive(false);
        spriteRenderer.sprite = open;
    }

    [Button]
    public void Lock()
    {
        if (locked)
            return;

        locked = true;
        mesh.SetActive(true);
        spriteRenderer.sprite = closed;
    }
}
