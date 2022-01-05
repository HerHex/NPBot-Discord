using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Template.Common;
using static Template.Common.APIstuff;

namespace Template.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;
        private readonly Servers _servers;


        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config, Servers servers)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
            _servers = servers;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.ChannelCreated += OnChannelCreated;
            _client.JoinedGuild += OnJoinedGuild;
            _client.ReactionAdded += OnReactionAdded;
            _client.ReactionRemoved += OnReactionRemoved;
            



            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = msg.Value as SocketUserMessage;


            string accessKey = "lAZIXxW9aVBLQq1CgCsapMTG2AqEywT3X7pjEyVGr0k"; 
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://api.unsplash.com/photos/random?client_id={accessKey}"); 
            response.EnsureSuccessStatusCode(); 
            string responseBody = await response.Content.ReadAsStringAsync(); 
            Root UnSplash = JsonConvert.DeserializeObject<Root>(responseBody);


            if (reaction.User.Value.IsBot) return;
            var chan = _client.GetChannel(839915981018103831) as IMessageChannel;
            
            if (reaction.Emote.Name == "💕" || reaction.Emote.Name == "🔼")
            {
                

                await _servers.NumberOfCommitments(reaction.User.Value.Id, 1);
                var NumOfCommitments = await _servers.GetNumberOfCommitments(reaction.User.Value.Id);

                var specmessage = await channel.SendSuccessAsync("Success !", $"Good job ! Your commitment has been saved");
                await Task.Delay(10000);
                await specmessage.DeleteAsync();
                
                if (NumOfCommitments == 10)
                {
                    var MilestoneMessage = await channel.CommitmentAchievement($"Good Job !  {reaction.User} !", $"You have now officially reached the first milestone in Never Productive ! We hope you keep going ! Now FEAST your eyes on this gorgeous digital image by {UnSplash.user.name} !");
                }
            }
            else return;
            //await message.DeleteAsync();

        }
        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = msg.Value as SocketUserMessage;

            if (reaction.User.Value.IsBot) return;
            var chan = _client.GetChannel(839915981018103831) as IMessageChannel;

            if (reaction.Emote.Name == "💕" || reaction.Emote.Name == "🔼")
            {


                await _servers.RemoveCommitment(reaction.User.Value.Id, 1);
                var NumOfCommitments = await _servers.GetNumberOfCommitments(reaction.User.Value.Id);

            }
            else return;
        }

        private async Task OnJoinedGuild(SocketGuild arg)
        {
            await arg.DefaultChannel.SendMessageAsync("Thank you for using my discord bot !");
        }

        private async Task OnChannelCreated(SocketChannel arg)
        {
            if ((arg as ITextChannel) == null) return;
            var channel = arg as ITextChannel;

            await channel.SendMessageAsync("The event was called !");
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            var argPos = 0;
            var prefix = await _servers.GetGuildPrefix((message.Channel as SocketGuildChannel).Guild.Id) ?? "!";
            if (!message.HasStringPrefix(prefix, ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess) await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}