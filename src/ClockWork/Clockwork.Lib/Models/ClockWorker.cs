using System;

namespace Clockwork.Lib.Models
{
    public class ClockWorker : ICloneable
    {
        public ClockWorker(string familyName, string givenName, DateTime dateOfBirth)
        {
            FamilyName = familyName;
            GivenName = givenName;
            DateOfBirth = dateOfBirth;
        }
        public string FamilyName { get; }
        public string GivenName { get; }
        public DateTime DateOfBirth { get; }

        public override bool Equals(object obj)
        {
            return obj != null && (ReferenceEquals(this, obj) || obj is ClockWorker clockWorker && Equals(clockWorker));
        }

        protected bool Equals(ClockWorker other)
        {
            return 
                StringComparer.CurrentCultureIgnoreCase.Equals(FamilyName, other.FamilyName) &&
                StringComparer.CurrentCultureIgnoreCase.Equals(GivenName, other.GivenName) && 
                DateOfBirth.Date.Equals(other.DateOfBirth.Date);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = FamilyName?.ToLower().GetHashCode();
                hashCode = (hashCode * 397) ^ GivenName?.ToLower().GetHashCode();
                hashCode = (hashCode * 397) ^ DateOfBirth.Date.GetHashCode();
                return hashCode ?? 0;
            }
        }

        public object Clone() => new ClockWorker(FamilyName, GivenName, DateOfBirth.Date);
    }
}
