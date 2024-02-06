using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Shared.Models;

public class UsuarioModel : Entity<string>
{
    // STRINGS
    public string Nome { get; set; }

    //DATETIMES
    public DateTime DataCriacao { get; set; }

    // FKS
    public string? CriadoPorCodigo { get; set; }

    // RELACOES
    public UsuarioModel? CriadoPor { get; set; }

    public UsuarioModel()
    {
        Codigo = Guid.NewGuid().ToString();
        DataCriacao = DateTime.UtcNow;
    }
}

public class UsuarioModelConfig : IEntityTypeConfiguration<UsuarioModel>
{
    public void Configure(EntityTypeBuilder<UsuarioModel> builder)
    {
        builder.ToTable("usuario");

        builder.HasKey(x => x.Codigo);

        builder
            .Property(x => x.Codigo)
            .HasColumnName("usua_cd_usuario");

        // STRINGS
        builder
            .Property(x => x.Nome)
            .HasColumnName("usua_tx_nome");

        //DATETIMES
        builder
            .Property(x => x.DataCriacao)
            .HasColumnName("usua_dt_criacao");

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
