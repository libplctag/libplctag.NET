using libplctag.DataTypes;
using libplctag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using libplctag.Browse;

namespace Tag_Browser
{
    public partial class BrowseTagsDialog : Form
    {
        public BrowseTagsDialog()
        {
            InitializeComponent();
            ApplyDefaultParametersFromCommandline();
        }

        private void ApplyDefaultParametersFromCommandline()
        {
            GateWayIpAddress = "128.0.0.1";
            CpuPath = "0,1";
            PlcType =  PlcType.ControlLogix;
        }

        public string GateWayIpAddress { get; set; }
        public string CpuPath { get; set; }
        public PlcType PlcType { get; set; }
        private TagBrowser Tb;
       
        private void Form1_Load(object sender, EventArgs e)
        {
            var Form = new SettingsDialog();
            Form.GateWayIpAddress = GateWayIpAddress;
            Form.CpuPath = CpuPath;
            Form.PlcType = PlcType;
            if (Form.ShowDialog(this) == DialogResult.OK)
            {
                GateWayIpAddress = Form.GateWayIpAddress;
                CpuPath = Form.CpuPath;
                PlcType = Form.PlcType;
                RefreshData();
            }
        }

        #region Selection
        public IEnumerable<string> SelectedTagNames
        {
            get
            {
                var sel = new List<string>();
                foreach(ListViewItem lvi in this.listViewSelection.Items)
                {
                    sel.Add(lvi.Text);
                }
                return sel;
            }
        }

        public IEnumerable<SelectedTag> SelectedTagd
        {
            get
            {
                var sel = new List<SelectedTag>();
                foreach (ListViewItem lvi in this.listViewSelection.Items)
                {
                    var selTag = new SelectedTag()
                    {
                        TagName = lvi.Text,
                        TagType = lvi.SubItems[1].Text
                    };
                    sel.Add(selTag);
                }
                return sel;
            }
        }

        private void toolStripButtonRemoveSelection_Click(object sender, EventArgs e)
        {
            if (this.listViewSelection.SelectedItems.Count == 0) return;
            foreach (ListViewItem item in this.listViewSelection.SelectedItems)
            {
                this.listViewSelection.Items.Remove(item);
            }
        }

        private void addAllBelowCurrentNodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewTags.SelectedNode == null) return;
            var nodes = new List<TreeNode>();
            foreach (TreeNode tn in treeViewTags.SelectedNode.Nodes)
            {
                nodes.Add(tn);
                foreach (TreeNode ctn in tn.Nodes)
                {
                    nodes.Add(ctn);
                }
            }

