using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using MediatR;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using ThisWarOfMine.Application.Telegram.Abstraction;
using ThisWarOfMine.Common.Wrappers;

namespace ThisWarOfMine.Infrastructure.Telegram.Notification
{
    internal sealed class NotificationCreator : INotificationCreator
    {
        private readonly IGuidProvider _guidProvider;

        private static readonly ImmutableSortedDictionary<UpdateType, Route> Mapping = new Dictionary<UpdateType, Route>
        {
            { UpdateType.Message, Route.From<Message>(x => x.Message) },
            { UpdateType.InlineQuery, Route.From<InlineQuery>(x => x.InlineQuery) },
            { UpdateType.ChosenInlineResult, Route.From<ChosenInlineResult>(x => x.ChosenInlineResult) },
            { UpdateType.CallbackQuery, Route.From<CallbackQuery>(x => x.CallbackQuery) },
            { UpdateType.EditedMessage, Route.From<Message>(x => x.EditedMessage) },
            { UpdateType.ChannelPost, Route.From<Message>(x => x.ChannelPost) },
            { UpdateType.EditedChannelPost, Route.From<Message>(x => x.EditedChannelPost) },
            { UpdateType.ShippingQuery, Route.From<ShippingQuery>(x => x.ShippingQuery) },
            { UpdateType.PreCheckoutQuery, Route.From<PreCheckoutQuery>(x => x.PreCheckoutQuery) },
            { UpdateType.Poll, Route.From<Poll>(x => x.Poll) },
            { UpdateType.PollAnswer, Route.From<PollAnswer>(x => x.PollAnswer) },
            { UpdateType.MyChatMember, Route.From<ChatMemberUpdated>(x => x.MyChatMember) },
            { UpdateType.ChatMember, Route.From<ChatMemberUpdated>(x => x.ChatMember) },
            { UpdateType.ChatJoinRequest, Route.From<ChatJoinRequest>(x => x.ChatJoinRequest) }
        }.ToImmutableSortedDictionary();

        public NotificationCreator(IGuidProvider guidProvider)
        {
            _guidProvider = guidProvider;
        }

        public INotification CreateFrom(Update update)
        {
            var route = Mapping
                .TryFind(update.Type)
                .GetValueOrThrow($"Cannot map update type: `{update.Type}` to actual type");

            return route.Create(_guidProvider.NewGuid(), update);
        }

        private sealed record Route(Type Type, Func<Update, object> Selector)
        {
            public INotification Create(Guid guid, Update update)
            {
                var notificationType = typeof(TelegramNotification<>).MakeGenericType(Type);
                var payload = Selector(update);
                return (INotification)Activator.CreateInstance(notificationType, guid, update.Id, payload)!;
            }

            public static Route From<T>(Func<Update, T?> selector) => new(typeof(T), x => selector(x)!);
        }
    }
}
