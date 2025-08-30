using System;
using System.Reflection;

namespace agilum.mvc.web.Configuration
{
    public class ConverterClasse
    {
        public T MapearTextoParaObjeto<T>(string[] nomesPropriedades, string[] valoresPropriedades) 
        {
            T instancia = Activator.CreateInstance<T>();

            //percorrer os nomes das propriedades
            for(int i = 0;i < nomesPropriedades.Length;i++) 
            {
                // Obtém a propriedade atual através do nome.
                string nomePropriedade = nomesPropriedades[i];

                PropertyInfo propertyInfo = instancia.GetType().GetProperty(nomePropriedade);
                
                // Verifica se a propriedade foi encontrada.
                if (propertyInfo != null) 
                {
                    // Obtém o tipo da propriedade.
                    Type propertyType = propertyInfo.PropertyType;

                    // Obtém o valor da propriedade.
                    string valor = valoresPropriedades[i];

                    // Converte o valor da propriedade para o tipo correto.
                    object valorConvertido = Convert.ChangeType(valor, propertyType);

                    // Guarda o valor convertido na propriedade.
                    propertyInfo.SetValue(instancia, valorConvertido);
                }
            }

            return instancia;
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            // Cria uma instância do tipo de destino.
            TDestination destination = Activator.CreateInstance<TDestination>();

            // Obtém todas as propriedades do tipo de destino.
            PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties();

            // Itera sobre cada propriedade do tipo de destino.
            foreach (PropertyInfo destinationProperty in destinationProperties)
            {
                // Tenta encontrar uma propriedade correspondente no tipo de origem.
                PropertyInfo sourceProperty = typeof(TSource).GetProperty(destinationProperty.Name);

                // Se uma propriedade correspondente for encontrada, copia o valor.
                if (sourceProperty != null && destinationProperty.CanWrite)
                {
                    object value = sourceProperty.GetValue(source);
                    destinationProperty.SetValue(destination, value);
                }
            }

            return destination;
        }

        public static TDATA Mapear<TDATA>(object oldObject) where TDATA : new()
        {
            // Create a new object of type TDATA
            TDATA newObject = new TDATA();
            try
            {
                // If the old object is null, just return the new object
                if (oldObject == null) return newObject;
                // Get the type of the new object and the type of the old object passed in
                Type newObjType = typeof(TDATA);
                Type oldObjType = oldObject.GetType();
                // Get a list of all the properties in the new object
                var propertyList = newObjType.GetProperties();
                // If the new object has properties
                if (propertyList.Length > 0)
                {
                    // Loop through each property in the new object
                    foreach (var newObjProp in propertyList)
                    {
                        // Get the corresponding property in the old object
                        var oldProp = oldObjType.GetProperty(newObjProp.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.ExactBinding);
                        // If there is a corresponding property in the old object and it can be read and the new object's property can be written to
                        if (oldProp != null && oldProp.CanRead && newObjProp.CanWrite)
                        {
                            // assign property type of both object to new variables
                            var oldPropertyType = oldProp.PropertyType;
                            var newPropertyType = newObjProp.PropertyType;
                            //check if property is nullable or not. if property is nullable then get it's original data type from generic argument
                            if (oldPropertyType.IsGenericType && oldPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) oldPropertyType = oldPropertyType.GetGenericArguments()[0];
                            if (newPropertyType.IsGenericType && newPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) newPropertyType = newPropertyType.GetGenericArguments()[0];
                            //check type of both property if match then set value
                            if (newPropertyType == oldPropertyType)
                            {
                                // Get the value of the property in the old object
                                var value = oldProp.GetValue(oldObject);
                                // Set the value of the property in the new object
                                newObjProp.SetValue(newObject, value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If there is an exception, log it
            }
            // Return the new object
            return newObject;
        }
    }
}
