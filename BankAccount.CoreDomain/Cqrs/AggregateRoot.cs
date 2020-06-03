﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BankAccount.CoreDomain.Cqrs
{
    public abstract class AggregateRoot<TEvent> : IAggregateRoot<TEvent> where TEvent : notnull
    {
        private const string ApplyMethodName = "Apply";
        private readonly Dictionary<Type, ApplyInvocation> applyInvocations = new Dictionary<Type, ApplyInvocation>();
        private readonly OId<BankAccount, Guid> id;
        private readonly List<TEvent> uncommittedEvents = new List<TEvent>();
        private int initialVersion;
        private int version;

        protected AggregateRoot(OId<BankAccount, Guid> id)
        {
            this.id = id;
        }

        OId<BankAccount, Guid> IAggregateRoot<TEvent>.Id => id;

        int IAggregateRoot<TEvent>.Version => version;

        int IAggregateRoot<TEvent>.InitialVersion => initialVersion;

        IReadOnlyCollection<TEvent> IAggregateRoot<TEvent>.GetUncommittedEvents() => uncommittedEvents;

        void IAggregateRoot<TEvent>.OnEventsCommitted() => uncommittedEvents.Clear();

        protected void RaiseEvent(TEvent @event)
        {
            ApplyEvent(@event);
            uncommittedEvents.Add(@event);
        }

        public void Replay(IEnumerable<TEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyEvent(@event);
                initialVersion++;
            }
        }

        private void ApplyEvent(TEvent @event)
        {
            var eventType = @event.GetType();
            if (!applyInvocations.TryGetValue(eventType, out var applyInvocation))
            {
                var domainObjectType = GetType().GetTypeInfo();
                applyInvocation = domainObjectType.DeclaredMethods
                    .Where(IsApplyMethod)
                    .Where(method => HasCorrectParameters(method, eventType))
                    .Select(method => new ApplyInvocation(method, this))
                    .FirstOrDefault();

                if (applyInvocation == null)
                {
                    throw new EventNotRecognizedException($"Type {domainObjectType} does not implement IApply<{eventType}>");
                }

                applyInvocations.Add(eventType, applyInvocation);
            }

            applyInvocation.Execute(@event);
            version++;
        }

        private static bool IsApplyMethod(MethodInfo method)
        {
            var isApplyMethod = method.Name == ApplyMethodName;
            return isApplyMethod || method.Name.EndsWith($".{ApplyMethodName}");
        }

        private static bool HasCorrectParameters(MethodBase method, Type eventType)
        {
            var parameters = method.GetParameters();
            var isApplyMethod = parameters.Length == 1;
            if (isApplyMethod)
            {
                var firstParameter = parameters[0];
                isApplyMethod = firstParameter.ParameterType == eventType;
            }

            return isApplyMethod;
        }

        private class ApplyInvocation
        {
            private readonly object _domainObject;
            private readonly MethodInfo _method;

            internal ApplyInvocation(MethodInfo method, object domainObject)
            {
                _method = method;
                _domainObject = domainObject;
            }

            internal void Execute(object @event) => _method.Invoke(_domainObject, new[] { @event });
        }
    }
}