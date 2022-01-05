using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Template.Common.APIstuff;

namespace Template.Common
{
    public static class Extensions
    {
        
        public static async Task<IMessage> SendSuccessAsync(this ISocketMessageChannel channel, string title, string description)
        {
            var embed = new EmbedBuilder()
                .WithColor(new Color(147, 4, 224))
                .WithDescription(description)
                .WithAuthor(author =>
                {
                    author
                    .WithIconUrl("https://i.pinimg.com/originals/11/5f/0a/115f0ac90dfc685ff3564a27cb9e11d1.png")
                    .WithName(title);
                })
                .Build();
                
            var message = await channel.SendMessageAsync(embed: embed);
            return message;
        }
       
        public static async Task<IMessage> CommitmentAchievement(this ISocketMessageChannel channel, string title, string description)
        {
            string accessKey = "lAZIXxW9aVBLQq1CgCsapMTG2AqEywT3X7pjEyVGr0k"; //< ----put your access key in here
           HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://api.unsplash.com/photos/random?client_id={accessKey}"); // Gets the information from a random image request, using your accesskey
            response.EnsureSuccessStatusCode(); // to make sure it went correctly
            string responseBody = await response.Content.ReadAsStringAsync(); // This puts the response correctly in a string
            Root UnSplash = JsonConvert.DeserializeObject<Root>(responseBody);
            



            var embed = new EmbedBuilder()
                .WithColor(new Color(147, 4, 224))
                .WithDescription(description)
                .WithImageUrl(UnSplash.urls.full)
                .WithAuthor(author =>
                {
                    author
                    .WithIconUrl("https://i.pinimg.com/originals/11/5f/0a/115f0ac90dfc685ff3564a27cb9e11d1.png")
                    .WithName(title);
                    
                })
                .Build();

            var message = await channel.SendMessageAsync(embed: embed);
            return message;
        }
        
    }
}
