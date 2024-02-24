using MediatR;
using Telegram.Bot;

namespace ThisWarOfMine.Application.Telegram.Abstraction
{
    internal sealed class TaskWhenAllOrDefaultTelegramNotificationPublisher : INotificationPublisher
    {
        private static readonly Type TelegramNotificationHandlerMarker = typeof(ITelegramNotificationHandler);
        private readonly ITelegramBotClient _telegramBotClient;

        public TaskWhenAllOrDefaultTelegramNotificationPublisher(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Publish(
            IEnumerable<NotificationHandlerExecutor> handlerExecutors,
            INotification notification,
            CancellationToken cancellationToken
        )
        {
            var allExecutors = handlerExecutors.ToArray();
            var allKnownExecutors = allExecutors.Where(HandlerTypeIsBaseNotificationHandler).ToArray();
            ThrowIfExistsUnknownExecutor(allExecutors, allKnownExecutors);

            var notificationType = notification.GetType();
            ThrowIfNotificationTypeIsNotSupported(notificationType);

            SetContextForAllExecutors(notification, allKnownExecutors);

            var handlers = allKnownExecutors
                .Select(x => x.HandlerInstance)
                .Cast<ITelegramNotificationHandler>()
                .ToArray();
            var nonDefaultHandlers = handlers
                .Where(handler => HandlerIsNotDefault(handler, notificationType))
                .ToArray();
            var success = await ExecuteAllHandlersAsync(nonDefaultHandlers, cancellationToken);
            if (success)
            {
                return;
            }

            var defaultHandlers = handlers.Except(nonDefaultHandlers).ToArray();
            await ExecuteAllHandlersAsync(defaultHandlers, cancellationToken);
        }

        #region Filter Helpers

        private static bool HandlerTypeIsBaseNotificationHandler(NotificationHandlerExecutor executor) =>
            executor.HandlerInstance.GetType().IsAssignableTo(TelegramNotificationHandlerMarker);

        private static bool HandlerIsNotDefault(object handler, Type notificationType) =>
            !HandlerIsDefault(handler, notificationType);

        private static bool HandlerIsDefault(object handler, Type notificationType)
        {
            var payloadType = notificationType.GetGenericArguments()[0];
            var defaultHandlerMarker = typeof(DefaultTelegramNotificationHandler<>).MakeGenericType(payloadType);
            return handler.GetType().IsAssignableTo(defaultHandlerMarker);
        }

        #endregion

        #region Behavior Helpers

        private void SetContextForAllExecutors(
            INotification notification,
            IEnumerable<NotificationHandlerExecutor> executors
        )
        {
            var initializers = executors.Select(x => x.HandlerInstance).OfType<INotificationContextInitializer>();
            foreach (var initializer in initializers)
            {
                initializer.SetContext(notification, _telegramBotClient);
            }
        }

        private static async Task<bool> ExecuteAllHandlersAsync(
            IReadOnlyCollection<ITelegramNotificationHandler> handlers,
            CancellationToken token
        )
        {
            if (!handlers.Any())
            {
                return false;
            }

            var tasks = handlers.Select(handler => ExecuteHandlerAsync(handler, token));
            var results = await Task.WhenAll(tasks);
            var success = results.Any(result => result);
            return success;
        }

        private static async Task<bool> ExecuteHandlerAsync(
            ITelegramNotificationHandler handler,
            CancellationToken token
        )
        {
            var cannotHandle = !await handler.CanHandleAsync(token);
            if (cannotHandle)
            {
                return false;
            }

            await handler.HandleAsync(token);
            return true;
        }

        #endregion

        #region Exception Helpers

        private static void ThrowIfExistsUnknownExecutor(
            IEnumerable<NotificationHandlerExecutor> allExecutors,
            IEnumerable<NotificationHandlerExecutor> allKnownExecutors
        )
        {
            if (allExecutors.Except(allKnownExecutors).ToArray() is { Length: > 0 } unknownExecutors)
            {
                throw new InvalidOperationException(
                    $"Cannot publish notifications for: {string.Join(", ", unknownExecutors.Select(x => x.GetType()))}"
                );
            }
        }

        private static void ThrowIfNotificationTypeIsNotSupported(Type notificationType)
        {
            if (
                !notificationType.IsGenericType
                || notificationType.GetGenericTypeDefinition() != typeof(TelegramNotification<>)
            )
            {
                throw new NotSupportedException($"TelegramNotification of type `{notificationType}` is not supported");
            }
        }

        #endregion
    }
}