            foreach (var tn in nodes)
            {
                treeViewTags.SelectedNode = tn;
                toolStripButtonAddToSelection_ButtonClick(this, EventArgs.Empty);
            }
        }

        private void toolStripButtonAddToSelection_ButtonClick(object sender, EventArgs e)
        {
            if (treeViewTags.SelectedNode == null) return;

            var Node = treeViewTags.SelectedNode;
            var Tag = Node.Tag as TypedEntity;

            var Name = VarNameFromSelectedNode();
            var Type = Tag.GetTypeDescription().Name;

            var item = this.listViewSelection.Items.Add(Name);
            item.SubItems.Add(Type);
            item.ImageKey = "Tag";
        }

        public class SelectedTag
        {
            public string TagName { get; set; }
            public string TagType { get; set; }
        }
        #endregion

        #region Refresh Data
        public void RefreshData()
        {
            this.toolStripButtonRefresh.Enabled = false;
            backgroundWorkerRefreshData.RunWorkerAsync();
        }

        private void backgroundWorkerRefreshData_DoWork(object sender, DoWorkEventArgs e)
        {
            Tb = new TagBrowser(GateWayIpAddress, CpuPath, PlcType);
            Tb.Browse();
        }

        private void backgroundWorkerRefreshData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                return;
            }

            RefreshView();
            this.toolStripButtonRefresh.Enabled = true;
        }
        #endregion

        #region Refresh Tree view
        private bool isRefreshingView = false;
        private void RefreshView()
        {
            //Re-entrance Protection. This can happen if we Throw an exception while 
            //Filtering the Tag item. This can happen if an invalid RegEx is entered as 
            //Search term
            if (isRefreshingView) return; 

            isRefreshingView = true;
            treeViewTags.BeginUpdate();
            var mem = MemorizeExpandedNodes();
            treeViewTags.Nodes.Clear();
            if (Tb == null) return;

            //build Tree-view
            var ContTn = treeViewTags.Nodes.Add("Controller Tags");
            ContTn.ImageKey = "Controller";
            ContTn.SelectedImageKey =  ContTn.ImageKey;
            foreach (var tag in Tb.ControllerTags)
            {
                AddTagToNode(ContTn, tag);
            }

            foreach (var prog in Tb.Programs)
            {
                var ProgTN = treeViewTags.Nodes.Add(prog.ToString());
                ProgTN.Tag = prog;
                ProgTN.ImageKey = "Program";
                ProgTN.SelectedImageKey =  ProgTN.ImageKey;
                foreach (var tag in prog.Tags)
                {
                    AddTagToNode(ProgTN, tag);
                }
            }

            var UdtsTn = treeViewTags.Nodes.Add("Udts");
            UdtsTn.ImageKey = "Udt";
            UdtsTn.SelectedImageKey =  UdtsTn.ImageKey;
            foreach (var udt in Tb.Udts.Values)
            {
                var UdtTn = UdtsTn.Nodes.Add(udt.Name);
                AddUdtToNode(UdtTn, udt);
            }
            ApplyExpandedNodes(mem);
            treeViewTags.EndUpdate();
            isRefreshingView = false;
        }

        private bool isTagFilteredOut(TagEntry tag)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.toolStripTextBoxTagNameFilter.Text))
                {
                    return !System.Text.RegularExpressions.Regex.IsMatch(tag.Name, this.toolStripTextBoxTagNameFilter.Text, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
            }
            catch (Exception)
            {
                this.toolStripTextBoxTagNameFilter.Text = "";
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(this.toolStripTextBoxDataTypeFilter.Text))
                {
                    return !System.Text.RegularExpressions.Regex.IsMatch(tag.GetTypeDescription().Name, this.toolStripTextBoxDataTypeFilter.Text, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
            }
            catch (Exception)
            {
                this.toolStripTextBoxDataTypeFilter.Text = "";
            }

            return false;
        }

        private void AddTagToNode(TreeNode tn, TagEntry tag)
        {
            if (isTagFilteredOut(tag)) return;

            TreeNode TagTn;
            if (showDatatypesToolStripMenuItem.Checked) TagTn = tn.Nodes.Add(tag.ToString());
            else TagTn = tn.Nodes.Add(tag.Name);

            TagTn.Tag = tag;
            TagTn.ImageKey = "Tag";
            TagTn.SelectedImageKey =  TagTn.ImageKey;

            if (tag.isStruct)
            {
                TagTn.ImageKey = "Udt";
                TagTn.SelectedImageKey =  TagTn.ImageKey;      
                AddUdtToNode(TagTn, tag.Udt);
            }
            else if (tag.isArray)
            {
                AppArraySequenceToNode(tag, TagTn);
            }
        }

        private static void AppArraySequenceToNode(TagEntry tag, TreeNode TagTn)
        {
            if (tag.ArrayDimCount == 1)
            {
                for (var i = 0; i < tag.ArrayDims[0]; i++)
                {
                    var ArryTn = TagTn.Nodes.Add(String.Format("[{0}]", i));
                    ArryTn.Tag = new TypedEntity() { Name = ArryTn.Text };
                    ArryTn.ImageKey = "Tag";
                    ArryTn.SelectedImageKey =  ArryTn.ImageKey;
                }
            }
            else if (tag.ArrayDimCount == 2)
            {
                for (var i = 0; i < tag.ArrayDims[1]; i++)
                {
                    for (var j = 0; j < tag.ArrayDims[0]; j++)
                    {
                        var ArryTn = TagTn.Nodes.Add(String.Format("[{0},{1}]", j, i));
                        ArryTn.Tag = new TypedEntity() { Name = ArryTn.Text };
                        ArryTn.ImageKey = "Tag";
                        ArryTn.SelectedImageKey =  ArryTn.ImageKey;
                    }
                }
            }
            else if (tag.ArrayDimCount == 3)
            {
                for (var k = 0; k < tag.ArrayDims[2]; k++)
                {
                    for (var i = 0; i < tag.ArrayDims[1]; i++)
                    {
                        for (var j = 0; j < tag.ArrayDims[0]; j++)
                        {
                            var ArryTn = TagTn.Nodes.Add(String.Format("[{0},{1},{2}]", j, i, k));
                            ArryTn.Tag = new TypedEntity() { Name = ArryTn.Text };
                            ArryTn.ImageKey = "Tag";
                            ArryTn.SelectedImageKey =  ArryTn.ImageKey;
                        }
                    }
                }
            }
        }

        private void AddUdtToNode(TreeNode tn, UdtEntry tag)
        {
            if (tag == null) return;
            foreach(var field in tag.Fields)
            {
                var fieldtn = tn.Nodes.Add(field.ToString());
                fieldtn.Tag = field;
                fieldtn.ImageKey = "Field";
                fieldtn.SelectedImageKey =  fieldtn.ImageKey;

                if (field.isStruct)
                {
                    AddUdtToNode(fieldtn, field.Udt);
                }
            }
        }

        private string VarNameFromSelectedNode()
        {
            if (treeViewTags.SelectedNode == null) return "";

            var Node = treeViewTags.SelectedNode;
            string Path = "";
            while(Node != null)
            {
                var Tag = Node.Tag as TypedEntity;
                if (Tag == null) return Path.Trim('.');

                Path = Tag.Name + "." + Path;
                Node = Node.Parent;
            }

            return Path.Trim('.');

        }

        private ICollection<string> MemorizeExpandedNodes()
        {
            var mem = new List<string>();
            foreach (TreeNode tn in treeViewTags.Nodes)
            {
                WalkTreeNodes(tn, (a) =>
                {
                    if (!a.IsExpanded) return;
                    if (!mem.Contains(a.Text)) mem.Add(a.Text);
                });
            }
            return mem;
        }

        private void ApplyExpandedNodes(ICollection<string> mem)
        {
            foreach (TreeNode tn in treeViewTags.Nodes)
            {
                WalkTreeNodes(tn, (a) =>
                {
                    if (mem.Contains(a.Text)) a.Expand();
                });
            }
        }

        private void WalkTreeNodes(TreeNode node, Action<TreeNode> action)
        {
            action.Invoke(node);
            foreach (TreeNode nodetn in node.Nodes)
            {
                WalkTreeNodes(nodetn, action);
            }
        }

        #endregion

        #region Event Handling
        private void treeViewTags_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBoxCurrentSelection.Text = VarNameFromSelectedNode();
        }

        private void toolStripTextBoxTagNameFilter_TextChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void toolStripTextBoxDataTypeFilter_TextChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void toolStripButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            var Form = new SettingsDialog();
            Form.GateWayIpAddress = GateWayIpAddress;
            Form.CpuPath = CpuPath;
            Form.PlcType = PlcType;
            Form.ShowDialog(this);
        }

        private void showDatatypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showDatatypesToolStripMenuItem.Checked = !showDatatypesToolStripMenuItem.Checked;
            RefreshView();
        }
        #endregion

        #region Monitor Values
        private void toolStripButtonViewValues_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem lvi in this.listViewSelection.Items)
                {
                    //set up the Tag that will let us upload the Inventory information
                    var tag = new libplctag.Tag();
                    tag.Name = lvi.Text; //this tag gives us all Controller tags
                    tag.Gateway = GateWayIpAddress;
                    tag.Path = CpuPath;
                    tag.PlcType = PlcType;
                    tag.Protocol = Protocol.ab_eip;
                    tag.Timeout = Tb.Timeout;

                    //Read the Tags data, which comes as an byte[]
                    tag.Initialize();
                    tag.Read();
                    var v = FormatValueToString(tag, lvi.SubItems[1].Text);
                    tag.Dispose();

                    if (lvi.SubItems.Count <= 2) lvi.SubItems.Add("");
                    lvi.SubItems[2].Text = v.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string FormatValueToString(libplctag.Tag tag, string DataType)
        {
            if (DataType.ToLower().Contains("bool"))
            {
                return tag.GetBit(0).ToString();
            }
            else if (DataType.ToLower().Contains("lint"))
            {
                return tag.GetInt64(0).ToString();
            }
            else if (DataType.ToLower().Contains("dint"))
            {
                return tag.GetInt32(0).ToString();
            }
            else if (DataType.ToLower().Contains("sint"))
            {
                return tag.GetInt8(0).ToString();
            }
            else if (DataType.ToLower().Contains("int"))
            {
                return tag.GetInt16(0).ToString();
            }
            else if (DataType.ToLower().Contains("lreal"))
            {
                return tag.GetFloat64(0).ToString();
            }
            else if (DataType.ToLower().Contains("real"))
            {
                return tag.GetFloat32(0).ToString();
            }
            else if (DataType.ToLower().Contains("string"))
            {
                return tag.GetString(0).ToString();
            }
            return ByteArrayToString(tag.GetBuffer());
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        #endregion
    }
}