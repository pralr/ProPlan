using System.ComponentModel;

namespace ProjectManager.Enums
{
	public enum EnumStatusProjeto
	{
		[Description("Em processo de criação")]
		EM_PROCESSO_CRIACAO = 1,

		[Description("Iniciado")]
		INICIADO = 2,

		[Description("Em andamento")]
		EM_ANDAMENTO = 3,

		[Description("Concluído")]
		CONDLUIDO = 4
	}
}
