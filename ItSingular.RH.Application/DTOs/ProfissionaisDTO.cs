using System;
using System.Collections.Generic;
using System.Text;

namespace ItSingular.RH.Application.DTOs
{
    public class ProfissionaisDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public string Senioridade { get; set; }
        public decimal? PretensaoSalarial { get; set; }
        public string PrincipaisTecnologias { get; set; }
        public string Tags { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int CriadoPor { get; set; }
        public int AtualizadoPor { get; set; }

    }
}
