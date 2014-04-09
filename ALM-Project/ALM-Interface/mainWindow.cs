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
using ALM_Interface.UserControls;
using System.Configuration;

namespace ALM_Interface {
	public partial class mainWindow : Form {

		private Dictionary<String , List<AbstractObjectNode>> parentIdDictionary;

		DatabaseFacade database;

		private Int32 MinTreeWidth = 180;
		private Int32 CurrentTreeWidth = 0;

		private FolderControl FolderControl;
		private DeviceControl DeviceControl;
		private DatapointControl DatapointControl;

		public mainWindow () {
			InitializeComponent();

			ImageList nodeTreeImageList = new ImageList();
			nodeTreeImageList.Images.Add( Properties.Resources.FolderNodeOpenIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.RootNodeIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.FolderNodeIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.DeviceNodeIcon );
			nodeTreeImageList.Images.Add( Properties.Resources.DatapointNodeIcon );

			NodeTreeView.ImageList = nodeTreeImageList;

			String DatabaseType = ConfigurationManager.AppSettings[ "DatabaseConnector" ];
			String DatabaseName = ConfigurationManager.AppSettings[ "DatabaseName" ];
			String DatabaseServer = ConfigurationManager.AppSettings[ "DatabaseServer" ];
			String DatabaseUsername = ConfigurationManager.AppSettings[ "DatabaseUsername" ];
			String DatabasePassword = ConfigurationManager.AppSettings[ "DatabasePassword" ];

			// Create a DatabaseFacade instance
			this.database = new DatabaseFacade();
			if( DatabaseType.Equals( "MYSQL" ) ) {
				// Tell the DatabseFacade to use the MySQLConnector
				this.database.SetDatabaseConnector(
					new MySQLConnector( String.Format( "SERVER={0};DATABASE={1};UID={2};PASSWORD={3};" , DatabaseServer , DatabaseName , DatabaseUsername , DatabasePassword ) )
				);
			}

			// Open a connection to the datbase
			this.database.OpenConnection();

			parentIdDictionary = database.GetObjectTreeSortedByParentID();
			PopulateTreeView();
			NodeTreeView.Nodes[ 0 ].Expand();

			this.database.CloseConnection();

			this.FolderControl = new FolderControl();
			this.FolderControl.Dock = DockStyle.Fill;
			ContentPanel.Controls.Add( this.FolderControl );

			this.DeviceControl = new DeviceControl();
			this.DeviceControl.Dock = DockStyle.Fill;
			ContentPanel.Controls.Add( this.DeviceControl );

			this.DatapointControl = new DatapointControl();
			this.DatapointControl.Dock = DockStyle.Fill;
			ContentPanel.Controls.Add( this.DatapointControl );

			ContentPanel.Controls[ 0 ].Visible = true;
			ContentPanel.Controls[ 1 ].Visible = false;
			ContentPanel.Controls[ 2 ].Visible = false;
		}

		private void PopulateTreeView ( ) {
			AbstractObjectNode rootNode = this.parentIdDictionary[ "0" ].ElementAt( 0 );
			TreeNode rootTreeNode = new TreeNode();
			rootTreeNode.Name = rootNode.GetID().ToString();
			rootTreeNode.Text = rootNode.GetName();
			rootTreeNode.ImageIndex = rootNode.GetType();
			rootTreeNode.SelectedImageIndex = rootNode.GetType();
			rootTreeNode.ToolTipText = rootNode.GetDescription();
			AddNodesToNode( rootNode.GetID() , rootTreeNode );
			NodeTreeView.Nodes.Add( rootTreeNode );
		}

		private void AddNodesToNode( Int64 parentID , TreeNode parentNode ) {
			if( this.parentIdDictionary.ContainsKey( parentID.ToString() ) ) {
				List<AbstractObjectNode> subNodes = this.parentIdDictionary[ parentID.ToString() ];
				foreach( AbstractObjectNode currentSubNode in subNodes ) {
					TreeNode subNode = new TreeNode();
					subNode.Name = currentSubNode.GetID().ToString();
					subNode.Text = currentSubNode.GetName();
					subNode.ImageIndex = currentSubNode.GetType();
					subNode.SelectedImageIndex = currentSubNode.GetType();
					subNode.ToolTipText = currentSubNode.GetDescription();
					AddNodesToNode( currentSubNode.GetID() , subNode );
					parentNode.Nodes.Add( subNode );
				}
			}
		}

		private void NodeTreeViewAfterExpand ( object sender , TreeViewEventArgs e ) {
			int CurrentTreeWidth = NodeTreeView.ClientSize.Width;
			if( e.Node.Nodes != null ) {
				foreach( TreeNode node in e.Node.Nodes ) {
					CurrentTreeWidth = Math.Max( CurrentTreeWidth , node.Bounds.Right );
				}
			}
			if( ( this.CurrentTreeWidth + 14 ) < this.MinTreeWidth ) this.CurrentTreeWidth = this.MinTreeWidth; 
			NodeTreeView.Width = this.CurrentTreeWidth;
			//nodeTreeView.ClientSize = new Size( maxRight , nodeTreeView.ClientSize.Height );
		}

		private void NodeTreeViewAfterCollapse ( object sender , TreeViewEventArgs e ) {
			GetWidthForNode( NodeTreeView.Nodes[ 0 ] );
			if( this.CurrentTreeWidth < 130 ) this.CurrentTreeWidth = 130;
			NodeTreeView.Width = this.CurrentTreeWidth;
			//nodeTreeView.ClientSize = new Size( this.CurrentTreeWidth , nodeTreeView.ClientSize.Height );
		}

		private void GetWidthForNode ( TreeNode node ) {
			if( node.IsExpanded ) {
				if( node.Nodes != null ) {
					foreach( TreeNode currentNode in node.Nodes ) {
						GetWidthForNode( currentNode );
					}
				}
			} else {
				this.CurrentTreeWidth = Math.Max( CurrentTreeWidth , node.Bounds.Right );
			}
		}

		private void NodeTreeViewNodeMouseClick ( object Sender , TreeNodeMouseClickEventArgs Events ) {
			if( Events.Node.ImageIndex == RootNode.NODE_TYPE || Events.Node.ImageIndex == FolderNode.NODE_TYPE ) {
				ContentPanel.Controls[ 0 ].Visible = true;
				ContentPanel.Controls[ 1 ].Visible = false;
				ContentPanel.Controls[ 2 ].Visible = false;
			}
			if( Events.Node.ImageIndex == DeviceNode.NODE_TYPE ) {
				ContentPanel.Controls[ 0 ].Visible = false;
				ContentPanel.Controls[ 1 ].Visible = true;
				ContentPanel.Controls[ 2 ].Visible = false;
			}
			if( Events.Node.ImageIndex == DatapointNode.NODE_TYPE ) {
				ContentPanel.Controls[ 0 ].Visible = false;
				ContentPanel.Controls[ 1 ].Visible = false;
				ContentPanel.Controls[ 2 ].Visible = true;
				this.DatapointControl.LoadDatapointByID( Int32.Parse( Events.Node.Name ) , this.database );
			}
		}
	}
}
