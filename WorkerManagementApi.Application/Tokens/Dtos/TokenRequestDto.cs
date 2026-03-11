using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerManagementApi.Application.Tokens.Dtos
{
    public class TokenRequestDto
    {
        [Required]
        [JsonProperty("Username")]
        public string Username { get; set; }
        [Required]
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
