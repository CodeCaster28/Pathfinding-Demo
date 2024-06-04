namespace Gameplay
{
    /// <summary>
    /// Enum path info used to determine why shortest path wasn't created or if it was found.
    /// </summary>
    /// 
    public enum PathInfo
    {
        ValidPath,
        NoStartNoGoal,
        NoStart,
        NoGoal,
        NoValidPath
    }
}
