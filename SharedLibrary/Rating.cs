using System.Runtime.Serialization;

namespace SharedLibrary
{
    [DataContract]
    public class Rating
    {
        [DataMember]
        public int Id;

        [DataMember]
        public int Score;

        [DataMember]
        public string Comment;

        [DataMember]
        public int Version;

        [DataMember]
        public User User;

        [DataMember]
        public Item Item;

        protected bool Equals(Rating other)
        {
            return Id == other.Id && Score == other.Score && string.Equals(Comment, other.Comment) && Equals(User, other.User) && Equals(Item, other.Item);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rating) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ Score;
                hashCode = (hashCode * 397) ^ (Comment != null ? Comment.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (User != null ? User.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Item != null ? Item.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
