﻿using ENTIDADE;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace BLL
{
    public class ESTATISTICA_BLL
    {
        private IHttpClientFactory _clientFactory;
        private HttpClient client;
        private string key = "AIzaSyAEEZGVU8BvneXl1MRkisFKkf3KTYoWmtg";

        public ESTATISTICA_BLL(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            client = _clientFactory.CreateClient();
        }

        public Dictionary<string, int> CarregarEstatistica()
        {
            ANIMAL_BLL animal_bll = new ANIMAL_BLL();

            List<ANIMAL> animais = animal_bll.CarregarAnimais();
            List<Dictionary<string, int>> racas;

            Dictionary<string, int> cidades = new Dictionary<string, int>
            {
                { "Salto",  0 },
                { "Itu", 0 },
                { "Sorocaba", 0 }
            };

            Dictionary<string, int> animaisInfo = new Dictionary<string, int>
            {
                { "Total",  0 },
                { "Macho", 0 },
                { "Femea", 0 },
                { "Indefinido", 0 },
                { "Cachorro", 0 },
                { "Gato", 0 }
            };

            Dictionary<string, int> RacaCachorroInfo = new Dictionary<string, int>
            {
                { "Labrador",  0 },
                { "Rottweiler", 0 },
                { "GoldenRetriever", 0 },
                { "ViraLataCachorro", 0 },
                { "Poodle", 0 },
                { "PastorAlemao", 0 },
                { "SpitzAlemao", 0 },
                { "Buldogue", 0 },
                { "ShihTzu",0 },
                { "Maltes", 0 },
                { "IndefinidoCachorro", 0 }
            };

            Dictionary<string, int> RacaGatoInfo = new Dictionary<string, int>
            {
                { "Persa",  0 },
                { "Siames", 0 },
                { "ViralataGato", 0 },
                { "Siberiano", 0 },
                { "Sphynx", 0 },
                { "Angora", 0 },
                { "Abissinio", 0 },
                { "IndefinidoGato", 0 }
            };

            cidades = calcularEstastiscaCidade(animais, cidades);
            animaisInfo = calcularEstatisticaAnimal(animais, animaisInfo);
            racas = calcularEstatisticasRaca(animais, RacaCachorroInfo, RacaGatoInfo);

            RacaCachorroInfo = racas[0];
            RacaGatoInfo = racas[1];

            return cidades.Union(animaisInfo).ToDictionary(k => k.Key, v => v.Value).Union(RacaCachorroInfo).ToDictionary(k => k.Key, v => v.Value)
               .Union(RacaGatoInfo).ToDictionary(k => k.Key, v => v.Value);

        }

        public Dictionary<string, int> calcularEstastiscaCidade(List<ANIMAL> animais, Dictionary<string, int> cidades)
        {

            List<string> nomeCidades = new List<string>(cidades.Keys);

            foreach (var animal in animais)
            {
                string latitude = animal.LATITUDE.ToString();

                string longitude = animal.LONGITUDE.ToString();

                string latlng = latitude + "," + longitude;

                using (HttpResponseMessage res = client.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?latlng={latlng}&key={key}").Result)
                {
                    if (res.IsSuccessStatusCode)
                    {
                        //string json = res.Content.ReadAsStringAsync().Result;
                        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(res.Content.ReadAsStringAsync().Result);

                        JArray address = jsonObject["results"][0]["address_components"] as JArray;

                        foreach (var ad in address)
                        {
                            foreach (var nome in nomeCidades)
                            {
                                int qtd = ad["long_name"].ToString() == nome ? cidades[nome] + 1 : 0;

                                if (qtd > 0)
                                {
                                    cidades[nome] = qtd;
                                }
                            }
                        }
                    }
                    else
                    {
                        Exception ex = JsonConvert.DeserializeObject<Exception>(res.Content.ReadAsStringAsync().Result);
                        throw ex;
                    }
                }
            }

            return cidades;
        }

        public Dictionary<string, int> calcularEstatisticaAnimal(List<ANIMAL> animais, Dictionary<string, int> animaisInfo)
        {
            animaisInfo["Total"] = animais.Count;

            foreach(var animal in animais)
            {
                switch(animal.SEXO)
                {
                    case "F":
                        animaisInfo["Femea"] = animaisInfo["Femea"] + 1;
                        break;
                    case "M":
                        animaisInfo["Macho"] = animaisInfo["Macho"] + 1;
                        break;
                    case "I":
                        animaisInfo["Indefinido"] = animaisInfo["Indefinido"] + 1;
                        break;
                    default:
                        break;
                }

                switch(animal.TIPO)
                {
                    case "Cachorro":
                        animaisInfo["Cachorro"] = animaisInfo["Cachorro"] + 1;
                        break;
                    case "Gato":
                        animaisInfo["Gato"] = animaisInfo["Gato"] + 1;
                        break;
                    default:
                        break;
                }
            }

            return animaisInfo;
        }

        public List<Dictionary<string, int>> calcularEstatisticasRaca(List<ANIMAL> animais, Dictionary<string, int> RacaCachorroInfo,
            Dictionary<string, int> RacaGatoInfo)
        {

            List<Dictionary<string, int>> racas = new List<Dictionary<string, int>>();

            List<string> racasGatos = new List<string>(RacaGatoInfo.Keys);

            List<string> racasCachorros = new List<string>(RacaCachorroInfo.Keys);

            foreach (var animal in animais)
            {
                switch(animal.TIPO)
                {
                    case "Cachorro":
                        foreach (var raca in racasCachorros)
                        {
                            if (String.Compare(raca, animal.RACA, CultureInfo.GetCultureInfo("pt-BR"), CompareOptions.IgnoreNonSpace) == 0 ||
                                String.Compare(raca, animal.RACA, CultureInfo.GetCultureInfo("pt-BR"), CompareOptions.IgnoreSymbols) == 0)
                                {
                                    RacaCachorroInfo[raca] = RacaCachorroInfo[raca] + 1;
                                    break;
                                }
                                    

                            if(animal.RACA == "Indefinido")
                            {
                                RacaCachorroInfo["IndefinidoCachorro"] = RacaCachorroInfo["IndefinidoCachorro"] + 1;
                                break;
                            }

                            if(animal.RACA == "Pastor Alemão")
                            {
                                RacaCachorroInfo["PastorAlemao"] = RacaCachorroInfo["PastorAlemao"] + 1;
                                break;
                            }

                            if (animal.RACA == "Spitz Alemão")
                            {
                                RacaCachorroInfo["Spitz Alemão"] = RacaCachorroInfo["Spitz Alemão"] + 1;
                                break;
                            }

                            if (animal.RACA == "Vira lata")
                            {
                                RacaCachorroInfo["ViraLataCachorro"] = RacaCachorroInfo["ViraLataCachorro"] + 1;
                                break;
                            }
                        }
                        break;

                    case "Gato":
                        foreach (var raca in racasGatos)
                        {
                            if (String.Compare(raca, animal.RACA, CultureInfo.GetCultureInfo("pt-BR"), CompareOptions.IgnoreNonSpace) == 0 ||
                                String.Compare(raca, animal.RACA, CultureInfo.GetCultureInfo("pt-BR"), CompareOptions.IgnoreSymbols) == 0)
                                    RacaGatoInfo[raca] = RacaGatoInfo[raca] + 1;

                            if (animal.RACA == "Indefinido")
                            {
                                RacaGatoInfo["IndefinidoGato"] = RacaGatoInfo["IndefinidoGato"] + 1;
                                break;
                            }

                            else if (animal.RACA == "Vira lata")
                            {
                                RacaGatoInfo["ViralataGato"] = RacaGatoInfo["ViralataGato"] + 1;
                                break;
                            }
                        }
                        break;
                }
            }

            racas.Add(RacaCachorroInfo);
            racas.Add(RacaGatoInfo);

            return racas;
        }

        public string RetornaJson()
        {
            return "{\n   \"plus_code\" : {\n      \"compound_code\" : \"QPQJ+J3 Jardim da Cidade, Salto - SP, Brazil\",\n      \"global_code\" : \"588JQPQJ+J3\"\n   },\n   \"results\" : [\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"328\",\n               \"short_name\" : \"328\",\n               \"types\" : [ \"street_number\" ]\n            },\n            {\n               \"long_name\" : \"Avenida das Bandeiras\",\n               \"short_name\" : \"Av. das Bandeiras\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Nossa Senhora do Monte Serrat\",\n               \"short_name\" : \"Nossa Sra. do Monte Serrat\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"13323-310\",\n               \"short_name\" : \"13323-310\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Av. das Bandeiras, 328 - Nossa Sra. do Monte Serrat, Salto - SP, 13323-310, Brazil\",\n         \"geometry\" : {\n            \"location\" : {\n               \"lat\" : -23.2110083,\n               \"lng\" : -47.2698589\n            },\n            \"location_type\" : \"ROOFTOP\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2096593197085,\n                  \"lng\" : -47.2685099197085\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2123572802915,\n                  \"lng\" : -47.2712078802915\n               }\n            }\n         },\n         \"place_id\" : \"ChIJA1MzuUFOz5QRdB4wyxinQbs\",\n         \"plus_code\" : {\n            \"compound_code\" : \"QPQJ+H3 Nossa Senhora do Monte Serrat, Salto - SP, Brazil\",\n            \"global_code\" : \"588JQPQJ+H3\"\n         },\n         \"types\" : [ \"establishment\", \"point_of_interest\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"328\",\n               \"short_name\" : \"328\",\n               \"types\" : [ \"street_number\" ]\n            },\n            {\n               \"long_name\" : \"Avenida das Bandeiras\",\n               \"short_name\" : \"Av. das Bandeiras\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Nossa Senhora do Monte Serrat\",\n               \"short_name\" : \"Nossa Sra. do Monte Serrat\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"13323-310\",\n               \"short_name\" : \"13323-310\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Av. das Bandeiras, 328 - Nossa Sra. do Monte Serrat, Salto - SP, 13323-310, Brazil\",\n         \"geometry\" : {\n            \"location\" : {\n               \"lat\" : -23.2110083,\n               \"lng\" : -47.2698589\n            },\n            \"location_type\" : \"ROOFTOP\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2096593197085,\n                  \"lng\" : -47.2685099197085\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2123572802915,\n                  \"lng\" : -47.2712078802915\n               }\n            }\n         },\n         \"place_id\" : \"ChIJ9el0t0FOz5QRaStRwb88-ug\",\n         \"plus_code\" : {\n            \"compound_code\" : \"QPQJ+H3 Nossa Senhora do Monte Serrat, Salto - SP, Brazil\",\n            \"global_code\" : \"588JQPQJ+H3\"\n         },\n         \"types\" : [ \"street_address\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"328\",\n               \"short_name\" : \"328\",\n               \"types\" : [ \"street_number\" ]\n            },\n            {\n               \"long_name\" : \"Avenida das Bandeiras\",\n               \"short_name\" : \"Av. das Bandeiras\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Nossa Senhora do Monte Serrat\",\n               \"short_name\" : \"Nossa Sra. do Monte Serrat\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"13323-310\",\n               \"short_name\" : \"13323-310\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Av. das Bandeiras, 328 - Nossa Sra. do Monte Serrat, Salto - SP, 13323-310, Brazil\",\n         \"geometry\" : {\n            \"location\" : {\n               \"lat\" : -23.2108781,\n               \"lng\" : -47.2697996\n            },\n            \"location_type\" : \"RANGE_INTERPOLATED\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2095291197085,\n                  \"lng\" : -47.2684506197085\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2122270802915,\n                  \"lng\" : -47.2711485802915\n               }\n            }\n         },\n         \"place_id\" : \"ElJBdi4gZGFzIEJhbmRlaXJhcywgMzI4IC0gTm9zc2EgU3JhLiBkbyBNb250ZSBTZXJyYXQsIFNhbHRvIC0gU1AsIDEzMzIzLTMxMCwgQnJhemlsIhsSGQoUChIJuQCgvUFOz5QR_AZzZAx9mr0QyAI\",\n         \"types\" : [ \"street_address\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"290-336\",\n               \"short_name\" : \"290-336\",\n               \"types\" : [ \"street_number\" ]\n            },\n            {\n               \"long_name\" : \"Avenida das Bandeiras\",\n               \"short_name\" : \"Av. das Bandeiras\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Nossa Senhora do Monte Serrat\",\n               \"short_name\" : \"Nossa Sra. do Monte Serrat\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"13323-310\",\n               \"short_name\" : \"13323-310\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Av. das Bandeiras, 290-336 - Nossa Sra. do Monte Serrat, Salto - SP, 13323-310, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2108343,\n                  \"lng\" : -47.2694226\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2110204,\n                  \"lng\" : -47.2699136\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.2109285,\n               \"lng\" : -47.2696686\n            },\n            \"location_type\" : \"GEOMETRIC_CENTER\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2095783697085,\n                  \"lng\" : -47.26831911970849\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2122763302915,\n                  \"lng\" : -47.27101708029149\n               }\n            }\n         },\n         \"place_id\" : \"ChIJuQCgvUFOz5QR_AZzZAx9mr0\",\n         \"types\" : [ \"route\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"13323-311\",\n               \"short_name\" : \"13323-311\",\n               \"types\" : [ \"postal_code\" ]\n            },\n            {\n               \"long_name\" : \"Nossa Senhora do Monte Serrat\",\n               \"short_name\" : \"Nossa Sra. do Monte Serrat\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Nossa Sra. do Monte Serrat, Salto - SP, 13323-311, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2050013,\n                  \"lng\" : -47.26673419999999\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2122257,\n                  \"lng\" : -47.2703983\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.2096396,\n               \"lng\" : -47.26819130000001\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2050013,\n                  \"lng\" : -47.26673419999999\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2122257,\n                  \"lng\" : -47.2703983\n               }\n            }\n         },\n         \"place_id\" : \"ChIJA2_SwUFOz5QRSL3u_W_SXcw\",\n         \"types\" : [ \"postal_code\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Jardim da Cidade\",\n               \"short_name\" : \"Jardim da Cidade\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"13323\",\n               \"short_name\" : \"13323\",\n               \"types\" : [ \"postal_code\", \"postal_code_prefix\" ]\n            }\n         ],\n         \"formatted_address\" : \"Jardim da Cidade, Salto - SP, 13323, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2101989,\n                  \"lng\" : -47.2679494\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2156038,\n                  \"lng\" : -47.2748579\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.2130299,\n               \"lng\" : -47.2710091\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2101989,\n                  \"lng\" : -47.2679494\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2156038,\n                  \"lng\" : -47.2748579\n               }\n            }\n         },\n         \"place_id\" : \"ChIJo3GS80BOz5QRMS984RPPTOY\",\n         \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"13323\",\n               \"short_name\" : \"13323\",\n               \"types\" : [ \"postal_code\", \"postal_code_prefix\" ]\n            },\n            {\n               \"long_name\" : \"Itu\",\n               \"short_name\" : \"Itu\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"State of São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Itu - State of São Paulo, 13323, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2033602,\n                  \"lng\" : -47.2527724\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2357808,\n                  \"lng\" : -47.28596109999999\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.2170245,\n               \"lng\" : -47.2703983\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.2033602,\n                  \"lng\" : -47.2527724\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2357808,\n                  \"lng\" : -47.28596109999999\n               }\n            }\n         },\n         \"place_id\" : \"ChIJRwuwmUhOz5QREESo4vHFv4I\",\n         \"types\" : [ \"postal_code\", \"postal_code_prefix\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Ajudante\",\n               \"short_name\" : \"Ajudante\",\n               \"types\" : [ \"administrative_area_level_4\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"State of São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Salto - Ajudante, Salto - State of São Paulo, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.1107345,\n                  \"lng\" : -47.2534717\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2341637,\n                  \"lng\" : -47.36047079999999\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.2000805,\n               \"lng\" : -47.293538\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.1107345,\n                  \"lng\" : -47.2534717\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2341637,\n                  \"lng\" : -47.36047079999999\n               }\n            }\n         },\n         \"place_id\" : \"ChIJ7frs2GVSz5QRp5SW3cXtdOc\",\n         \"types\" : [ \"locality\", \"political\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Salto\",\n               \"short_name\" : \"Salto\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"State of São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Salto - State of São Paulo, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.1108996,\n                  \"lng\" : -47.1956016\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2368173,\n                  \"lng\" : -47.3895154\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.2000684,\n               \"lng\" : -47.2935486\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -23.1108996,\n                  \"lng\" : -47.1956016\n               },\n               \"southwest\" : {\n                  \"lat\" : -23.2368173,\n                  \"lng\" : -47.3895154\n               }\n            }\n         },\n         \"place_id\" : \"ChIJx3Bu4wVOz5QRscS995xOcAo\",\n         \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"State of São Paulo\",\n               \"short_name\" : \"SP\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"State of São Paulo, Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : -19.7796583,\n                  \"lng\" : -44.1613651\n               },\n               \"southwest\" : {\n                  \"lat\" : -25.3126231,\n                  \"lng\" : -53.1101046\n               }\n            },\n            \"location\" : {\n               \"lat\" : -23.5431786,\n               \"lng\" : -46.6291845\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : -19.7796583,\n                  \"lng\" : -44.1613651\n               },\n               \"southwest\" : {\n                  \"lat\" : -25.3126231,\n                  \"lng\" : -53.1101046\n               }\n            }\n         },\n         \"place_id\" : \"ChIJrVgvRn1ZzpQRF3x74eJBUh4\",\n         \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n      },\n      {\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Brazil\",\n               \"short_name\" : \"BR\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Brazil\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 5.2717863,\n                  \"lng\" : -28.650543\n               },\n               \"southwest\" : {\n                  \"lat\" : -34.0891,\n                  \"lng\" : -73.9828169\n               }\n            },\n            \"location\" : {\n               \"lat\" : -14.235004,\n               \"lng\" : -51.92528\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 5.2717863,\n                  \"lng\" : -28.650543\n               },\n               \"southwest\" : {\n                  \"lat\" : -34.0891,\n                  \"lng\" : -73.9828169\n               }\n            }\n         },\n         \"place_id\" : \"ChIJzyjM68dZnAARYz4p8gYVWik\",\n         \"types\" : [ \"country\", \"political\" ]\n      }\n   ],\n   \"status\" : \"OK\"\n}\n";
        }
    }
}
