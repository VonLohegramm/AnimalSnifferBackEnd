using Newtonsoft.Json;
using System.Collections.Generic;

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
        public decimal LATITUDE;
        [JsonProperty(PropertyName = "longitude")]
        public decimal LONGITUDE;
        [JsonProperty(PropertyName = "imagem")]
        public string IMAGEM;
        [JsonProperty(PropertyName = "ativo")]
        public bool ATIVO;
        [JsonProperty(PropertyName = "idUsuario")]
        public int IDUSUARIO;
        [JsonProperty(PropertyName = "avaliacoes")]
        public List<AVALIACAO> AVALIACOES;
    }
}
