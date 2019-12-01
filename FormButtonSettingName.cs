using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatrixControl
{
    public partial class FormButtonSettingName : Form
    {
        ButtonsPreferences mParent;
        VirtualButton virtualButton;
        DataGridViewRow dataGridViewRow;
        string buttonName;
        
        ///------------------------------------------------------------------------------------------------------------------------
        public FormButtonSettingName(ButtonsPreferences parent, DataGridViewRow dgvr)// VirtualButton virtual_button)
        {
            InitializeComponent();

            mParent = parent;
            virtualButton = (VirtualButton)dgvr.Cells["Key"].Value;
            virtualButton.NoRouting = true;
            dataGridViewRow = dgvr;

            DataRow dr = virtualButton.dataRow;
            if (dr["PortLabel"] != DBNull.Value)
                txtButtonName.Text = virtualButton.dataRow.PortLabel;

            string button_type = "Входная кнопка";
            if (virtualButton.TypeButton == ButtonType.OutputPort)
                button_type = "Выходная кнопка";
            string panel_name = virtualButton.mParent.AliasName == String.Empty ? virtualButton.mParent.sName : virtualButton.mParent.AliasName;
            this.Text = button_type + " на роутере" + " " + panel_name;
        }
        ///------------------------------------------------------------------------------------------------------------------------


        ///------------------------------------------------------------------------------------------------------------------------
        private void btnSetName_Click(object sender, EventArgs e)
        {
            DialogResult res =
                    MessageBox.Show("Название кнопки будет записано в роутер. Продолжить?",
                        Application.ProductName, MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
            if (res == DialogResult.No)
                return;

            if (buttonName == String.Empty)
            {
                MessageBox.Show("Название кнопки не задано.",
                        Application.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                return;
            }
            
            try
            {
                buttonName = virtualButton.SetNewNameButton(buttonName);
                virtualButton.dataRow.AcceptChanges();
                txtButtonName.Text = buttonName;
                dataGridViewRow.Cells["Название"].Value = buttonName;
                virtualButton.Update(virtualButton.mParent.mParent.RemoveNumberInButtonName,
                                virtualButton.mParent.mParent.CarryNameButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                        Application.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
            }
        }
        ///------------------------------------------------------------------------------------------------------------------------


        ///------------------------------------------------------------------------------------------------------------------------
        private void txtButtonName_TextChanged(object sender, EventArgs e)
        {
            buttonName = txtButtonName.Text.Trim();
        }
        ///------------------------------------------------------------------------------------------------------------------------


        ///------------------------------------------------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            //if (txtButtonName.Text != txtButtonName.Tag.ToString())
            //    DialogResult = DialogResult.OK;
            //else
            //    DialogResult = DialogResult.Cancel;
        }
        ///------------------------------------------------------------------------------------------------------------------------


        ///------------------------------------------------------------------------------------------------------------------------
        private void btnSetNameFromRouter_Click(object sender, EventArgs e)
        {
            try
            {
                Data.Preferences.PortRow data_row = virtualButton.GetNameButton(ModeName.Router, true);
                virtualButton.dataRow.PortLabel = data_row.PortLabel;
                virtualButton.dataRow.AcceptChanges();
                txtButtonNameFromRouter.Text = virtualButton.dataRow.PortLabel;
                dataGridViewRow.Cells["Название"].Value = virtualButton.dataRow.PortLabel;
                virtualButton.Update(virtualButton.mParent.mParent.RemoveNumberInButtonName,
                                virtualButton.mParent.mParent.CarryNameButton);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                        Application.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
            }


        }
        ///------------------------------------------------------------------------------------------------------------------------


        ///------------------------------------------------------------------------------------------------------------------------
        private void FormButtonSettingName_FormClosing(object sender, FormClosingEventArgs e)
        {
            virtualButton.NoRouting = false;
        }
        ///------------------------------------------------------------------------------------------------------------------------
    }
}
