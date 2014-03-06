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

		private Int32 MinTreeWidth = 180;
		private Int32 CurrentTreeWidth = 0;

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
			nodeTreeView.Nodes[ 0 ].Expand();

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

		private void nodeTreeViewAfterExpand ( object sender , TreeViewEventArgs e ) {
			int CurrentTreeWidth = nodeTreeView.ClientSize.Width;
			if( e.Node.Nodes != null ) {
				foreach( TreeNode node in e.Node.Nodes ) {
					CurrentTreeWidth = Math.Max( CurrentTreeWidth , node.Bounds.Right );
				}
			}
			if( ( this.CurrentTreeWidth + 14 ) < this.MinTreeWidth ) this.CurrentTreeWidth = this.MinTreeWidth; 
			nodeTreeView.Width = this.CurrentTreeWidth;
			//nodeTreeView.ClientSize = new Size( maxRight , nodeTreeView.ClientSize.Height );
		}

		private void nodeTreeViewAfterCollapse ( object sender , TreeViewEventArgs e ) {
			getWidthForNode( nodeTreeView.Nodes[ 0 ] );
			if( this.CurrentTreeWidth < 130 ) this.CurrentTreeWidth = 130;
			nodeTreeView.Width = this.CurrentTreeWidth;
			//nodeTreeView.ClientSize = new Size( this.CurrentTreeWidth , nodeTreeView.ClientSize.Height );
		}

		private void getWidthForNode ( TreeNode node ) {
			if( node.IsExpanded ) {
				if( node.Nodes != null ) {
					foreach( TreeNode currentNode in node.Nodes ) {
						getWidthForNode( currentNode );
					}
				}
			} else {
				this.CurrentTreeWidth = Math.Max( CurrentTreeWidth , node.Bounds.Right );
			}
		}

		private void tableLayoutPanel1_Paint ( object sender , PaintEventArgs e ) {

		}

		private void tabPage1_Click ( object sender , EventArgs e ) {

		}
	}
}
