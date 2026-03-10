using Mono.Cecil;
using OutwardModsCommunicatorChatControl.Events.Publishers;
using OutwardModsCommunicatorChatControl.Managers;
using OutwardModsCommunicatorChatControl.Utility.Enums;
using OutwardModsCommunicatorChatControl.Utility.Helpers;
using OutwardModsCommunicator.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutwardModsCommunicatorChatControl.Events
{
    public static class EventBusPublisher
    {
        public const string Event_AddCommand = "ChatCommandsManager@AddChatCommand";
        public const string Event_RemoveCommand = "ChatCommandsManager@RemoveChatCommand";

        //other mods listener uid
        public const string ChatCommands_Listener = "gymmed.chat_commands_manager_*";

        public static void SendCommands()
        {
            ListEventsPublisher.SendListEventsCommand();
            GetEventPublisher.SendGetEventCommand();
            PublishEventPublisher.SendPublishCommand();
        }
    }
}
