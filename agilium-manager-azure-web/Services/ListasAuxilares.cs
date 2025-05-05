using agilium.webapp.manager.mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Services
{
    public static class ListasAuxilares
    {
        public static List<string> ObterTeclaAtalho()
        {
            var teclasAtalho = new List<string>();

            teclasAtalho.Add("F2");
            teclasAtalho.Add("F3");
            teclasAtalho.Add("F4");
            teclasAtalho.Add("F7");
            teclasAtalho.Add("F8");
            teclasAtalho.Add("F9");
            teclasAtalho.Add("Crtl+1");
            teclasAtalho.Add("Crtl+2");
            teclasAtalho.Add("Crtl+3");
            teclasAtalho.Add("Crtl+4");
            teclasAtalho.Add("Crtl+5");
            teclasAtalho.Add("Crtl+6");
            teclasAtalho.Add("Crtl+7");
            teclasAtalho.Add("Crtl+8");
            teclasAtalho.Add("Crtl+9");
            teclasAtalho.Add("Crtl+0");
            
            return teclasAtalho;
        }

        public static List<Estado> ObterEstados()
        {
            List<Estado> estados = new List<Estado>();
            estados.Add(new Estado() { Sigla = "RJ", Nome = "Rio de Janeiro" });
            estados.Add(new Estado() { Sigla = "MG", Nome = "Minas Gerais" });
            estados.Add(new Estado() { Sigla = "SP", Nome = "São Paulo" });
            estados.Add(new Estado() { Sigla = "AC", Nome = "Acre" });
            estados.Add(new Estado() { Sigla = "AL", Nome = "Alagoas" });
            estados.Add(new Estado() { Sigla = "AP", Nome = "Amapá" });
            estados.Add(new Estado() { Sigla = "AM", Nome = "Amazonas" });
            estados.Add(new Estado() { Sigla = "BA", Nome = "Bahia" });
            estados.Add(new Estado() { Sigla = "CE", Nome = "Ceará" });
            estados.Add(new Estado() { Sigla = "DF", Nome = "Distrito Federal" });
            estados.Add(new Estado() { Sigla = "ES", Nome = "Espírito Santo" });
            estados.Add(new Estado() { Sigla = "GO", Nome = "Goiás" });
            estados.Add(new Estado() { Sigla = "MA", Nome = "Maranhão" });
            estados.Add(new Estado() { Sigla = "RS", Nome = "Rio Grande do Sul" });
            estados.Add(new Estado() { Sigla = "SC", Nome = "Santa Catarina" });
            estados.Add(new Estado() { Sigla = "PR", Nome = "Parana" });
            estados.Add(new Estado() { Sigla = "MT", Nome = "Mato Grosso" });
            estados.Add(new Estado() { Sigla = "MS", Nome = "Mato Grosso do Sul" });
            estados.Add(new Estado() { Sigla = "RR", Nome = "Roraima" });
            estados.Add(new Estado() { Sigla = "RD", Nome = "Rondonia" });
            estados.Add(new Estado() { Sigla = "TO", Nome = "Tocantis" });
            estados.Add(new Estado() { Sigla = "PA", Nome = "Pará" });
            estados.Add(new Estado() { Sigla = "RN", Nome = "Rio Grande do Norte" });
            estados.Add(new Estado() { Sigla = "RS", Nome = "Paraíba" });
            estados.Add(new Estado() { Sigla = "PI", Nome = "Piauí" });
            estados.Add(new Estado() { Sigla = "SE", Nome = "Sergipe" });

            return estados;
        }
    }
}
