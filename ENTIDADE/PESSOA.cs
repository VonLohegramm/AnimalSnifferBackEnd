using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ENTIDADE
{
    public class PESSOA
    {
        [JsonProperty(PropertyName = "cpf")]
        public string CPF { get; set; }
        [JsonProperty(PropertyName = "nome")]
        public string NOME { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
    }
}
