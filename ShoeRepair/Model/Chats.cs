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
    
    public partial class Chats
    {
        public Chats()
        {
            this.Messages = new HashSet<Messages>();
        }
    
        public int Id { get; set; }
        public int Id_User { get; set; }
        public int Id_Admin { get; set; }
        public System.DateTime CreationDate { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual ICollection<Messages> Messages { get; set; }
    }
}
