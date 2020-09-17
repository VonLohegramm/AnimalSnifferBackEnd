using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENTIDADE
{
    public class USUARIO
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "nome")]
        public string NOME { get; set; }
        [JsonProperty(PropertyName = "senha")]
        public string SENHA { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string EMAIL { get; set; }
    }
}
