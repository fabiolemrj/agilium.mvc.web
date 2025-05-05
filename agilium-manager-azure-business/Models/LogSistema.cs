using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class LogSistema: Entity
    {
        public int id_log { get; set; }
        public string usuario { get; set; }
        public string descr { get; set; }
        public string tela { get; set; }
        public string controle { get; set; }
        public string maquina { get; set; }
        public DateTime data_log { get; set; }
        public string hora_log { get; set; }
        public string SQL_log { get; set; }
        public string so { get; set; }
        public LogSistema()
        {
            
        }

        public LogSistema(int id_log, string usuario, string descr, string tela, string controle, string maquina, DateTime data_log, string hora_log, string sQL_log, string so)
        {
            this.id_log = id_log;
            this.usuario = usuario;
            this.descr = descr;
            this.tela = tela;
            this.controle = controle;
            this.maquina = maquina;
            this.data_log = data_log;
            this.hora_log = hora_log;
            SQL_log = sQL_log;
            this.so = so;
        }
    }
}
