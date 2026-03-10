using OutwardModsCommunicatorChatControl.Utility.Enums;
using OutwardModsCommunicator.EventBus;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutwardModsCommunicatorChatControl.Utility.Helpers
{
    public static class EventBusHelpers
    {
        public static IReadOnlyDictionary<string, Dictionary<string, EventDefinition>> GetRegisteredEvents()
        {
            return EventBus.GetRegisteredEvents();
        }

        public static bool TryGetEvent(string modNamespace, string eventName, out EventDefinition eventDef)
        {
            eventDef = null;
            var events = GetRegisteredEvents();

            if (events == null)
                return false;

            if (!events.TryGetValue(modNamespace, out var modEvents))
                return false;

            return modEvents.TryGetValue(eventName, out eventDef);
        }

        public static bool TryFindEvent(string eventName, out string foundNamespace, out EventDefinition eventDef)
        {
            eventDef = null;
            foundNamespace = null;
            var events = GetRegisteredEvents();

            if (events == null)
                return false;

            foreach (var mod in events)
            {
                if (mod.Value.TryGetValue(eventName, out eventDef))
                {
                    foundNamespace = mod.Key;
                    return true;
                }
            }

            return false;
        }

        public static void SendEventNotFound(ChatPanel panel, string modNamespace, string eventName)
        {
            ChatHelpers.SendChatLog(panel, $"Event '{eventName}' not found in mod '{modNamespace}'", ChatLogStatus.Error);
        }

        public static void SendModNotFound(ChatPanel panel, string modNamespace)
        {
            ChatHelpers.SendChatLog(panel, $"Mod '{modNamespace}' not found", ChatLogStatus.Error);
        }

        public static void SendMissingParams(ChatPanel panel)
        {
            ChatHelpers.SendChatLog(panel, "Missing --mod or --event parameter. Usage: /event --mod=<namespace> --event=<name>", ChatLogStatus.Warning);
        }

        public static void SendEventInfo(ChatPanel panel, string modNamespace, string eventName, EventDefinition eventDef)
        {
            ChatHelpers.SendChatLog(panel, $"{modNamespace}.{eventName}", ChatLogStatus.Success);

            string description = string.IsNullOrEmpty(eventDef.Description) ? "(no description)" : eventDef.Description;
            ChatHelpers.SendChatLog(panel, $"Description: {description}", ChatLogStatus.Info);

            if (eventDef.Schema.Fields.Count > 0)
            {
                ChatHelpers.SendChatLog(panel, "Parameters:", ChatLogStatus.Info);

                foreach (var field in eventDef.Schema.Fields)
                {
                    string fieldDesc = eventDef.Schema.GetDescription(field.Key) ?? "(no description)";
                    ChatHelpers.SendChatLog(panel, $"  - {field.Key} ({GetTypeName(field.Value)}): {fieldDesc}", ChatLogStatus.Info);
                }
            }
            else
            {
                ChatHelpers.SendChatLog(panel, "No parameters defined.", ChatLogStatus.Info);
            }
        }

        private static string GetTypeName(System.Type type)
        {
            if (type == null)
                return "Unknown";

            if (type.IsGenericType)
            {
                string name = type.Name.Split('`')[0];
                string args = string.Join(", ", type.GetGenericArguments().Select(t => t.Name));
                return $"{name}<{args}>";
            }

            return type.Name;
        }
    }
}
