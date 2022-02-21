using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http
{
    public class CommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public CommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }
        public async Task SendPlatformToCommand(PlatfromReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
                );

            var responce = await _httpClient.PostAsync($"{_config["CommandService"]}", httpContent);

            if (responce.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post to Command Service Was Ok");
            }
            else
            {
                Console.WriteLine("--> Sync Post to Command Service Was not Ok");
            }
        }
    }
}
