using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ItSingular.RH.Application.DTOs;
using ItSingular.RH.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ItSingular.RH.Mvc.Controllers
{
    public class ProfissionaisController : Controller
    {
		readonly string Baseurl = "https://localhost:44356/";
		//readonly string requestUri = "api/Profissionais/ListaDeProfissionais";
		readonly string requestUri = "api/Profissionais/ListaDeProfissionais";
		readonly string loginUri = "api/Account/Login";

        public async Task<IActionResult> Index()
        {

			return View();

        }

		public async Task<IActionResult> ListaProfissionais([FromForm] string usuario, string senha)
		{
			using (var client = new HttpClient())
			{
				try
				{
					#region Autenticacao

					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json"));

					client.BaseAddress = new Uri(Baseurl);

					// Você vai pegar o UserName e a Password de TextBox na tela
					HttpResponseMessage respToken = client.PostAsync(loginUri,
						new StringContent(
							JsonConvert.SerializeObject(new
							{
								//UserName = "Administrador01",
								//Password = "Admin01@sinistro"
								UserName = usuario,
								Password = senha
							}), Encoding.UTF8, "application/json")).Result;

					string conteudo = respToken.Content.ReadAsStringAsync().Result;

					if (respToken.StatusCode == HttpStatusCode.OK)
					{
						Token token = JsonConvert.DeserializeObject<Token>(conteudo);
						if (token.Authenticated)
						{
							client.DefaultRequestHeaders.Authorization =
								new AuthenticationHeaderValue("Bearer", token.AccessToken);

						}
					}

					#endregion

					//Dictionary<string, string> parameters = new Dictionary<string, string>();
					//parameters.Add("TenantId", "1");
					//parameters.Add("TenantName", "LuftLogistica");

					//MultipartFormDataContent multiContent = new MultipartFormDataContent();
					//HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
					//multiContent.Add(DictionaryItems, "TenantData");


					var response = await client.GetAsync(requestUri);

					//return StatusCode((int)result.StatusCode); //201 Created the request has been fulfilled, resulting in the creation of a new resource.
					//Checking the response is successful or not which is sent using HttpClient  
					var ProfissionaisResponse = response.Content.ReadAsStringAsync().Result;

					if (!response.IsSuccessStatusCode)
					{
						return BadRequest(Json(ProfissionaisResponse).Value);
					}
					else
					{
						var liatProfissionaisDTO = JsonConvert.DeserializeObject<List<ProfissionaisDTO>>(ProfissionaisResponse.ToString());
						return View(liatProfissionaisDTO);
					}

				}
				catch (Exception ex)
				{
					//throw(ex);
					return NotFound(ex.Message);
				}
			}

		}

		public async Task<IActionResult> ProfissionalById()
		{

			using (var client = new HttpClient())
			{
				try
				{
					#region Autenticacao

					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(
						new MediaTypeWithQualityHeaderValue("application/json"));

					client.BaseAddress = new Uri(Baseurl);

					HttpResponseMessage respToken = client.PostAsync(loginUri,
						new StringContent(
							JsonConvert.SerializeObject(new
							{
								UserName = "Administrador01",
								Password = "Admin01@sinistro"
							}), Encoding.UTF8, "application/json")).Result;

					string conteudo = respToken.Content.ReadAsStringAsync().Result;

					if (respToken.StatusCode == HttpStatusCode.OK)
					{
						Token token = JsonConvert.DeserializeObject<Token>(conteudo);
						if (token.Authenticated)
						{
							client.DefaultRequestHeaders.Authorization =
								new AuthenticationHeaderValue("Bearer", token.AccessToken);

						}
					}

					#endregion

					//Dictionary<string, string> parameters = new Dictionary<string, string>();
					//parameters.Add("TenantId", "1");
					//parameters.Add("TenantName", "LuftLogistica");

					//MultipartFormDataContent multiContent = new MultipartFormDataContent();
					//HttpContent DictionaryItems = new FormUrlEncodedContent(parameters);
					//multiContent.Add(DictionaryItems, "TenantData");


					var response = await client.GetAsync("api/Profissionais/5");

					//return StatusCode((int)result.StatusCode); //201 Created the request has been fulfilled, resulting in the creation of a new resource.
					//Checking the response is successful or not which is sent using HttpClient  
					var ProfissionaisResponse = response.Content.ReadAsStringAsync().Result;

					//return Ok(ProfissionaisResponse);

					if (!response.IsSuccessStatusCode)
					{
						return BadRequest(Json(ProfissionaisResponse).Value);
					}
					else
					{
						return Ok(Json(ProfissionaisResponse).Value);
					}

				}
				catch (Exception ex)
				{
					//throw(ex);
					return NotFound(ex.Message);
				}
			}

		}
	}
}