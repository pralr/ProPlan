using System.ComponentModel;

namespace ProjectManager.Enums
{
    public enum EnumTipoCargo
    {
        [Description("Graduando")]
        GRADUANDO = 1,

        [Description("Mestrando")]
        MESTRANDO = 2,

        [Description("Doutorando")]
        DOUTORANDO = 3,

        [Description("Professor")]
        PROFESSOR = 4,

        [Description("Pesquisador")]
        PESQUISADOR = 5,

        [Description("Desenvolvedor")]
        DESENVOLVEDOR = 6,

        [Description("Testador")]
        TESTADOR = 7,

        [Description("Analista")]
        ANALISTA = 8,

        [Description("Técnico")]
        TECNICO = 9
    }
}
