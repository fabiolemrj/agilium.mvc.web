using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class LogErro: Entity
    {
        public int id_logerro { get; set; }
        public string usuario { get; set; }
        public string erro { get; set; }
        public string tipo { get; set; }
        public string controle { get; set; }
        public string maquina { get; set; }
        public DateTime Data_erro { get; set; }
        public string Hora_erro { get; set; }
        public string SQL_erro { get; set; }
        public LogErro()
        {            
        }

        public LogErro(string usuario, string erro, string tipo, string controle, string maquina, DateTime data_erro, string hora_erro, string sQL_erro)
        {
            this.usuario = usuario;
            this.erro = erro;
            this.tipo = tipo;
            this.controle = controle;
            this.maquina = maquina;
            Data_erro = data_erro;
            Hora_erro = hora_erro;
            SQL_erro = sQL_erro;
        }
    }
}
