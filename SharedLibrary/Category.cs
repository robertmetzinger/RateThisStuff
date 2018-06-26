using System.Runtime.Serialization;

namespace SharedLibrary
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Name;

        [DataMember]
        public int Version;

        protected bool Equals(Category other)
        {
            return Id == other.Id && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Category) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}
