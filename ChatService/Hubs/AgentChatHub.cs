using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AgentChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            var avatar = Context.User?.Claims?.FirstOrDefault(c=>c.Type=="avatar")?.Value;
            await Clients.All.SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message);
        }
    }
}