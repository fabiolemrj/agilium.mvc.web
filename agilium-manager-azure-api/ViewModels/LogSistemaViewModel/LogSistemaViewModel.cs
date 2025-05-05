using System;

namespace agilium.api.manager.ViewModels.LogSistemaViewModel
{
    public class LogSistemaViewModel
    {
        public int id { get; set; }
        public string NomeUsuario { get; set; }
        public string Descricao { get; set; }
        public string TelaOrigem { get; set; }
        public string AcaoOriem { get; set; }
        public string Maquina { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public string SQLScript { get; set; }
        public string so { get; set; }
    }
}
