namespace lab5v5;

public class BitCollection
{
    private bool[] _bits;

    public int Count => _bits.Length;

    public BitCollection(int size)
    {
        _bits = new bool[size];
    }

    public BitCollection(bool[] bits)
    {
        _bits = (bool[])bits.Clone();
    }

    public bool this[int index]
    {
        get => _bits[index];
        set => _bits[index] = value;
    }

    public void SetAll(bool value)
    {
        for (int i = 0; i < _bits.Length; i++)
        {
            _bits[i] = value;
        }
    }

    public static BitCollection operator &(BitCollection a, BitCollection b)
    {
        BitCollection result = new BitCollection(a.Count);
        for (int i = 0; i < a.Count; i++)
        {
            result[i] = a[i] & b[i];
        }
        return result;
    }

    public static BitCollection operator |(BitCollection a, BitCollection b)
    {
        BitCollection result = new BitCollection(a.Count);
        for (int i = 0; i < a.Count; i++)
        {
            result[i] = a[i] | b[i];
        }
        return result;
    }

    public static bool operator ==(BitCollection? a, BitCollection? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(BitCollection? a, BitCollection? b) => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj is not BitCollection other || Count != other.Count) return false;

        for (int i = 0; i < Count; i++)
        {
            if (_bits[i] != other._bits[i]) return false;
        }
        return true;
    }

    public override int GetHashCode() => _bits.GetHashCode();

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < _bits.Length; i++)
        {
            result += (_bits[i] ? "1" : "0") + (i < _bits.Length - 1 ? ", " : "");
        }
        return $"[{result}]";
    }
}