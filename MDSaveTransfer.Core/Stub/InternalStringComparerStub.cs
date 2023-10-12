namespace MDSaveTransfer;

public class InternalStringComparerStub : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        return string.Equals(x, y);
    }

    public int GetHashCode(string obj)
    {
        return obj.GetHashCode();
    }
}