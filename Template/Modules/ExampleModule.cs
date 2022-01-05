using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Template.Common;
using static Template.Common.APIstuff;

namespace Template.Modules
{
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {
        private readonly ILogger<ExampleModule> _logger;
        private readonly Servers _servers;
        private readonly TimerService _service;

        public ExampleModule(ILogger<ExampleModule> logger, Servers servers, TimerService service)
        {
            _logger = logger;
            _servers = servers;
            _service = service;
        }

        [Command("TestThis")]
        public async Task TestThis(string Test = null)
        {
            string accessKey = "lAZIXxW9aVBLQq1CgCsapMTG2AqEywT3X7pjEyVGr0k";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://api.unsplash.com/photos/random?client_id={accessKey}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Root UnSplash = JsonConvert.DeserializeObject<Root>(responseBody);

            await Context.Channel.CommitmentAchievement($"Good Job !  {Context.User} !", $"You have now officially reached the first milestone in Never Productive ! We hope you keep going ! Now FEAST your eyes on this gorgeous digital image by {UnSplash.user.name}  !");
        }

        [Command("Commit")]
        public async Task Commit (string Commitment = null)
        {
            await Context.Message.DeleteAsync();
            await _servers.NumberOfCommitments(Context.User.Id, 1);
            var NumOfCommitments = await _servers.GetNumberOfCommitments(Context.User.Id);
            var message = await Context.Channel.SendSuccessAsync("Success !", $"Good job ! Your commitment has been saved");
            if (NumOfCommitments == 10)
            {
                await ReplyAsync($"Good job . You have now officially reached the first milestone in your productivity and we hereby give you the title of Productivity Knight !");
            }
            //await message.DeleteAsync();

        }
        [Command("FindCommitments")]
        public async Task FindCommitments (string Com = null)
        {
            await Context.Message.DeleteAsync();
            var NumOfCommitments = await _servers.GetNumberOfCommitments(Context.User.Id);
            await ReplyAsync($"The number of commitments is {NumOfCommitments}!");
        }
        
        [Command("meme")]
        [Alias("reddit")]
        public async Task Meme(string subreddit = null)
        {
            await Context.Message.DeleteAsync();
            var client = new HttpClient();
            var result = await client.GetStringAsync($"https://reddit.com/r/{subreddit ?? "memes"}/random.json?limit=1");
            if (!result.StartsWith("["))
            {
                await Context.Channel.SendMessageAsync("This subreddit doesnt exist !");
                return;
            }
            JArray arr = JArray.Parse(result);
            JObject post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString());

            var builder = new EmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithColor(new Color(92, 0, 184))
                .WithTitle(post["title"].ToString())
                .WithUrl("https://reddit.com" + post["permalink"].ToString())
                .WithFooter($"🗨 {post["num_comments"]} ⤴️ {post["ups"]}");
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
        [Command("StartMessageNow")]
        public async Task StartMessageNow()
        {
            await Context.Message.DeleteAsync();
            _service.StartNow();
            var message = await ReplyAsync("Starting....");
            await Task.Delay(2000);
            await message.DeleteAsync();
            
        }
        [Command("stoptimer")]
        public async Task StopCmd()

        {
            await Context.Message.DeleteAsync();
            _service.Stop();
            await ReplyAsync("Timer stopped.");
        }

        [Command("starttimer")]
        public async Task RestartCmd()
        {
            await Context.Message.DeleteAsync();
            _service.Restart();
            await ReplyAsync("Timer (re)started.");
        }

        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int amount)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);

            var message = await Context.Channel.SendMessageAsync($"{messages.Count()} messages deleted successfully!");
            await Task.Delay(2500);
            await message.DeleteAsync();
        }
        [Command("info")]
        public async Task Info(SocketGuildUser user = null)
        {
            user ??= (SocketGuildUser)Context.User;
            var builder = new EmbedBuilder()
                        .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                        .WithDescription($"In this message you can see some information about {user}")
                        .WithColor(new Color(92, 0, 184))
                        .AddField("User ID,", user.Id, true)
                        .AddField("Discriminator", user.Discriminator, true)
                        .AddField("Created at", user.CreatedAt.ToString("dd/MM/yyyy"), true)
                        .AddField("Joined at", user.JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                        .AddField("Roles", string.Join(" ", user.Roles.Select(x => x.Mention)))
                        .WithCurrentTimestamp();
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);

        }
        [Command("server")]
        public async Task Server()
        {
            var builder = new EmbedBuilder()
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithDescription("In this message you can find info about the server.")
                .WithTitle($"{Context.Guild.Name} Infromation")
                .WithColor(new Color(92, 0, 184))
                .AddField("Created at", Context.Guild.CreatedAt.ToString("dd/MM/yyyy"))
                .AddField("Membercount", (Context.Guild as SocketGuild).MemberCount + " members", true)
                .AddField("Online users", (Context.Guild as SocketGuild).Users.Where(x => x.Status != UserStatus.Offline).Count() + " members", true);
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }

        [Command("prefix")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Prefix(string prefix = null)
        {
            if(prefix == null)
            {
                var guildPrefix = await _servers.GetGuildPrefix(Context.Guild.Id) ?? "!";
                await ReplyAsync($"The current prefix of this servers is `{guildPrefix}`.");
                return;
            }
            if(prefix.Length > 8)
            {
                await ReplyAsync("The length of the new prefix is too long !");
                return;
            }

            await _servers.ModifyGuildPrefix(Context.Guild.Id, prefix);
            await ReplyAsync($"The prefix has been adjusted to `{prefix}`.");
        }

    }
}