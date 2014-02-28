using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gsmgh.Alm.Database;
using Gsmgh.Alm.Model;

namespace ALM_Interface {
	public partial class mainWindow : Form {

		private Dictionary<String , List<AbstractObjectNode>> parentIdDictionary;

		public mainWindow () {
			InitializeComponent();

			ImageList nodeTreeImageList = new ImageList();
			nodeTreeImageList.Images.Add( Properties.Resources.FolderNodeOpenIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.RootNodeIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.FolderNodeIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.DeviceNodeIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.DatapointNodeIcon );

			nodeTreeView.ImageList = nodeTreeImageList;

			// Create a DatabaseFacade instance
			DatabaseFacade database = new DatabaseFacade();

			// Tell the DatabseFacade to use the MySQLConnector
			database.SetDatabaseConnector(
				new MySQLConnector( "SERVER=localhost;DATABASE=seminarkurs2014;UID=root;PASSWORD=root;" )
			);

			// Open a connection to the datbase
			database.OpenConnection();

			parentIdDictionary = database.GetObjectTreeSortedByParentID();
			populateTreeView();

			database.CloseConnection();

		}

		private void populateTreeView ( ) {
			AbstractObjectNode rootNode = this.parentIdDictionary[ "0" ].ElementAt( 0 );
			TreeNode rootTreeNode = new TreeNode();
			rootTreeNode.Text = rootNode.GetName();
			rootTreeNode.ImageIndex = rootNode.GetType();
			rootTreeNode.SelectedImageIndex = rootNode.GetType();
			rootTreeNode.ToolTipText = rootNode.GetDescription();
			addNodesToNode( rootNode.GetID() , rootTreeNode );
			nodeTreeView.Nodes.Add( rootTreeNode );
		}

		private void addNodesToNode( Int64 parentID , TreeNode parentNode ) {
			if( this.parentIdDictionary.ContainsKey( parentID.ToString() ) ) {
				List<AbstractObjectNode> subNodes = this.parentIdDictionary[ parentID.ToString() ];
				foreach( AbstractObjectNode currentSubNode in subNodes ) {
					TreeNode subNode = new TreeNode();
					subNode.Text = currentSubNode.GetName();
					subNode.ImageIndex = currentSubNode.GetType();
					subNode.SelectedImageIndex = currentSubNode.GetType();
					subNode.ToolTipText = currentSubNode.GetDescription();
					addNodesToNode( currentSubNode.GetID() , subNode );
					parentNode.Nodes.Add( subNode );
				}
			}
		}
	}
}
