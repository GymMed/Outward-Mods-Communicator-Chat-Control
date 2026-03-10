using OutwardModsCommunicatorChatControl.Utility.Enums;
using OutwardModsCommunicatorChatControl.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;

namespace OutwardModsCommunicatorChatControl.Events.Publishers
{
    public static class GetEventPublisher
    {
        public static void SendGetEventCommand()
        {
            Action<Character, Dictionary<string, string>> function = TriggerFunction;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "event",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = new Dictionary<string, (string, string)>
                {
                    { "mod", ("Mod namespace (optional if searching all mods).", null) },
                    { "event", ("Event name to look up.", null) }
                },
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Gets detailed information about a specific event."
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void TriggerFunction(Character caller, Dictionary<string, string> arguments)
        {
            ChatPanel panel = caller?.CharacterUI?.ChatPanel;

            if (panel == null)
            {
                OMCCC.LogMessage("GetEventPublisher@TriggerFunction: No chat panel found");
                return;
            }

            arguments.TryGetValue("mod", out string modNamespace);
            arguments.TryGetValue("event", out string eventName);

            if (string.IsNullOrEmpty(eventName))
            {
                EventBusHelpers.SendMissingParams(panel);
                return;
            }

            EventDefinition eventDef;
            string foundNamespace;

            if (!string.IsNullOrEmpty(modNamespace))
            {
                if (!EventBusHelpers.TryGetEvent(modNamespace, eventName, out eventDef))
                {
                    EventBusHelpers.SendEventNotFound(panel, modNamespace, eventName);
                    return;
                }
                foundNamespace = modNamespace;
            }
            else
            {
                if (!EventBusHelpers.TryFindEvent(eventName, out foundNamespace, out eventDef))
                {
                    ChatHelpers.SendChatLog(panel, $"Event '{eventName}' not found in any mod", ChatLogStatus.Error);
                    return;
                }
            }

            EventBusHelpers.SendEventInfo(panel, foundNamespace, eventName, eventDef);
        }
    }
}
