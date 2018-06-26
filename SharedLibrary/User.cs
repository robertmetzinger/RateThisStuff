using System.Runtime.Serialization;

namespace SharedLibrary
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int Id;

        [DataMember]
        public string Username;

        [DataMember]
        public string Firstname;

        [DataMember]
        public string Lastname;

        [DataMember]
        public string Password;

        [DataMember]
        public bool IsAdmin;

        [DataMember]
        public int Version;

        protected bool Equals(User other)
        {
            return Id == other.Id && string.Equals(Username, other.Username) && string.Equals(Firstname, other.Firstname) && string.Equals(Lastname, other.Lastname) && string.Equals(Password, other.Password) && IsAdmin == other.IsAdmin;

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Firstname != null ? Firstname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Lastname != null ? Lastname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Password != null ? Password.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsAdmin.GetHashCode();
                return hashCode;
            }
        }
    }
}
