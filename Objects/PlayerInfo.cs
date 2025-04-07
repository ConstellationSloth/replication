using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace godot.Objects
{
    public class PlayerInfo
    {
        // This is going to be the version that goes out to the clients of other people
        public string Name { get; set; }
        public int Id { get; set; }
        public int NetworkId { get; set; }
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
        // Should this instead be a guild object?
        public int? GuildId { get; set; } // Nullable in case they are not in a guild
        // TODO: should I add health here? 
        // TODO: put equipment, title, guild, party here

    }
}
