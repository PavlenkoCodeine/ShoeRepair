//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoeRepair.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Appointments = new HashSet<Appointments>();
            this.Appointments1 = new HashSet<Appointments>();
            this.Chats = new HashSet<Chats>();
            this.Chats1 = new HashSet<Chats>();
            this.MasterLeaves = new HashSet<MasterLeaves>();
            this.Messages = new HashSet<Messages>();
        }
    
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public int ProfileID { get; set; }
    
        public virtual ICollection<Appointments> Appointments { get; set; }
        public virtual ICollection<Appointments> Appointments1 { get; set; }
        public virtual ICollection<Chats> Chats { get; set; }
        public virtual ICollection<Chats> Chats1 { get; set; }
        public virtual ICollection<MasterLeaves> MasterLeaves { get; set; }
        public virtual ICollection<Messages> Messages { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
