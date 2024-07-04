using AutoScript.Share;
using Spring.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoScript.Server
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }




        private object locker = new object();
        private IController singleController = (IController)Config.applicationContext.GetObject("SingleController");
        private IController teamController = (IController)Config.applicationContext.GetObject("TeamController");
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = "文本文件|*.txt|所有文件|*.*";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //生成账号列表
                Account.Reload(openFileDialog.FileName);
                Device.Reload();
                List<Account> accounts = Account.GetAccounts();
                List<IDevice> devices = Device.GetAllDevices();
                if (devices.Any())
                {
                    int deviceIndex = 0;//用于跟踪设备列表的索引
                    foreach (Account account in accounts)
                    {
                        if (deviceIndex > devices.Count - 1) break;
                        // 使用模运算确保deviceIndex不会超出设备列表的范围
                        IDevice device = devices[deviceIndex];
                        account.Hwnd = device.Hwnd;
                        account.DeviceName = device.Title;
                        // 更新设备索引
                        deviceIndex++;
                    }

                }



                //绑定到列表控件
                dgvList.DataSource = accounts;
                dgvList.Columns[0].DataPropertyName = "UserName";
                dgvList.Columns[1].DataPropertyName = "Password";
                dgvList.Columns[2].DataPropertyName = "DeviceName";
                dgvList.Columns[3].DataPropertyName = "Hwnd";

                //多余的列隐藏
                foreach (DataGridViewColumn item in dgvList.Columns)
                {
                    if (item.Index > 5)
                    {
                        item.Visible = false;
                    }
                }

            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            Device.Reload();
            //dgvList.DataSource = new BindingList<IDevice>(Device.GetAllDevices());
            List<Account> accounts = (List<Account>)dgvList.DataSource;
            List<IDevice> devices = Device.GetAllDevices();
            if (devices.Any())
            {
                int deviceIndex = 0;//用于跟踪设备列表的索引
                foreach (Account account in accounts)
                {
                    if (deviceIndex > devices.Count - 1) break;
                    // 使用模运算确保deviceIndex不会超出设备列表的范围
                    IDevice device = devices[deviceIndex];
                    account.Hwnd = device.Hwnd;
                    // 更新设备索引
                    deviceIndex++;
                }

            }
            dgvList.DataSource = new BindingList<Account>(accounts);

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<Account> secList = new List<Account>();
            List<Account> accounts = (List<Account>)dgvList.DataSource;
            foreach (DataGridViewRow row in dgvList.SelectedRows)
            {
                // 假设你想获取第一列（索引为0）的内容作为示例
                string cellValue = row.Cells[3].Value?.ToString();
                Account account = accounts.FirstOrDefault(t => t.Hwnd.ToString() == cellValue);
                if (account != null)
                {
                    secList.Add(account);
                }

            }
            secList= secList.OrderBy(t => t.Index).ToList();
            Script.ReLoad(txtScript.Text);
            singleController.Start(secList);
            //btnStart.Enabled = false;
            //btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            List<Account> secList = new List<Account>();
            List<Account> accounts = (List<Account>)dgvList.DataSource;
            foreach (DataGridViewRow row in dgvList.SelectedRows)
            {
                // 假设你想获取第一列（索引为0）的内容作为示例
                string cellValue = row.Cells[3].Value?.ToString();
                Account account = accounts.FirstOrDefault(t => t.Hwnd.ToString() == cellValue);
                if (account != null)
                {
                    secList.Add(account);
                }

            }

            singleController.Stop(secList);
            //btnStart.Enabled = true;
            //btnStop.Enabled = false;
        }

        private void txtScript_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            GameScriptExecutor.StatusChangedEvent += GameHelper_StatusChangedEvent;
        }

        private void GameHelper_StatusChangedEvent(object? sender, EventArgs e)
        {
            GameHelperArgs args = (GameHelperArgs)e;
            lock (locker)
            {
                dgvList.Rows[args.Index].Cells[2].Value = args.DeviceName;
                dgvList.Rows[args.Index].Cells[3].Value = args.Hwnd;
                dgvList.Rows[args.Index].Cells[4].Value = args.Script;
                dgvList.Rows[args.Index].Cells[5].Value = args.Status;
            }
        }

        private void dgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnStartAll_Click(object sender, EventArgs e)
        {
            Script.ReLoad(txtScript.Text);
            singleController.Start();
            //btnStart.Enabled = false;
            //btnStop.Enabled = true;
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            singleController.Stop();
            //btnStart.Enabled = true;
            //btnStopAll.Enabled = false;
        }
    }
}
