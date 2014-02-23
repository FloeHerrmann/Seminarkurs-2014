using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminarkurs2014Console.model {
	abstract public class AbstractObjectNode {

		private Int64 ID;
		private Int64 ParentID;
		private Int32 Type;
		private String Path;
		private String Name;
		private String Description;
		private DateTime LastUpdated;

		public void SetID ( Int64 ID ) {
			this.ID = ID;
		}
		public Int64 GetID () {
			return this.ID;
		}

		public void SetParentID ( Int64 ParentID ) {
			this.ParentID = ParentID;
		}
		public Int64 GetParentID () {
			return this.ParentID;
		}

		public void SetType ( Int32 Type ) {
			this.Type = Type;
		}
		public Int32 GetType () {
			return this.Type;
		}

		public void SetPath ( String Path ) {
			this.Path = Path;
		}
		public String GetPath () {
			return this.Path;
		}

		public void SetName ( String Name ) {
			this.Name = Name;
		}
		public String GetName () {
			return this.Name;
		}

		public void SetDescription ( String Description ) {
			this.Description = Description;
		}
		public String GetDescription () {
			return this.Description;
		}

		public void SetLastUpdated ( DateTime LastUpdated ) {
			this.LastUpdated = LastUpdated;
		}
		public DateTime GetLastUpdated () {
			return this.LastUpdated;
		}
	}
}
