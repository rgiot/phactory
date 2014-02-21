using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CPCBitmap.View
{
    public partial class View : UserControl
    {
        public PhactoryHost.Database.Resource Resource;
        public Document.Document Document = new Document.Document();

        private bool modified;
        public bool IsReady = false;

        private const int FullScreenBitmap = 0;
        private const int SpriteRawData = 1;
        private const int SpriteScreenData = 2;
        private const int SpriteData = 3;
        private const int SpriteOpcodes = 4;
        private const int SpriteFullScreen = 5;
        private const int Font = 6;

        private int XIncrement = 0;
        private int maxColors = 0;

        public View(PhactoryHost.Database.Resource resource)
        {
            InitializeComponent();

            Resource = resource;
            modified = false;

            lvFiles.Clear();
            lvFiles.Columns.Add("Filename", 172);
            lvFiles.Columns.Add("WidthCPC", 64);
            lvFiles.Columns.Add("Width", 30);
            lvFiles.Columns.Add("Height", 30);
            
            lvPalette.Clear();
            lvPalette.Columns.Add("Pen", 100);
            lvPalette.Columns.Add("Color", 100);
            lvPalette.Columns.Add("Gate Array", 100);

            cbMode.Items.Clear();
            cbMode.Items.Add("MODE0");
            cbMode.Items.Add("MODE1");
            cbMode.Items.Add("MODE2");

            RefreshProperties();
        }

        private List<string> GetFilterExtension()
        {
            List<string> filterExtension = new List<string>();
            filterExtension.Add(".bmp");
            filterExtension.Add(".gif");
            filterExtension.Add(".jpg");
            filterExtension.Add(".png");
            filterExtension.Add(".tiff");

            return filterExtension;
        }

        private ListViewItem AddListViewItem(PhactoryHost.Database.Resource resource, Document.Item docItem)
        {
            FileInfo fileInfo = Plugin.ControllerEditor.Host.GetFileInfo(resource);
            if ( fileInfo.Exists == false )
            {
                return null;
            }

            ListViewItem item = new ListViewItem(resource.RelativePath, 0); // Relative Path
            //item.SubItems.Add(((fileInfo.Length / 1024) + 1).ToString() + " Kb"); // File Size

            Image image = Image.FromFile(fileInfo.FullName);
            item.SubItems.Add("" + image.Width / XIncrement); // Width
            item.SubItems.Add("" + image.Width); // Width
            item.SubItems.Add("" + image.Height); // Height

            item.Tag = docItem;
            
            lvFiles.Items.Add(item);

            SetModified(true);

            return item;
        }

        List<Image> processedImages = new List<Image>();

        public void CreateProcessedImages()
        {
            processedImages = new List<Image>();

            foreach (Document.Item item in Document.Items)
            {
                Document.IntermediateImage intermediateImage = item.IntermediateImage;

                Bitmap processedBitmap = new Bitmap(intermediateImage.Width * XIncrement, intermediateImage.Height, PixelFormat.Format24bppRgb);

                for (int y = 0; y < intermediateImage.Height; y++)
                {
                    for (int x = 0; x < intermediateImage.Width; x++)
                    {
                        /* from 0 to 15 */
                        int pen = intermediateImage.Data[(y * intermediateImage.Width) + x];
                        
                        /* from 0 to 26 */
                        int color = 0; // default color
                        if (pen < Document.GetCPCPaletteIndices().Count)
                        {
                            color = Document.GetCPCPaletteIndices()[pen];
                        }                       

                        /* argb */
                        int argb = Document.GetCPCPalette()[color];

                        Color processedColor = Color.FromArgb(argb);

                        int xStart = x * XIncrement;
                        for (int i = 0; i < XIncrement; i++)
                        {
                            processedBitmap.SetPixel(xStart+i, y, processedColor);
                        }
                    }
                }

                processedImages.Add((Image)processedBitmap);
            }
        }

        public void RefreshUI()
        {
            cbMode.SelectedIndex = Document.VideoMode;

            RefreshVideoMode();

            var selectedItem = GetSelectedItem();

            ListViewItem selLvItem = null;

            var toRemove = new List<Document.Item>();

            tbStartIndex.Text = "" + Document.StartIndex;

            bool isGroupDelimiter = false;
            int groupCount = 0;
            Color groupColor = Color.Empty;

            lvFiles.Items.Clear();            
            foreach (Document.Item item in Document.Items)
            {
                PhactoryHost.Database.Resource resource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
                if (resource == null)
                {
                    toRemove.Add(item);
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                    continue;
                }

                ListViewItem lvItem = AddListViewItem(resource, item);
                if (lvItem == null)
                {
                    toRemove.Add(item);
                    Plugin.ControllerEditor.Host.Log("Unknown resource identifier : " + item.ResourceID);
                    continue;
                }

                if (item.IsGroupDelimiter)
                {
                    if (isGroupDelimiter)
                    {
                        lvItem.BackColor = groupColor;
                        
                        isGroupDelimiter = false;
                        groupColor = Color.Empty;
                    }
                    else
                    {
                        isGroupDelimiter = true;

                        if (groupCount == 0)
                        {
                            groupColor = Color.PaleTurquoise;
                        }
                        else if (groupCount == 1)
                        {
                            groupColor = Color.LightGreen;
                        }
                        else if (groupCount == 2)
                        {
                            groupColor = Color.LightSalmon;
                        }
                        else if (groupCount == 3)
                        {
                            groupColor = Color.LightCyan;
                        }

                        groupCount++; 
                        
                        lvItem.BackColor = groupColor;
                    }
                }
                else
                {
                    if (groupColor != Color.Empty)
                    {
                        lvItem.BackColor = groupColor;
                    }
                }
                
                if (item == selectedItem)
                {
                    selLvItem = lvItem;
                }
            }

           /* for (int iColumn = 0; iColumn<lvFiles.Columns.Count; iColumn++)
            {
                lvFiles.AutoResizeColumn(iColumn, ColumnHeaderAutoResizeStyle.ColumnContent);
            }*/

            if (toRemove.Count > 0)
            {
                SetModified(true);
                MessageBox.Show("" + toRemove.Count + " resources were found as referenced but missing in the project !\n\nThese references have been automatically removed.", "Missing references", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            foreach (Document.Item removeItem in toRemove)
            {
                Document.Items.Remove(removeItem);
            }

            Document.CompileInternal();

            CreateProcessedImages();

            RefreshUIIndices();

            lvFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            if (selLvItem != null)
            {
                selLvItem.Selected = true;
            }

            if (selectedItem == null)
            {
                if (lvFiles.Items.Count > 0)
                {
                    lvFiles.Items[0].Selected = true;
                }
            }
        }

        public void RefreshUIIndices()
        {
            bool failed = false;

            lvPalette.Items.Clear();
            for (int i = 0; i < Document.GetCPCPaletteIndices().Count; i++)
            {
                int index = Document.GetCPCPaletteIndices()[i];
                int color = Document.GetCPCPalette()[index];

                var item = new ListViewItem("" + (i+Document.StartIndex)); // Pen
                item.SubItems.Add("" + index); // Color
                item.SubItems.Add( "0x" + String.Format("{0:X2}", Document.GetCPCPaletteGateArray()[index]) ); // Color (Gate Array)

                item.BackColor = Color.FromArgb(color);

                if (index < maxColors)
                {
                    item.ForeColor = Color.White;
                }
                else
                {
                    item.ForeColor = Color.Black;
                }

                if ((i + Document.StartIndex) >= maxColors)
                {
                    failed = true;
                }

                lvPalette.Items.Add(item);
            }

            if (failed)
            {
                PaletteLabel.ForeColor = Color.Red;
                PaletteLabel.Text = "Invalid palette !";
            }
            else
            {
                PaletteLabel.ForeColor = Color.Green;
                PaletteLabel.Text = "Palette is OK";
            }
        }

        public bool IsModified()
        {
            return modified;
        }

        public void SetModified(bool modified)
        {
            if (IsReady == false)
            {
                return;
            }

            if (this.modified != modified)
            {
                this.modified = modified;

                RefreshTitle();
            }
        }

        public delegate void RefreshTitleDelegate();
        public void RefreshTitle()
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new RefreshTitleDelegate(RefreshTitle));
                return;
            }

            var parentPanel = this.Parent as Panel;
            var parentForm = parentPanel.Parent as Form;

            parentForm.Text = Resource.DisplayName;
            if (this.modified)
            {
                parentForm.Text += "*";
            }

            string toolTipText = Plugin.ControllerEditor.Host.GetFileInfo(Resource).FullName;
            if (this.modified)
            {
                toolTipText += "*";
            }
            Plugin.ControllerEditor.Host.SetToolTipText(parentPanel, toolTipText);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var resources = Plugin.ControllerEditor.Host.ShowResourceSelector(null, true, GetFilterExtension());
            if (resources != null)
            {
                foreach (PhactoryHost.Database.Resource resource in resources)
                {
                    Document.Item item = new Document.Item();
                    item.ResourceID = resource.Id;
                    item.Type = CPCBitmap.Document.ItemType.FullScreenBitmap;

                    Document.Items.Add(item);
                }

                SetModified(true);

                RefreshUI();
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var indices = new List<int>();

            foreach (int index in lvFiles.SelectedIndices)
            {
                Document.Item item = Document.Items[index];

                List<PhactoryHost.Database.Resource> otherReferencingResources = Plugin.ControllerEditor.Host.IsOutputResourcesReferencedByOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id));
                if ( otherReferencingResources.Count > 0 )
                {
                    Plugin.ControllerEditor.Host.ShowCantRemoveBecauseOtherResources(Plugin.ControllerEditor.Host.GetResource(item.ResourceID), Plugin.ControllerEditor.Host.GetResource(Resource.Id), otherReferencingResources);
                    return;
                }
                else
                {
                    indices.Add(index);
                }
            }

            // reverse delete
            for (int iIndex = indices.Count - 1; iIndex >= 0; iIndex--)
            {
                int index = indices[iIndex];

                Document.Items.RemoveAt(index);
                lvFiles.Items.RemoveAt(index);
            }

            if (indices.Count > 0)
            {
                SetModified(true);
            }
        }

        private void RefreshProperties()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                processedBox.Image = null;

                SetMaskPreview(-1);
                SetMaskMergePreview(-1);
                SetMaskFadePreview(-1);
                SetCloudPreview(-1);
                
                return;
            }

            int index = lvFiles.SelectedIndices[0];
            if (index < 0)
            {
            }
            else
            {
                var item = Document.Items[index];

                var bmpResource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);

                var filename = Plugin.ControllerEditor.Host.GetFileInfo(bmpResource).FullName;
                
                if (index < processedImages.Count)
                {
                    processedBox.Image = processedImages[index];
                }

                SetMaskPreview(item.MaskResourceID);
                SetMaskMergePreview(item.MaskMergeResourceID);
                SetMaskFadePreview(item.MaskFadeResourceID);
                SetCloudPreview(item.CloudResourceID);

                IsReady = false;

                switch (item.Type)
                {
                    case CPCBitmap.Document.ItemType.FullScreenBitmap:
                        this.comboBoxType.SelectedIndex = FullScreenBitmap;
                        isFullScreenTitle.Enabled = true;
                        break;

                    case CPCBitmap.Document.ItemType.SpriteRawData:
                        isFullScreenTitle.Enabled = false;
                        this.comboBoxType.SelectedIndex = SpriteRawData;
                        break;

                    case CPCBitmap.Document.ItemType.SpriteData:
                        isFullScreenTitle.Enabled = false;
                        this.comboBoxType.SelectedIndex = SpriteData;
                        break;

                    case CPCBitmap.Document.ItemType.SpriteScreenData:
                        isFullScreenTitle.Enabled = false;
                        this.comboBoxType.SelectedIndex = SpriteScreenData;
                        break;

                    case CPCBitmap.Document.ItemType.SpriteOpcodes:
                        isFullScreenTitle.Enabled = false;
                        this.comboBoxType.SelectedIndex = SpriteOpcodes;
                        break;

                    case CPCBitmap.Document.ItemType.SpriteFullScreen:
                        this.comboBoxType.SelectedIndex = SpriteFullScreen;
                        isFullScreenTitle.Enabled = false;
                        break;

                    case CPCBitmap.Document.ItemType.Font:
                        this.comboBoxType.SelectedIndex = Font;
                        isFullScreenTitle.Enabled = false;
                        break;
                }

                isMerge.Checked = item.IsMerge;
                isFade.Checked = item.IsFade;
                MergePosX.Text = "" + item.MergePosX;
                MergePosY.Text = "" + item.MergePosY;
                MaskPenIndex.Text = "" + item.MaskPenIndex;

                MaskMergeBitmap.Enabled = isMerge.Checked;
                BrowseMergeMask.Enabled = isMerge.Checked;                
                MergePosX.Enabled = isMerge.Checked;
                MergePosY.Enabled = isMerge.Checked;
                RefreshButton.Enabled = isMerge.Checked;

                MaskFadeBitmap.Enabled = isFade.Checked; 
                BrowseFadeMask.Enabled = isFade.Checked;
                
                cbGroupDelimiter.Checked = item.IsGroupDelimiter;

                isFullScreenTitle.Checked = item.IsFullScreenTitle;
                MaskPenIndex.Enabled = isFullScreenTitle.Checked;                   

                isCloudSprite.Checked = item.IsCloudSprite;
                CloudBitmap.Enabled = isCloudSprite.Checked;
                BrowseCloud.Enabled = isCloudSprite.Checked;

                FontAlignOnCharaterLine.Checked = item.FontAlignOnCharaterLine;
                FontCharWidthInBytes.Text = "" + item.FontCharWidthInBytes;
                
                switch (item.UseMaskType)
                {
                    case CPCBitmap.Document.UseMaskType.NoMask:
                        this.cbColorMask.Checked = false;
                        this.cbMaskBitmap.Checked = false;
                        break;

                    case CPCBitmap.Document.UseMaskType.ColorMask:
                        this.cbColorMask.Checked = true;
                        this.cbMaskBitmap.Checked = false;
                        break;

                    case CPCBitmap.Document.UseMaskType.BitmapMask:
                        this.cbColorMask.Checked = false;
                        this.cbMaskBitmap.Checked = true;
                        break;
                }

                this.PanelColorMask.BackColor = Color.FromArgb(item.ColorMask);

                IsReady = true;
            }
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshProperties();
        }

        private void UpdateData()
        {
            if (IsReady == false)
            {
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            if (this.comboBoxType.SelectedIndex == FullScreenBitmap)
            {
                item.Type = CPCBitmap.Document.ItemType.FullScreenBitmap;
            }
            else if (this.comboBoxType.SelectedIndex == SpriteRawData)
            {
                item.Type = CPCBitmap.Document.ItemType.SpriteRawData;
            }
            else if (this.comboBoxType.SelectedIndex == SpriteData)
            {
                item.Type = CPCBitmap.Document.ItemType.SpriteData;
            }
            else if (this.comboBoxType.SelectedIndex == SpriteScreenData)
            {
                item.Type = CPCBitmap.Document.ItemType.SpriteScreenData;
            }
            else if (this.comboBoxType.SelectedIndex == SpriteOpcodes)
            {
                item.Type = CPCBitmap.Document.ItemType.SpriteOpcodes;
            }
            else if (this.comboBoxType.SelectedIndex == SpriteFullScreen)
            {
                item.Type = CPCBitmap.Document.ItemType.SpriteFullScreen;
            }
            else if (this.comboBoxType.SelectedIndex == Font)
            {
                item.Type = CPCBitmap.Document.ItemType.Font;
            }
            
            item.IsMerge = isMerge.Checked;
            item.IsFade = isFade.Checked;
            item.IsFullScreenTitle = isFullScreenTitle.Checked;
            item.FontAlignOnCharaterLine = FontAlignOnCharaterLine.Checked;
            
            item.IsCloudSprite = isCloudSprite.Checked;
            
            item.UseMaskType = CPCBitmap.Document.UseMaskType.NoMask;
            if (cbColorMask.Checked)
            {
                item.UseMaskType = CPCBitmap.Document.UseMaskType.ColorMask;
            }
            if (cbMaskBitmap.Checked)
            {
                item.UseMaskType = CPCBitmap.Document.UseMaskType.BitmapMask;
            }
            
            item.ColorMask = this.PanelColorMask.BackColor.ToArgb();

            RefreshUI();

            SetModified(true);
        }
                    
        private void ItemType_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void BrowseMask_Click(object sender, EventArgs e)
        {
            CPCBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            this.cbColorMask.Checked = false; 
            this.cbMaskBitmap.Checked = true;

            List<PhactoryHost.Database.Resource> maskResource = Plugin.ControllerEditor.Host.ShowResourceSelector(Plugin.ControllerEditor.Host.GetResource(item.MaskResourceID), false, GetFilterExtension());
            if (maskResource != null)
            {
                item.MaskResourceID = maskResource[0].Id;

                SetMaskPreview(item.MaskResourceID);

                SetModified(true);
            }                
        }

        private CPCBitmap.Document.Item GetSelectedItem()
        {
            if (lvFiles.SelectedIndices.Count == 0)
            {
                return null;
            }

            int index = lvFiles.SelectedIndices[0];

            CPCBitmap.Document.Item item = Document.Items[index];
            return item;
        }

        private void pickColor_Click(object sender, EventArgs e)
        {
            CPCBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            PhactoryHost.Database.Resource bmpResource = Plugin.ControllerEditor.Host.GetResource(item.ResourceID);
            string filename = Plugin.ControllerEditor.Host.GetFileInfo(bmpResource).FullName;

            CPCBitmap.Editor.PreviewColorPicker previewColorPicker = new CPCBitmap.Editor.PreviewColorPicker();
            previewColorPicker.SetImage(Image.FromFile(filename));   
            previewColorPicker.ShowDialog(this);

            this.PanelColorMask.BackColor = Color.FromArgb(previewColorPicker.GetPickedColor());

            this.cbColorMask.Checked = true;
            this.cbMaskBitmap.Checked = false;
            UpdateData();
            SetModified(true);
        }

        private void tbStartIndex_TextChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            int index = 0;
            try
            {
                index = (int) Convert.ToInt16(tbStartIndex.Text);
            }
            catch
            {
            }

            Document.StartIndex = index;

            SetModified(true);

            RefreshUI();
        }

        private void BrowseMergeMask_Click(object sender, EventArgs e)
        {
            CPCBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            List<PhactoryHost.Database.Resource> maskMergeResource = Plugin.ControllerEditor.Host.ShowResourceSelector(Plugin.ControllerEditor.Host.GetResource(item.MaskMergeResourceID), false, GetFilterExtension());
            if (maskMergeResource != null)
            {
                item.MaskMergeResourceID = maskMergeResource[0].Id;

                SetMaskMergePreview(item.MaskMergeResourceID);

                SetModified(true);
            }                
        }

        private void MaskPenIndex_TextChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.MaskPenIndex = 0;
            try
            {
                item.MaskPenIndex = (int)Convert.ToInt16(MaskPenIndex.Text);
            }
            catch
            {
            }

            SetModified(true);
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            UpdateData();

            SetModified(true);
        }

        private void MergePosX_TextChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.MergePosX = 0;
            try
            {
                item.MergePosX = (int)Convert.ToInt16(MergePosX.Text);
            }
            catch
            {
            }

            SetModified(true);
        }

        private void MergePosY_TextChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.MergePosY = 0;
            try
            {
                item.MergePosY = (int)Convert.ToInt16(MergePosY.Text);
            }
            catch
            {
            }

            SetModified(true);
        }

        private void isMerge_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();

            SetModified(true);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void isCloudSprite_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();

            SetModified(true);
        }

        private void BrowseCloud_Click(object sender, EventArgs e)
        {
            CPCBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            List<PhactoryHost.Database.Resource> cloudResource = Plugin.ControllerEditor.Host.ShowResourceSelector(Plugin.ControllerEditor.Host.GetResource(item.CloudResourceID), false, GetFilterExtension());
            if (cloudResource != null)
            {
                item.CloudResourceID = cloudResource[0].Id;

                SetCloudPreview(item.CloudResourceID);

                SetModified(true);
            }  
        }

        private void cbGroupDelimiter_CheckedChanged(object sender, EventArgs e)
        {
            CPCBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.IsGroupDelimiter = cbGroupDelimiter.Checked;

            UpdateData();

            SetModified(true);
        }

        private void isFullScreenTitle_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();

            SetModified(true);
        }

        private void ExportPlusButton_Click(object sender, EventArgs e)
        {
            Plugin.ControllerEditor.Host.Log("Getting final width/height");
            Plugin.ControllerEditor.Host.AppDoEvents();

            int height = 0;
            int width = 0;

            foreach( var image in processedImages )
            {
                if ( image.Width > width )
                {
                    width = image.Width;
                }
                height += image.Height;
            }

            Plugin.ControllerEditor.Host.Log("Defaulting final bitmap");
            Plugin.ControllerEditor.Host.AppDoEvents();

            var bgColor = new Bitmap(processedImages[0]).GetPixel(0, 0);

            var finalBitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            // set default bg color (1st pixel of 1st bitmap)
            for ( int iY = 0; iY<finalBitmap.Height; iY++ )
            {
                for ( int iX = 0; iX<finalBitmap.Width; iX++ )
                {
                    finalBitmap.SetPixel(iX, iY, bgColor);
                }
            }

            int iHeight = 0;

            Plugin.ControllerEditor.Host.Log("Injecting all images into final bitmap");
            Plugin.ControllerEditor.Host.AppDoEvents();

            // draw sub-images in final bitmap
            foreach( var image in processedImages )
            {
                var tempBitmap = new Bitmap(image);

                for ( int iY = 0; iY<tempBitmap.Height; iY++ )
                {
                    for ( int iX = 0; iX<tempBitmap.Width; iX++ )
                    {
                        var color = tempBitmap.GetPixel(iX, iY);

                        finalBitmap.SetPixel(iX, iY + iHeight, color);
                    }
                }

                Plugin.ControllerEditor.Host.AppDoEvents();
                iHeight += image.Height;
            }

            Plugin.ControllerEditor.Host.Log("Done with final bitmap generation!"); 
            
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter =
                "All Bitmap files (*.bmp)|*.bmp";

            saveFileDialog.Title = "Export to...";
            saveFileDialog.FileName = Resource.DisplayName + ".bmp";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != String.Empty)
            {
                finalBitmap.Save(saveFileDialog.FileName, ImageFormat.Bmp);

                Plugin.ControllerEditor.Host.Log("Final bitmap saved"); 
            }
        }

        private void ImportPlusButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter =
                "All Bitmap files (*.bmp)|*.bmp";

            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Import Plus bitmap...";
            openFileDialog.FileName = String.Empty;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var bitmap = new Bitmap(openFileDialog.FileName);

                int asicColorCount = 0;

                for (int iY = 0; iY < bitmap.Height; iY++)
                {
                    for (int iX = 0; iX < bitmap.Width; iX++)
                    {
                        Color srcColor = bitmap.GetPixel(iX, iY);

                        byte g = ((byte)srcColor.G);
                        byte b = ((byte)srcColor.B);
                        byte r = ((byte)srcColor.R);

                        r >>= 4;
                        g >>= 4;
                        b >>= 4;

                        UInt16 pal = (UInt16)b;
                        pal += (UInt16)(16 * (UInt16)r);
                        pal += (UInt16)(256 * (UInt16)g);

                        bool found = false;

                        for (int i = 0; i < asicColorCount; i++)
                        {
                            UInt16 v = Document.CPCAsicPalette[i];
                            if (v == pal)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            if (asicColorCount < 16)
                            {
                                Document.CPCAsicPalette[asicColorCount] = pal;
                                asicColorCount++;
                            }
                        }
                    }
                }

                if (asicColorCount < 17)
                {
                    MessageBox.Show("Import OK !\n\n" + asicColorCount + " ASIC colors found", "Imported OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Too many ASIC colors !\n\n" + asicColorCount + " ASIC colors found", "Bad Import image", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SetModified(true);
            }
        }

        private void BrowseFadeMask_Click(object sender, EventArgs e)
        {
            CPCBitmap.Document.Item item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            List<PhactoryHost.Database.Resource> maskFadeResource = Plugin.ControllerEditor.Host.ShowResourceSelector(Plugin.ControllerEditor.Host.GetResource(item.MaskFadeResourceID), false, GetFilterExtension());
            if (maskFadeResource != null)
            {
                item.MaskFadeResourceID = maskFadeResource[0].Id;

                SetMaskFadePreview(item.MaskFadeResourceID);

                SetModified(true);
            }   
        }

        private void isFade_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();

            SetModified(true);
        }

        private void SetMaskPreview(int maskResourceID)
        {
            if (maskResourceID == -1)
            {
                MaskBitmap.Text = "";
            }
            else
            {
                PhactoryHost.Database.Resource bmpMaskResource = Plugin.ControllerEditor.Host.GetResource(maskResourceID);

                string filename = Plugin.ControllerEditor.Host.GetFileInfo(bmpMaskResource).FullName;

                MaskBitmap.Text = bmpMaskResource.RelativePath;
            }
        }

        private void SetMaskMergePreview(int maskMergeResourceID)
        {
            if (maskMergeResourceID == -1)
            {
                MaskMergeBitmap.Text = "";
            }
            else
            {
                PhactoryHost.Database.Resource bmpMaskMergeResource = Plugin.ControllerEditor.Host.GetResource(maskMergeResourceID);

                string mergeFilename = Plugin.ControllerEditor.Host.GetFileInfo(bmpMaskMergeResource).FullName;

                MaskMergeBitmap.Text = bmpMaskMergeResource.RelativePath;
            }
        }

        private void SetMaskFadePreview(int maskFadeResourceID)
        {
            if (maskFadeResourceID == -1)
            {
                MaskFadeBitmap.Text = "";
            }
            else
            {
                PhactoryHost.Database.Resource bmpMaskFadeResource = Plugin.ControllerEditor.Host.GetResource(maskFadeResourceID);

                string mergeFilename = Plugin.ControllerEditor.Host.GetFileInfo(bmpMaskFadeResource).FullName;

                MaskFadeBitmap.Text = bmpMaskFadeResource.RelativePath;
            }
        }

        private void SetCloudPreview(int cloudResourceID)
        {
            if (cloudResourceID == -1)
            {
                CloudBitmap.Text = "";
            }
            else
            {
                PhactoryHost.Database.Resource bmpCloudResource = Plugin.ControllerEditor.Host.GetResource(cloudResourceID);

                string mergeFilename = Plugin.ControllerEditor.Host.GetFileInfo(bmpCloudResource).FullName;

                CloudBitmap.Text = bmpCloudResource.RelativePath;
            }
        }

        private void cbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            Document.VideoMode = cbMode.SelectedIndex;

            SetModified(true);

            RefreshUI();
        }

        private void RefreshVideoMode()
        {
            XIncrement = 0;
            if (Document.VideoMode == 0)
            {
                maxColors = 16;
                XIncrement = 2;
            }
            else if (Document.VideoMode == 1)
            {
                maxColors = 4;
                XIncrement = 1;
            }
            else
            {
                maxColors = 2;
                XIncrement = 1;
            }
        }

        private void FontAlignOnCharaterLine_CheckedChanged(object sender, EventArgs e)
        {
            UpdateData();

            SetModified(true);
        }
        
        private void FontCharWidthInBytes_TextChanged(object sender, EventArgs e)
        {
            if (IsReady == false)
            {
                return;
            }

            var item = GetSelectedItem();
            if (item == null)
            {
                return;
            }

            item.FontCharWidthInBytes = 0;
            try
            {
                item.FontCharWidthInBytes = (int)Convert.ToInt32(FontCharWidthInBytes.Text);
            }
            catch
            {
            }

            SetModified(true);
        }
    }
}
