using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HackAtHome.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace HackAtHome.SAL
{
    public class ServiceClient
    {
        // Dirección base de la Web API
        const string WebAPIBaseAddress = "https://ticapacitacion.com/hackathome/";
        // ID del diplomado
        const string EventID = "xamarin30";
        const string RequestUri = "api/evidence/Authenticate";

        /// <summary>
        /// Realiza la autenticación al servicio Web API
        /// </summary>
        /// <param name="studentEmail">Correo del usuario</param>
        /// <param name="studentPassword">Password del usuario</param>
        /// <returns>Objeto ResultInfo con los datos del usuario y un token de autenticación.</returns>
        public async Task<ResultInfo> AutenticateAsync(string studentEmail, string studentPassword)
        {
            var result = new ResultInfo();

            // El servicio requiere un objeto UserInfo con los datos del usuario y evento.
            var user = new UserInfo
            {
                Email = studentEmail,
                Password = studentPassword,
                EventID = EventID
            };

            // Utilizamos el objeto System.Net.Http.HttpClient para consumir el servicio REST
            // Debe instalarse el paquete NuGet System.Net.Http
            using (var client = new HttpClient())
            {
                // Establecemos la dirección base del servicio REST
                client.BaseAddress = new Uri(WebAPIBaseAddress);

                // Limpiamos encabezados de la petición.
                client.DefaultRequestHeaders.Accept.Clear();

                // Indicamos al servicio que envie los datos en formato JSON.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Serializamos a formato JSON el objeto a enviar.
                    // Debe instalarse el paquete NuGet Newtonsoft.Json.
                    var jsonUserInfo = JsonConvert.SerializeObject(user);

                    // Hacemos una petición POST al servicio enviando el objeto JSON
                    var response =
                            await client.PostAsync(RequestUri,
                            new StringContent(jsonUserInfo, Encoding.UTF8, "application/json"));

                    // Leemos el resultado devuelto.
                    var resultWebApi = await response.Content.ReadAsStringAsync();

                    // Deserializamos el resultado JSON obtenido
                    result = JsonConvert.DeserializeObject<ResultInfo>(resultWebApi);
                }
                catch (Exception)
                {
                    // Aquí podemos poner el código para manejo de excepciones.
                    result.Status = Status.Error;
                }
            }
            return result;
        }

        /// <summary>
        /// Obtiene la lista de evidencias.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario.</param>
        /// <returns>Una lista con las evidencias.</returns>
        public async Task<List<Evidence>> GetEvidencesAsync(string token)
        {
            List<Evidence> Evidences = null;

            // Dirección del servicio REST
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidences?token={token}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una petición GET
                    var Response =
                            await Client.GetAsync(URI);

                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos el valor devuelto.
                        var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                        Evidences = JsonConvert.DeserializeObject<List<Evidence>>(ResultWebAPI);
                    }
                }
                catch (Exception)
                {
                    // Aquí podemos poner el código para manejo de excepciones.
                }
            }
            return Evidences;
        }

        /// <summary>
        /// Obtiene el detalle de una evidencia.
        /// </summary>
        /// <param name="token">Token de autenticación del usuario</param>
        /// <param name="evidenceID">Identificador de la evidencia.</param>
        /// <returns>Información de la evidencia.</returns>
        public async Task<EvidenceDetail> GetEvidenceByIDAsync(string token, int evidenceID)
        {
            EvidenceDetail Result = null;

            // URI de la evidencia.
            string URI = $"{WebAPIBaseAddress}api/evidence/getevidencebyid?token={token}&&evidenceid={evidenceID}";

            using (var Client = new HttpClient())
            {
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    // Realizamos una petición GET
                    var Response =
                            await Client.GetAsync(URI);

                    var ResultWebAPI = await Response.Content.ReadAsStringAsync();
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        // Si el estatus de la respuesta HTTP fue exitosa, leemos
                        // el valor devuelto. 
                        Result = JsonConvert.DeserializeObject<EvidenceDetail>(ResultWebAPI);
                    }
                }
                catch (Exception)
                {
                    // Aquí podemos poner el código para manejo de excepciones.
                }
            }
            return Result;
        }

    }
}