using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENTIDADE
{
    public class ANIMAL
    {
        [JsonProperty(PropertyName = "id")]
        public int ID;
        [JsonProperty(PropertyName = "tipo")]
        public string TIPO;
        [JsonProperty(PropertyName = "raca")]
        public string RACA;
        [JsonProperty(PropertyName = "sexo")]
        public string SEXO;
        [JsonProperty(PropertyName = "descricao")]
        public string DESCRICAO;
        [JsonProperty(PropertyName = "latitude")]
        public string LATITUDE;
        [JsonProperty(PropertyName = "longitude")]
        public string LONGITUDE;
        [JsonProperty(PropertyName = "imagem")]
        public string IMAGEM;
        [JsonProperty(PropertyName = "ativo")]
        public bool ATIVO;
    }
}
