using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.DBConnection
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> User => Set<User>();
        public DbSet<Note> Note => Set<Note>();
        public DbSet<Collaborator> Collaborator => Set<Collaborator>();
        public DbSet<Label> Label => Set<Label>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaborator>()
            .HasKey(nc => new { nc.NoteId, nc.CollaboratorId });

            modelBuilder.Entity<Collaborator>()
            .HasOne(nc => nc.Note)
            .WithMany(n => n.Collaborators)
            .HasForeignKey(nc => nc.NoteId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Collaborator>()
           .HasOne(nc => nc.CollaboratorUser)
           .WithMany(u => u.CollaboratedNotes)
           .HasForeignKey(nc => nc.CollaboratorId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
            .HasMany(n => n.Labels)
            .WithMany(l => l.Notes)
            .UsingEntity(j => j.ToTable("NoteLabels"));

            modelBuilder.Entity<Label>()
            .HasIndex(l => l.Name)
            .IsUnique();

        }
    }
}
