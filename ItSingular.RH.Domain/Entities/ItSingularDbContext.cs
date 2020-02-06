using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ItSingular.RH.Domain.Entities
{
    public partial class ItSingularDbContext : DbContext
    {
        public ItSingularDbContext()
        {
        }

        public ItSingularDbContext(DbContextOptions<ItSingularDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Profissionais> Profissionais { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS; Database=ItSingular;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profissionais>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AtualizadoPor).HasColumnName("atualizado_por");

                entity.Property(e => e.Cargo)
                    .HasColumnName("cargo")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf).HasColumnName("cpf");

                entity.Property(e => e.CriadoPor).HasColumnName("criado_por");

                entity.Property(e => e.DataAlteracao)
                    .HasColumnName("data_alteracao")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("data_cadastro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasColumnName("nome")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PretensaoSalarial)
                    .HasColumnName("pretensao_salarial")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PrincipaisTecnologias)
                    .HasColumnName("principais_tecnologias")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Senioridade)
                    .HasColumnName("senioridade")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tags)
                    .HasColumnName("tags")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasColumnName("telefone")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
