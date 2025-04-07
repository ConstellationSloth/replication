using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace godot.Objects
{
	// This is the object representing the player in the database
	public class Player
	{
		public string Name { get; set; }
		public int Id { get; set; }
		public string Map { get; set;}
		public int Level { get; set; }
		public int Exp { get; set; }
		public int Money { get; set; }
		public int SkillPoints { get; set; }
		// do we want to store guild id here as well?
	}
}
