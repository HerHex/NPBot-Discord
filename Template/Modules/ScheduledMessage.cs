using System;
using System.Linq;
using System.Threading; 
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;

public class TimerService
{
    private readonly Timer _timer;
    public readonly Servers _servers;
    private readonly DiscordSocketClient _client;// 2) Add a field like this



    // This example only concerns a single timer.
    // If you would like to have multiple independant timers,
    // you could use a collection such as List<Timer>,
    // or even a Dictionary<string, Timer> to quickly get
    // a specific Timer instance by name.

    public TimerService(DiscordSocketClient client)
    {
        

        _timer = new Timer(async _ =>
        {
            // 3) Any code you want to periodically run goes here, for example:
            
            var chan = client.GetChannel(839915981018103831) as IMessageChannel;
           
            var message = await chan.SendMessageAsync("Have you completed your commitment for today !?");
            if (chan != null)
            {
                

                var heartEmoji = new Emoji("\U0001f495");
                var randoEmoji = new Emoji("\uD83D\uDD3C");
                // Reacts to the message with the Emoji.
                await message.AddReactionAsync(heartEmoji);
                await message.AddReactionAsync(randoEmoji);
                



            }



        },
        null,
        TimeSpan.FromMinutes(1),  // 4) Time that message should fire after the timer is created
        TimeSpan.FromMinutes(1));
        // 5) Time after which message should repeat (use `Timeout.Infinite` for no repeat)
    }

    public void StartNow()
    {
        _timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromHours(24));
        
    }

    public void Stop() // 6) Example to make the timer stop running
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
    }

    public void Restart() // 7) Example to restart the timer
    {
        _timer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }
}
