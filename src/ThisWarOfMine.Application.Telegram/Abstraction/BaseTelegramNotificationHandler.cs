using MediatR;
using Telegram.Bot;

namespace ThisWarOfMine.Application.Telegram.Abstraction
{
    internal abstract class BaseTelegramNotificationHandler<T>
        : INotificationHandler<TelegramNotification<T>>,
            ITelegramNotificationHandler,
            INotificationContextInitializer
    {
        private TelegramNotificationHandlingContext<T>? _context;

        #region Protected Properties

        protected TelegramNotificationHandlingContext<T> Context =>
            _context ?? throw new InvalidOperationException("Context is not initialized yet");

        protected ITelegramBotClient Client => Context.Client;

        protected TelegramNotification<T> Notification => Context.TelegramNotification;

        protected T Payload => Notification.Payload;

        #endregion

        public abstract Task<bool> CanHandleAsync(CancellationToken token);

        public abstract Task HandleAsync(CancellationToken token);

        void INotificationContextInitializer.SetContext(INotification notification, ITelegramBotClient client)
        {
            lock (this)
            {
                _context ??= new TelegramNotificationHandlingContext<T>((TelegramNotification<T>)notification, client);
            }
        }

        Task INotificationHandler<TelegramNotification<T>>.Handle(
            TelegramNotification<T> telegramNotification,
            CancellationToken cancellationToken
        ) => HandleAsync(cancellationToken);
    }
}
