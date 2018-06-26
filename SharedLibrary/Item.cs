using System.Runtime.Serialization;

namespace SharedLibrary
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Name;

        [DataMember]
        public int Version;

        [DataMember]
        public Category Category;

        [DataMember]
        public double AverageRating;

        [DataMember]
        public int RatingsCount;

        protected bool Equals(Item other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && Equals(Category, other.Category);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Category != null ? Category.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
