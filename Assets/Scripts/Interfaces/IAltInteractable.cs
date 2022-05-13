using Unity.VisualScripting;

public interface IAltInteractable
{
    void AltInteract();

    InteractInfo CanInteract();
}

public struct InteractInfo
{
    public bool canInteract;

    public string text;

    public int priority;

    public InteractInfo(bool canInteract, string text, int priority)
    {
        this.canInteract = canInteract;
        this.text = text;
        this.priority = priority;
    }

}
