using Microsoft.AspNetCore.Identity;
using ProjectManager.Enums;

namespace ProjectManager.Models
{
    public class VALORES_BOLSA
    {
        public const double BOLSA_GRADUANDO = 1200.00;
        public const double BOLSA_MESTRANDO = 2200.00;
        public const double BOLSA_DOUTORANDO = 3600.00;
        public const double BOLSA_PROFESSOR = 4000.00;
        public const double BOLSA_PESQUISADOR = 3.800;
        public const double BOLSA_DESENVOLVEDOR = 3.600;
        public const double BOLSA_TESTADOR = 2.800;
        public const double BOLSA_ANALISTA = 2.100;
        public const double BOLSA_TECNICO = 1.100;
    }

    /// <summary>
    /// Classe que herda de IdentityUser para poder criar usuário
    /// com propriedades personalizadas
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string? Nome { get; set; }
        public string? Sobrenome { get; set; }
        public string? Descricao { get; set; }
        public EnumTipoCargo TipoCargo { get; set; }
        public List<int>? Projetos { get; set; }
        public List<int>? Atividades { get; set; }
        public double ValorBolsa { get; set; }
    }
}
