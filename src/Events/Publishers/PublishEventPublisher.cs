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
                [ChatCommandsManagerParamsHelper.Get(ChatCommandsManagerParams.CommandDescription).key] = "Publishes an event to OutwardModsCommunicator with proper type casting for collections (HashSet, List, arrays), enums, and other types. Usage: /publish [mod] event [--param=value...]",
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

                // If param is in registered schema, validate and parse with proper type casting
                if (registeredParams.Contains(paramName))
                {
                    Type paramType = eventDef.Schema.Fields[paramName];
                    var (success, parsedValue, error) = EventArgumentParser.TryParseWithDetails(paramType, stringValue);
                    
                    if (success)
                    {
                        payload[paramName] = parsedValue;
                    }
                    else
                    {
                        errors.Add($"Parameter '{paramName}' ({paramType.Name}): {error ?? "Unknown error"}");
                    }
                }
                // If param is NOT in registered schema, try to auto-detect type and add dynamically
                else
                {
                    var autoDetectedValue = TryAutoDetectAndParse(stringValue);
                    if (autoDetectedValue != null)
                    {
                        payload[paramName] = autoDetectedValue;
                        ChatHelpers.SendChatLog(panel, $"Dynamic parameter '{paramName}' auto-detected as {autoDetectedValue.GetType().Name}", ChatLogStatus.Info);
                    }
                    // Silently ignore unknown parameters (user freedom)
                }
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

        /// <summary>
        /// Attempts to auto-detect the type of a value string and parse it accordingly.
        /// Returns null if unable to detect/parse.
        /// </summary>
        private static object TryAutoDetectAndParse(string valueString)
        {
            if (string.IsNullOrWhiteSpace(valueString))
                return null;

            valueString = valueString.Trim();

            // Try as string array first (space-separated)
            var parts = valueString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1)
            {
                // Try as int array
                if (parts.All(p => int.TryParse(p, out _)))
                {
                    var intArray = new int[parts.Length];
                    for (int i = 0; i < parts.Length; i++)
                    {
                        int.TryParse(parts[i], out intArray[i]);
                    }
                    return intArray;
                }

                // Default to string array
                return parts;
            }

            // Single value - try int first, then string
            if (int.TryParse(valueString, out int intValue))
                return intValue;

            if (bool.TryParse(valueString, out bool boolValue))
                return boolValue;

            // Default to string
            return valueString;
        }

        private static void SendUsageError(ChatPanel panel)
        {
            ChatHelpers.SendChatLog(panel, "Missing --event parameter. Usage: /publish <mod> <event> [--param=value...]", ChatLogStatus.Warning);
        }
    }
}
