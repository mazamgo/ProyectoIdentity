using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace ProyectoIdentity.Servicios
{
	public class MailJetEmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;
		public OpcionesMailJet _opcionesMailJet;

		public MailJetEmailSender(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			//Este codigo es para acceder a la configuracion del appsetting.json y poner su contenido en la clase
			//para luego reemplazar el codigo de abajo.
			_opcionesMailJet = _configuration.GetSection("MailJet").Get<OpcionesMailJet>();

			//MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("****************************1234"), Environment.GetEnvironmentVariable("****************************abcd"))
			MailjetClient client = new MailjetClient(_opcionesMailJet.ApiKey, _opcionesMailJet.SecretKey)
			{
				Version = ApiVersion.V3_1,
			};
			MailjetRequest request = new MailjetRequest
			{
				Resource = Send.Resource,
			}
			 .Property(Send.Messages, new JArray {
	 new JObject {
	  {
	   "From",
	   new JObject {
		{"Email", "mauricio.zambrana.g@gmail.com"},
		{"Name", "Go Producciones..."}
	   }
	  }, {
	   "To",
	   new JArray {
		new JObject {
		 {
		  "Email",
		   email //"mauricio.zambrana.g@gmail.com"
		 }, {
		  "Name",
		  "Mauricio"
		 }
		}
	   }
	  }, {
	   "Subject",
	   subject //"Greetings from Mailjet."
	  }, 
	 //{
	 //  "TextPart",
	 //  "My first Mailjet email"
	 // }, 
		{
	   "HTMLPart",
	   htmlMessage //"<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
	  }
		 //,{
	  // "CustomID",
	  // "AppGettingStartedTest"	  
	  //}
	 }
			 });

			await client.PostAsync(request);
			//MailjetResponse response = await client.PostAsync(request);
			//if (response.IsSuccessStatusCode)
			//{
			//}
		}
	}
}
