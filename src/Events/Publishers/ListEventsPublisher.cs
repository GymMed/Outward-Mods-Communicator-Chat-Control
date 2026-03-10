using OutwardModsCommunicatorChatControl.Utility.Enums;
using OutwardModsCommunicatorChatControl.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutwardModsCommunicatorChatControl.Events.Publishers
{
    public static class ListEventsPublisher
    {
        public static void SendListEventsCommand()
        {
            Action<Character, Dictionary<string, string>> function = TriggerFunction;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "events",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = new Dictionary<string, (string, string)>(),
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Lists all registered events from OutwardModsCommunicator."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void TriggerFunction(Character caller, Dictionary<string, string> arguments)
        {
            ChatPanel panel = caller?.CharacterUI?.ChatPanel;

            if (panel == null)
            {
                OMCCC.LogMessage("ListEventsPublisher@TriggerFunction: No chat panel found");
                return;
            }

            var registeredEvents = EventBusHelpers.GetRegisteredEvents();

            if (registeredEvents == null || registeredEvents.Count == 0)
            {
                ChatHelpers.SendChatLog(panel, "0 events registered", ChatLogStatus.Info);
                return;
            }

            int totalEvents = registeredEvents.Values.Sum(modEvents => modEvents.Count);
            ChatHelpers.SendChatLog(panel, $"{totalEvents} events registered", ChatLogStatus.Success);

            foreach (var modNamespace in registeredEvents)
            {
                ChatHelpers.SendChatLog(panel, $"Mod: {modNamespace.Key}", ChatLogStatus.Info);

                foreach (var eventEntry in modNamespace.Value)
                {
                    string eventName = eventEntry.Key;
                    string description = string.IsNullOrEmpty(eventEntry.Value.Description) ? "(no description)" : eventEntry.Value.Description;

                    ChatHelpers.SendChatLog(panel, $"  - {eventName}: {description}", ChatLogStatus.Info);
                }
            }
        }
    }
}
