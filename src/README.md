<h1 align="center">
    Outward Mods Communicator Chat Control
</h1>
<br/>
<div align="center">
  <img src="https://raw.githubusercontent.com/GymMed/Outward-Mods-Communicator-Chat-Control/refs/heads/main/preview/images/Logo.png" alt="Logo"/>
</div>

<div align="center">
	<a href="https://thunderstore.io/c/outward/p/GymMed/Mods-Communicator-Chat-Control/">
		<img src="https://img.shields.io/thunderstore/dt/GymMed/Mods_Communicator_Chat_Control" alt="Thunderstore Downloads">
	</a>
	<a href="https://github.com/GymMed/Outward-Mods-Communicator-Chat-Control/releases/latest">
		<img src="https://img.shields.io/thunderstore/v/GymMed/Mods_Communicator_Chat_Control" alt="Thunderstore Version">
	</a>
	<a href="https://github.com/GymMed/Outward-Mods-Communicator/releases/latest">
		<img src="https://img.shields.io/badge/Mods_Communicator-v1.2.0-9966ff" alt="Mods Communicator Version">
	</a>
	<a href="https://github.com/GymMed/Outward-Chat-Commands-Manager/releases/latest">
		<img src="https://img.shields.io/badge/Chat_Commands_Manager-v0.1.0-33ccff" alt="Chat Commands Manager">
	</a>
</div>

<div align="center">
    Provides access to <a href="https://github.com/GymMed/Outward-Mods-Communicator">
    Mods Communicator</a> through chat. 
    It enables event-driven communication between mods, allowing users to
    interact with them directly through chat.
</div>

## How to use it

Firstly, install [Chat Commands
Manager](https://github.com/GymMed/Outward-Chat-Commands-Manager).
After that, you can use the commands provided by this mod directly in chat.

### Built-in Commands

<details>
    <summary>/events</summary>
Lists all registered events from OutwardModsCommunicator.<br>

<b>Usage:</b>
<code>/events</code>
</details>

<details>
    <summary>/event</summary>
Gets detailed information about a specific event including its parameters and types.<br>

<b>Usage:</b>
<code>/event gymmed.chat_commands_manager_* ChatCommandsManager@AddChatCommand</code><br>
<code>/event --mod=gymmed.chat_commands_manager_* --event=ChatCommandsManager@AddChatCommand</code><br>
<code>/event gymmed.loot_manager_* AddLoot</code><br>
</details>

<details>
    <summary>/publish</summary>
Publishes an event to OutwardModsCommunicator directly from chat. Parameters are dynamically parsed based on the event's registered schema in Mods Communicator.<br>

<b>How it works:</b>
The mod reads the registered event parameters from Mods Communicator and automatically parses your chat input to match the expected types. If a parameter type cannot be parsed from a string (like functions or delegates), it will be skipped. Unregistered parameters are silently ignored, giving you flexibility.<br>

<b>Usage:</b>
<code>/publish gymmed.loot_manager_* LootRulesSerializer@SaveLootRulesToXml</code><br>
<code>/publish gymmed.loot_manager_* LootRulesSerializer@LoadCustomLoots --filePath="C:/documents/myPath"</code><br>
<code>/publish gymmed.loot_manager_* AddLoot --itemId=4300040 --faction=bandits</code><br>
<code>/publish gymmed.loot_manager_* AddLoot --itemId=4300040 --faction="bandits"</code><br>
<code>/publish --event=AddLoot --itemId=4300040 --faction=Deer</code><br>

<b>Supported Types:</b>
- Primitives: string, int, float, bool, double, long, decimal, char
- Enums: Any game enum (e.g., Character.Factions.Bandits, Character.Factions.Deer)
- Nullable&lt;T&gt;: Optional enum/primitive types (e.g., Nullable&lt;Character.Factions&gt;)
- IEnumerable/ICollection: Space-separated values (e.g., --names="Sword Shield" becomes List&lt;string&gt;)
- Arrays: Space-separated values (e.g., --ids="4300040 4300041")

<b>Notes:</b>
- Parameters can be passed positionally (in order) or by name (--param=value)
- If mod namespace is omitted, searches all registered mods for the event
- If a value cannot be parsed to the expected type, an error is shown and the event is NOT published
- Unregistered parameters (not defined in the event schema) are silently dropped - they are not added to the event payload
- Each mod is responsible for validating and handling the received data in their own event handlers
</details>

## How to set up

To manually set up, do the following

1. Create the directory: `Outward\BepInEx\plugins\OutwardModsCommunicatorChatControl\`.
2. Extract the archive into any directory(recommend empty).
3. Move the contents of the plugins\ directory from the archive into the `BepInEx\plugins\OutwardModsCommunicatorChatControl\` directory you created.
4. It should look like `Outward\BepInEx\plugins\OutwardModsCommunicatorChatControl\OutwardModsCommunicatorChatControl.dll`
   Launch the game.

### If you liked the mod leave a star on [GitHub](https://github.com/GymMed/Outward-Mods-Communicator-Chat-Control) it's free
