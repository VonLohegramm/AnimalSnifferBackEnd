using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENTIDADE
{
    public class AVALIACAO
    {
        [JsonProperty(PropertyName = "idUsuario")]
        public int IDUSUARIO { get; set; }
        [JsonProperty(PropertyName = "idAnimal")]
        public int IDANIMAL { get; set; }
    }
}
