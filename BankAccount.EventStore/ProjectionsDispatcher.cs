using System;
using System.Collections.Generic;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace BankAccount.EventStore
{
    public class ProjectionsDispatcher : IDisposable
    {
        private readonly IPositionStore positionStore;
        private readonly IEnumerable<IProjection> projections;
        private readonly ILog log;

        public ProjectionsDispatcher(
            IEventStoreConnection eventStoreConnection,
            IPositionStore positionStore,
            IEnumerable<IProjection> projections,
            ILog log)
        {
            EventStoreConnection = eventStoreConnection;
            this.positionStore = positionStore;
            this.projections = projections;
            this.log = log;
        }

        private IEventStoreConnection EventStoreConnection { get; }

        public void Start(Action<IProjection> onProjectionLiveProcessingStarted = null)
        {
            foreach (var projection in projections)
            {
                StartProjection(projection, onProjectionLiveProcessingStarted);
            }
        }

        private void StartProjection(IProjection projection, Action<IProjection> onProjectionLiveProcessingStarted)
        {
            log.Info($"PROJECTION STARTING - {projection.Id}");
            try
            {
                var checkpoint = positionStore.GetLatest(projection.Id);

                EventStoreConnection.SubscribeToStreamFrom("$ce-BankAccount",
                    0,
                    CatchUpSubscriptionSettings.Default,
                    EventAppeared(projection),
                    LiveProcessingStarted(projection, onProjectionLiveProcessingStarted));
            }
            catch (Exception exception)
            {
                throw new ProjectionStartFailedException($"Starting projection {projection.Id} failed", exception);
            }

            log.Info($"PROJECTION STARTED - {projection.Id}");
        }

        private Action<EventStoreCatchUpSubscription> LiveProcessingStarted(IProjection projection, Action<IProjection> onProjectionLiveProcessingStarted)
        {
            return subscription =>
            {
                log.Info($"PROJECTION HAS CAUGHTUP - {projection.Id} - Now processing live");
                onProjectionLiveProcessingStarted?.Invoke(projection);
            };
        }

        private Action<EventStoreCatchUpSubscription, ResolvedEvent> EventAppeared(IProjection projection)
        {
            return (subscription, resolvedEvent) =>
            {
                var currentPosition = resolvedEvent.OriginalPosition;
                var projectionId = projection.Id;
                try
                {
                    var @event = resolvedEvent.Event;
                    if (!projection.CanHandle(@event.EventType))
                    {
                        return;
                    }

                    var deserializedEvent = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(resolvedEvent.Event.Data), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    projection.Handle(@event.EventType, deserializedEvent);
                    positionStore.Update(projectionId, currentPosition);
                    log.Debug($"EVENT HANDLED: EventType: {@event.EventType} AggregateId: {resolvedEvent.OriginalStreamId} Projection: {projectionId}");
                }
                catch (Exception exception)
                {
                    var aggregateId = resolvedEvent.OriginalStreamId;
                    var position = currentPosition.HasValue ? currentPosition.ToString() : "unknown";
                    log.Error(
                        $"EXCEPTION DURING PROJECTION UPDATE - AggregateId: {aggregateId} Projection: {projectionId} Position: {position}",
                        exception);
                    try
                    {
                        positionStore.OnError(projectionId, resolvedEvent.Event.EventType, aggregateId, currentPosition, exception);
                    }
                    catch (Exception ex)
                    {
                        log.Error($"EXCEPTION DURING ERROR POSITION UPDATE - AggregateId: {aggregateId} Projection: {projectionId} Position: {position}", ex);
                    }
                }
            };
        }

        public void Dispose()
        {
            EventStoreConnection.Dispose();
        }
    }
}