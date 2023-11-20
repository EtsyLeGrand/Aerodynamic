using System;

public class DebugCommand
{
    public string CommandID { get; }
    public Action Callback { get; }

    public DebugCommand(string commandID, Action callback)
    {
        CommandID = commandID;
        Callback = callback;
    }
}
