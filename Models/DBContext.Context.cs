//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Facebook.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DemoSMS_OnlienEntities1 : DbContext
    {
        public DemoSMS_OnlienEntities1()
            : base("name=DemoSMS_OnlienEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ChatBoard> ChatBoards { get; set; }
        public virtual DbSet<Emoji> Emojis { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
