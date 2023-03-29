[System.Serializable]
public class Message
{
    public Message(string newRole, string newContent)
    {
        this.role = newRole;
        this.content = newContent;
    }

    public string role;
    public string content;
}