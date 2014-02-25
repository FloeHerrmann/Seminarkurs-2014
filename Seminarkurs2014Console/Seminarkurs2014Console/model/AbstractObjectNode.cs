using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminarkurs2014Console.model {
	/// <summary>
	/// Abstrakte Klasse für die meisten nodes
	/// </summary>
	abstract public class AbstractObjectNode {

		/// <summary>
		/// ID of the node
		/// </summary>
		private Int64 ID;
		
		/// <summary>
		/// ID of nodes parent
		/// </summary>
		private Int64 ParentID;
		
		/// <summary>
		/// Node type
		/// </summary>
		private Int32 Type;
		
		/// <summary>
		/// Node path
		/// </summary>
		private String Path;
		
		/// <summary>
		/// Node name
		/// </summary>
		private String Name;
		
		/// <summary>
		/// Node description
		/// </summary>
		private String Description;
		
		/// <summary>
		/// The date when the node was last updated/modified
		/// </summary>
		private DateTime LastUpdated;

		/// <summary>
		/// Set the id of the node
		/// </summary>
		public void SetID ( Int64 ID ) {
			this.ID = ID;
		}

		/// <summary>
		/// Get the id of the node
		/// </summary>
		public Int64 GetID () {
			return this.ID;
		}

		/// <summary>
		/// Set the id of the parend node
		/// </summary>
		public void SetParentID ( Int64 ParentID ) {
			this.ParentID = ParentID;
		}

		/// <summary>
		/// Get the id of the parent node
		/// </summary>
		public Int64 GetParentID () {
			return this.ParentID;
		}

		/// <summary>
		/// Set the type of the node
		/// </summary>
		public void SetType ( Int32 Type ) {
			this.Type = Type;
		}

		/// <summary>
		/// Get the type of the node
		/// </summary>
		new public Int32 GetType () {
			return this.Type;
		}

		/// <summary>
		/// Set the path of the node
		/// </summary>
		public void SetPath ( String Path ) {
			this.Path = Path;
		}

		/// <summary>
		/// Get the path of the node
		/// </summary>
		public String GetPath () {
			return this.Path;
		}

		/// <summary>
		/// Set the name of the node
		/// </summary>
		public void SetName ( String Name ) {
			this.Name = Name;
		}

		/// <summary>
		/// Get the name of the node
		/// </summary>
		public String GetName () {
			return this.Name;
		}

		/// <summary>
		/// Set the description of the node
		/// </summary>
		public void SetDescription ( String Description ) {
			this.Description = Description;
		}

		/// <summary>
		/// Get the description of the node
		/// </summary>
		public String GetDescription () {
			return this.Description;
		}

		/// <summary>
		/// Set the date when the node was updated/modified the last time
		/// </summary>
		public void SetLastUpdated ( DateTime LastUpdated ) {
			this.LastUpdated = LastUpdated;
		}

		/// <summary>
		/// Get the date when the node was updated/modified the last time
		/// </summary>
		public DateTime GetLastUpdated () {
			return this.LastUpdated;
		}
	}
}
