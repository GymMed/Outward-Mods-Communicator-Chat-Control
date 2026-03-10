using OutwardModsCommunicatorChatControl.Utility.Enums;
using OutwardModsCommunicatorChatControl.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OutwardModsCommunicatorChatControl.Events.Publishers
{
    public static class PublishEventPublisher
    {
        public static void SendPublishCommand()
        {
            Action<Character, Dictionary<string, string>> function = TriggerFunction;

            var payload = new EventPayload
            {
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandName).key] = "publish",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandParameters).key] = new Dictionary<string, (string, string)>
                {
                    { "mod", ("Mod namespace (optional if searching all mods).", null) },
                    { "event", ("Event name.", null) }
                },
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandAction).key] = function,
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Publishes an event to OutwardModsCommunicator. Requires debug mode.",
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandRequiresDebugMode).key] = true,
                //[ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.IsCheatCommand).key] = true,
            };

            EventBus.Publish(EventBusPublisher.ChatCommands_Listener, EventBusPublisher.Event_AddCommand, payload);
        }

        public static void TriggerFunction(Character caller, Dictionary<string, string> arguments)
        {
            ChatPanel panel = caller?.CharacterUI?.ChatPanel;

            if (panel == null)
            {
                OMCCC.LogMessage("PublishEventPublisher@TriggerFunction: No chat panel found");
                return;
            }

            arguments.TryGetValue("mod", out string modNamespace);
            arguments.TryGetValue("event", out string eventName);

            if (string.IsNullOrEmpty(eventName))
            {
                SendUsageError(panel);
                return;
            }

            EventDefinition eventDef;
            string foundNamespace;

            if (!string.IsNullOrEmpty(modNamespace))
            {
                if (!EventBusHelpers.TryGetEvent(modNamespace, eventName, out eventDef))
                {
                    ChatHelpers.SendChatLog(panel, $"Event '{eventName}' not found in mod '{modNamespace}'", ChatLogStatus.Error);
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

            var payload = new EventPayload();
            var errors = new List<string>();

            // Get registered parameter names for this event
            var registeredParams = eventDef.Schema?.Fields?.Keys ?? new Dictionary<string, Type>().Keys;

            // Process each user-provided argument
            foreach (var userArg in arguments)
            {
                string paramName = userArg.Key;
                string stringValue = userArg.Value;

                // Skip mod and event - they're handled separately
                if (paramName == "mod" || paramName == "event")
                    continue;

                // If param is in registered schema, validate and parse
                if (registeredParams.Contains(paramName))
                {
                    Type paramType = eventDef.Schema.Fields[paramName];
                    if (EventArgumentParser.TryParse(paramType, stringValue, out object parsedValue))
                    {
                        payload[paramName] = parsedValue;
                    }
                    else
                    {
                        errors.Add($"Cannot parse '{stringValue}' for parameter '{paramName}' as {paramType.Name}");
                    }
                }
                // If param is NOT in registered schema, silently ignore (user freedom)
            }

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    ChatHelpers.SendChatLog(panel, error, ChatLogStatus.Error);
                }
                return;
            }

            EventBus.Publish(foundNamespace, eventName, payload);
            ChatHelpers.SendChatLog(panel, $"Published event '{foundNamespace}.{eventName}'", ChatLogStatus.Success);
        }

        private static void SendUsageError(ChatPanel panel)
        {
            ChatHelpers.SendChatLog(panel, "Missing --event parameter. Usage: /publish <mod> <event> [--param=value...]", ChatLogStatus.Warning);
        }
    }
}
