using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace CPNewbieBot
{
    internal class Program
    {
        const string TelegramBotToken = "692910923:AAGbJ9gv9Sw1U1I78gFAAqDvK1CQZuWinD8";


        static ITelegramBotClient TelegramBotClient;

        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            TelegramBotClient = new TelegramBotClient(TelegramBotToken);

            var me = await TelegramBotClient.GetMeAsync();
            Console.WriteLine("Telegram bot started!");

            TelegramBotClient.OnMessage += BotClient_OnMessage;
            TelegramBotClient.StartReceiving();

            while (true) Thread.Sleep(int.MaxValue);
        }

        static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            var msg = e.Message;
            var text = msg.Text.Trim();
            var responder = msg.From?.Username ?? "user";
            const string TelegramBotName = "@cpnewbiebot", TelegramBotCommand = "/search";

            // First type of message: reply referring me
            if (msg.ReplyToMessage != null && (text.Contains(TelegramBotName) || text.Contains(TelegramBotCommand)))
            {
                var finder = new DefinitionFinder();
                await finder.Parse(msg.ReplyToMessage.Text, responder);
                await TelegramBotClient.SendTextMessageAsync(msg.Chat, finder.Message, ParseMode.Markdown,
                    replyToMessageId: msg.MessageId);
            }
            // Second type of message: manual search
            else if (text.StartsWith(TelegramBotCommand))
            {
                var finder = new DefinitionFinder();
                await finder.Parse(text.Substring(TelegramBotCommand.Length), responder);
                await TelegramBotClient.SendTextMessageAsync(msg.Chat, finder.Message, ParseMode.Markdown,
                    replyToMessageId: msg.MessageId);
            }
            // "Fallback message": referring me but no reply
            else if (msg.ReplyToMessage == null && text.Contains(TelegramBotName))
            {
                var defaultMessage = $"Hi, {responder}! How could I help you today?\n\n" +
                    $"You can search for unknown terms in a message by replying to it with the command {TelegramBotCommand} or by " +
                    $"tagging me on the reply ({TelegramBotName}).\nYou can also search for a specific keyword by typing " +
                    $"{TelegramBotCommand} [keyword]. One is glad to be of service!";
                await TelegramBotClient.SendTextMessageAsync(msg.Chat, defaultMessage, replyToMessageId: msg.MessageId);
            }
        }
    }
}
