using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Shared.Models;

public class LivroModel : Entity<string>
{
    // STRINGS
    public string Titulo { get; set; }

    // DATETIMES
    public DateTime DataCriacao { get; set; }

    // FKS
    public string CriadoPorCodigo { get; set; }

    // RELACOES
    public UsuarioModel CriadoPor { get; set; }

    public LivroModel()
    {
        Codigo = Guid.NewGuid().ToString();
        DataCriacao = DateTime.UtcNow;
    }
}

public class LivroModelConfig : IEntityTypeConfiguration<LivroModel>
{
    public void Configure(EntityTypeBuilder<LivroModel> builder)
    {
        builder.ToTable("livro");

        builder.HasKey(x => x.Codigo);

        builder
            .Property(x => x.Codigo)
            .HasColumnName("livr_cd_livro");

        // STRINGS
        builder
            .Property(x => x.Titulo)
            .HasColumnName("livr_tx_titulo");

        // DATETIMES
        builder
            .Property(x => x.DataCriacao)
            .HasColumnName("livr_dt_criacao");

        // FKS
        builder
            .Property(x => x.CriadoPorCodigo)
            .HasColumnName("usua_cd_criado_por");

        // RELACOES
        builder
            .HasOne(x => x.CriadoPor)
            .WithMany()
            .HasForeignKey(x => x.CriadoPorCodigo);
    }
}
