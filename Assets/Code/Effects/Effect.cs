using System;

namespace Pasta
{
    public abstract class Effect : IEquatable<Effect>
    {
        public readonly Guid Id;
        private bool _applied = false;

        public Effect()
        {
            Id = new Guid();
        }

        public virtual void Apply()
        {
            if (_applied) return;
            _applied = true;
        }

        public virtual void Unapply()
        {
            _applied = false;
        }

        public sealed override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is not Effect) return false;
            return Equals((Effect)obj);
        }

        public bool Equals(Effect other)
        {
            return Id.Equals(other.Id);
        }

        public sealed override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
