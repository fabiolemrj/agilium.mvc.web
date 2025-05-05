using agilium.api.business.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.Mappings
{
    public class UsuarioFotoMongoMapping
    {
        public void RegisterMapp()
        {
            BsonClassMap.RegisterClassMap<UsuarioFoto>(cm =>
            {
                cm.AutoMap();
                cm.MapIdField(c => c.Id);
                cm.MapMember(c => c.Descricao);
                cm.MapMember(c => c.DataCadastro);
                cm.MapMember(c => c.IdUsuarioAspNet);
                cm.MapMember(c => c.Imagem);
                cm.MapMember(c => c.Ativo);
                cm.MapMember(c => c.NomeArquivo);
                cm.MapMember(c => c.NomeUsuario);
                cm.MapMember(c => c.IdUsuario);
            });
        }
    }
}
