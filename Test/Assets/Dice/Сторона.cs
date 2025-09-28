using UnityEngine;

public class Side : MonoBehaviour
{
    [SerializeField] private SNum Number;
    public bool IsBottom { get; private set; }

    public int GetNumber() => (int)Number;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<End>(out _))
        {
            IsBottom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<End>(out _))
        {
            IsBottom = false;
        }
    }
}

