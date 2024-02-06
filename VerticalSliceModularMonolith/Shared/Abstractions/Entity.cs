namespace VerticalSliceModularMonolith.Shared.Abstractions;

public class Entity<TId> : BaseEntity, IEquatable<Entity<TId>>
{
    protected Entity()
    {
    }

    public virtual TId Codigo { get; set; }

    public virtual bool Equals(Entity<TId> other)
    {
        if (other == null)
        {
            return false;
        }
        return Codigo.Equals(other.Codigo);
    }

    public override bool Equals(object obj)
    {
        var compareTo = obj as Entity<TId>;

        if (ReferenceEquals(this, compareTo))
        {
            return true;
        }

        if (compareTo is null)
        {
            return false;
        }

        return Codigo.Equals(compareTo.Codigo);
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (GetType().GetHashCode() * 907) + Codigo.GetHashCode();
        }
    }

    public override string ToString()
    {
        return GetType().Name + " [Codigo=" + Codigo + "]";
    }
}
