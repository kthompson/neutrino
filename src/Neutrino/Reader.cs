namespace Neutrino;

public abstract class Reader<T>
{
    public abstract T Head { get; }
    public abstract Reader<T> Tail { get; }
    public abstract bool IsEmpty { get; }
    
    public virtual Reader<T> Skip(int n)
    {
        var r = this;
        var cnt = n;
        while (cnt > 0)
        {
            r = r.Tail;
            cnt--;
        }

        return r;
    }
}