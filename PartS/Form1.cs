/*
 *PartY Windows App, Inventory Management System
 *
 *Arthur Mezheritskiy
 *Version 1.1.0
 *Released 7/25/2018
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

namespace PartS
{
    public partial class Form1 : Form
    {
        string filepath;
        int _;

        /* Center Justify
         * 
         * Centers a provided string over provided number of spaces
         */
        private string centerJustify(string original, int justification)
        {
            while (justification < original.Length)
                justification *= 2;

            string final = "";
            double left = (justification - original.Length) * 0.75;
            if ((original.Length / 4) > 1)
                left -= (original.Length / 8);
            double right = (justification - original.Length) * 1.5 - left;
            if ((original.Length / 4) > 1)
                right -= (original.Length / 4) - 1;



            for (int i = 0; i < left; i++)
            {
                final += " ";
            }

            final += original;

            for (int i = 0; i < right; i++)
            {
                final += " ";
            }

            return final;
        }

        private void refreshAll()
        {
            refreshRemovePrice();
            refreshRemoveMerch();
            refreshRemoveCons();
            refreshImage();
            getTotal();
        }

        private void refreshImage()
        {
            try
            {
                string[] values = checkedListBox1.GetItemText(checkedListBox1.SelectedItem).Split('　');
                if (System.Int32.TryParse(values[0], out _) == false)
                    return;
                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT DISTINCT Image
                                         FROM MERCH
                                         WHERE Name = '{0}' AND quantity = {1};", values[1], values[0]);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    Bitmap bmp = new Bitmap(Convert.ToString(row["Image"]));
                    pictureBox1.Image = bmp;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Refresh Image Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void refreshRemovePrice()
        {
            try
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                textBox1.Text = "";
                textBox2.Text = "";

                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT DISTINCT Sizlong, Cost
                                         FROM PRICE
                                         ORDER BY Cost, Sizlong ASC;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    listBox1.Items.Add(String.Format("{0:$#,###.#0}", row["Cost"]) + "　" + Convert.ToString(row["Sizlong"]));
                    listBox2.Items.Add(String.Format("{0:$#,###.#0}", row["Cost"]) + "　" + Convert.ToString(row["Sizlong"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Refresh Price Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshRemoveMerch()
        {
            try
            {
                listBox3.Items.Clear();
                listBox7.Items.Clear();
                checkedListBox1.Items.Clear();
                textBox3.Text = "";
                textBox4.Text = "";


                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT DISTINCT Name, quantity
                                         FROM MERCH
                                         ORDER BY Name, quantity ASC;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    listBox3.Items.Add(Convert.ToString(row["quantity"]) + '　' + Convert.ToString(row["Name"]));
                    listBox7.Items.Add(Convert.ToString(row["quantity"]) + '　' + Convert.ToString(row["Name"]));
                    checkedListBox1.Items.Add(Convert.ToString(row["quantity"]) + '　' + Convert.ToString(row["Name"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Refresh Merch Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void refreshRemoveCons()
        {
            try
            {
                listBox4.Items.Clear();
                listBox6.Items.Clear();
                textBox5.Text = "";

                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT DISTINCT Name
                                         FROM CON
                                         ORDER BY Name ASC;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    listBox4.Items.Add(Convert.ToString(row["Name"]));
                    listBox6.Items.Add(Convert.ToString(row["Name"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Refresh Cons Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog2.ShowDialog();
                filepath = openFileDialog2.FileName;
                if (filepath.Length <= 18 || filepath.Substring(filepath.Length - 18, 18) != "PartYInventory.mdf")
                    return;
                refreshAll();
                Bitmap bmp = new Bitmap(filepath.Substring(0, filepath.Length - 18) + "PartYLogo.png");
                pictureBox2.Image = bmp;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Dialog Filepath Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("You must enter a name and a price.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string name = textBox1.Text.Replace("'", "''");
                double Price = 0;
                string sPrice = String.Format("{0:$#,###.#0}", textBox2.Text.Replace("'", "''"));

                if (System.Double.TryParse(sPrice, out Price) == false)
                {
                    MessageBox.Show("Price must be a number.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                double Manuf = 0;
                string sManuf = String.Format("{0:$#,###.#0}", textBox12.Text.Replace("'", "''"));

                string sql;
                if (System.Double.TryParse(sManuf, out Manuf) == false)
                {
                    sql = string.Format(@"INSERT INTO PRICE(Sizlong,Cost) VALUES('{0}',{1});", name, Price);
                }
                else
                {
                    sql = string.Format(@"INSERT INTO PRICE(Sizlong,Cost,Manuf) VALUES('{0}',{1},{2});", name, Price, Manuf);
                }

                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();
                db.Close();

                refreshRemovePrice();
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().ToString().Contains("UNIQUE KEY"))
                {
                    MessageBox.Show("That name already exists! Pick a different one.\n" + ex.Message, "Duplicate Insert Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error Code:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void refreshAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] values = listBox1.GetItemText(listBox1.SelectedItem).Split('　');
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            string sql = string.Format(@"DELETE FROM PRICE WHERE Sizlong = '{0}' AND Cost = {1}", values[1], values[0]);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            db.Open();
            cmd.ExecuteNonQuery();
            db.Close();

            refreshRemovePrice();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("You must enter a name and a price.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string name = textBox3.Text.Replace("'", "''");
                int quantity = 0;
                if (System.Int32.TryParse(textBox4.Text, out quantity) == false)
                {
                    MessageBox.Show("Invalid quantity.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                string[] price = listBox2.GetItemText(listBox2.SelectedItem).Split('　');
                int priceID = -999;

                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT Siz FROM PRICE
                                         WHERE Sizlong = '{0}' AND Cost = {1};", price[1], price[0]);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    priceID = (Convert.ToInt32(row["Siz"]));
                }
                int nsfw = 0;
                if (checkBox1.Checked)
                    nsfw = 1;

                db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                sql = string.Format(@"INSERT INTO MERCH(Name,Siz,forsale,quantity,nsfw) VALUES('{0}',{1},1,{2},{3});", name, priceID, quantity, nsfw);
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();
                db.Close();

                refreshRemoveMerch();
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().ToString().Contains("UNIQUE KEY"))
                {
                    MessageBox.Show("That name already exists! Pick a different one.\n" + ex.Message, "Duplicate Insert Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error Code:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            try
            {
                string[] values = listBox3.GetItemText(listBox3.SelectedItem).Split('　');
                string sql = string.Format(@"
                                         BEGIN TRANSACTION;

                                         DECLARE @pid AS INT;


                                         SET @pid = (SELECT PID FROM MERCH WHERE Name = '{0}' AND quantity = {1});

                                         DELETE FROM SALE
                                         WHERE RID IN
										 (SELECT RID FROM SUBSALE
										 WHERE PID = @pid);

                                         DELETE FROM SUBSALE
                                         WHERE PID = @pid;

										 DELETE FROM MERCH
										 WHERE PID = @pid;

                                         COMMIT TRANSACTION;", values[1], values[0]);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();
                db.Close();

                refreshRemoveMerch();
            }
            catch(Exception ex)
            {
                
                string sql = string.Format(@"
                        ROLLBACK TRANSACTION;
                        ");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Delete failed and successfully rolled back.\nError Code:\n" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                db.Close();
                return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                if (string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("You must enter a name.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sql = string.Format(@"INSERT INTO CON(Name) VALUES('{0}');", textBox5.Text.Replace("'", "''"));
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();
                db.Close();

                refreshRemoveCons();
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().ToString().Contains("UNIQUE KEY"))
                {
                    MessageBox.Show("That name already exists! Pick a different one.\n" + ex.Message, "Duplicate Insert Failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error Code:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            string sql = string.Format(@"DELETE FROM CON WHERE Name = '{0}'", listBox4.GetItemText(listBox4.SelectedItem));
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            db.Open();
            cmd.ExecuteNonQuery();
            db.Close();

            refreshRemoveCons();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            string ImgPath = "NULL";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                ImgPath = ofd.FileName;
            }

            string[] values = listBox3.GetItemText(listBox3.SelectedItem).Split('　');
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            string sql = string.Format(@"UPDATE MERCH
                                         SET Image = '{0}'
                                         WHERE Name = '{1}' AND quantity = {2}", ImgPath, values[1], values[0]);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            db.Open();
            cmd.ExecuteNonQuery();
            db.Close();
            refreshImage();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshImage();
            try
            {
                string[] values = checkedListBox1.GetItemText(checkedListBox1.SelectedItem).Split('　');
                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT DISTINCT PRICE.Sizlong AS Sizlong, MERCH.nsfw AS nsfw FROM MERCH
                                         INNER JOIN PRICE ON MERCH.Siz = PRICE.Siz
                                         WHERE MERCH.Name = '{0}' AND MERCH.quantity = {1}", values[1], values[0]);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    textBox6.Text = Convert.ToString(row["Sizlong"]);
                    textBox7.Text = Convert.ToInt32(row["nsfw"]) == 1 ? "true" : "false";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            foreach (object itemChecked in checkedListBox1.CheckedItems)
            {
                listBox5.Items.Add(itemChecked.ToString());
            }
            textBox10.Text = getTotal();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox5.Items.Clear();
            textBox10.Text = getTotal();
        }

        private string getTotal()
        {
            double total = 0;
            foreach (object item in listBox5.Items)
            {
                string[] values = listBox5.GetItemText(item).Split('　');
                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                string sql = string.Format(@"SELECT DISTINCT PRICE.Cost AS Cost FROM MERCH
                                         INNER JOIN PRICE ON MERCH.Siz = PRICE.Siz
                                         WHERE MERCH.Name = '{0}' AND MERCH.quantity = {1}", values[1], values[0]);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                db.Close();
                DataSet ds = new DataSet();
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    total += Convert.ToDouble(row["Cost"]);
                }
            }
            double checkboxval = 0.0;
            if (checkBox3.Checked)
            {
                if (System.Double.TryParse(textBox9.Text, out checkboxval) == true)
                    total -= checkboxval;
                else
                    textBox9.Text = "0.0";
            }
            if (checkBox2.Checked)
            {
                if (System.Double.TryParse(textBox8.Text, out checkboxval) == true)
                    total *= (1.0 + (checkboxval / 100.0));
                else
                    textBox8.Text = "0.0";
            }
            return String.Format("{0:$#,##0.00}", total);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string venue = "NULL";
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            if (listBox6.SelectedIndex < 0)
            {
                DialogResult dialogResult = MessageBox.Show("You must select a venue for this sale.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (listBox5.Items.Count == 0)
            {
                DialogResult dialogResult = MessageBox.Show("Your cart is empty.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string sql = string.Format(@"BEGIN TRANSACTION");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();

                sql = string.Format(@"DECLARE @cid AS INT;
                                      SET @cid = (SELECT CID FROM CON
                                      WHERE Name = '{0}');

                                      INSERT INTO
                                      SALE(Thetime,CID,Total)
                                      VALUES(GETDATE(),@cid,{1});

                                      DECLARE @rid AS INT;
                                      SET @rid = @@IDENTITY;
                                      SELECT @rid AS RID;
                                      ", listBox6.SelectedItem.ToString(), textBox10.Text);
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    sql = string.Format(@"
                        ROLLBACK TRANSACTION;
                        ");

                    cmd = new SqlCommand();
                    cmd.Connection = db;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Sale no longer exists!?", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    db.Close();
                    return;
                }

                int RID = Convert.ToInt32(result);

                foreach (string item in listBox5.Items)
                {
                    string[] values = item.Split('　');
                    sql = string.Format(@"
                                      INSERT INTO
                                      SUBSALE(RID,PID)
                                      VALUES({1},(SELECT PID FROM MERCH
                                      WHERE Name = '{0}'));

                                      SELECT PID, quantity FROM MERCH
                                      WHERE Name = '{0}';
                                      ", values[1], RID);
                    cmd = new SqlCommand();
                    cmd.Connection = db;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    cmd.CommandText = sql;
                    adapter.Fill(ds);

                    int quantity = 0;
                    int PID = 0;
                    foreach (DataRow row in ds.Tables["Table"].Rows)
                    {
                        quantity = Convert.ToInt32(row["quantity"]);
                        PID = Convert.ToInt32(row["PID"]);
                    }

                    if(quantity > 0)
                    {
                        sql = string.Format(@"
                                      UPDATE MERCH
                                      SET quantity = {0}
                                      WHERE PID = {1};
                                      ", quantity - 1, PID);

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        sql = string.Format(@"
                        ROLLBACK TRANSACTION;
                        ");

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Item is out of stock!", "Transaction Interrupted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        db.Close();
                        return;
                    }
                }

                sql = string.Format(@"COMMIT TRANSACTION;");

                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                db.Close();
                refreshAll();
                listBox5.Items.Clear();
            }
            catch (Exception ex)
            {
                string sql = string.Format(@"
                ROLLBACK TRANSACTION;
                ");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                db.Close();
                MessageBox.Show("Transaction failed and successfully rolled back.\nError Code:\n" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            textBox10.Text = getTotal();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox10.Text = getTotal();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            double discard;
            if (System.Double.TryParse(textBox9.Text, out discard) == true)
                textBox10.Text = getTotal();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            double discard;
            if (System.Double.TryParse(textBox8.Text, out discard) == true)
                textBox10.Text = getTotal();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            chart1.Series["Series"].Points.Clear();
            try
            {
                string One = "NULL";
                string Two = "NULL";
                if (comboBox1.Text == "Year")
                {
                    One = "YEAR(SALE.Thetime)";
                }
                else if (comboBox1.Text == "Month")
                {
                    One = "MONTH(SALE.Thetime)";
                }
                else if (comboBox1.Text == "Venue")
                {
                    One = "CON.Name";
                }
                else
                {
                    return;
                }

                if (comboBox2.Text == "Sales")
                {
                    Two = "COUNT(SALE.RID)";
                }
                else if (comboBox2.Text == "Income")
                {
                    Two = "SUM(SALE.Total)";
                }
                else if (comboBox2.Text == "Profit")
                {
                    Two = "(SUM(SALE.Total) - SUM(PRICE.Manuf))";
                }
                else
                {
                    return;
                }

                string sql = string.Format(@"SELECT {0} AS One, {1} AS Two FROM SALE
                                             INNER JOIN CON ON SALE.CID = CON.CID
                                             INNER JOIN SUBSALE ON SALE.RID = SUBSALE.RID
                                             INNER JOIN MERCH ON SUBSALE.PID = MERCH.PID
                                             INNER JOIN PRICE ON MERCH.Siz = PRICE.Siz
                                             GROUP BY {0}
                                             ORDER BY {0} ASC;", One, Two);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                cmd.CommandText = sql;
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    if(comboBox1.Text == "Month")
                        chart1.Series["Series"].Points.AddXY(CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(row["One"])), Convert.ToInt32(row["Two"]));
                    else if (comboBox1.Text == "Venue")
                        chart1.Series["Series"].Points.AddXY(Convert.ToString(row["One"]), Convert.ToInt32(row["Two"]));
                    else
                        chart1.Series["Series"].Points.AddXY(Convert.ToInt32(row["One"]), Convert.ToInt32(row["Two"]));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Loading statistics failed.\nAre you sure you are connected to the database?\n\nError Code:\n" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
                chart2.Series["Series"].Points.Clear();
                string One = "NULL";
                string Two = "";
                string Three = "";
                if (comboBox3.Text == "Best selling")
                {
                    One = "COUNT(SUBSALE.PID)";
                }
                else if (comboBox3.Text == "Largest income")
                {
                    One = "SUM(PRICE.Cost)";
                }
                else if (comboBox3.Text == "Highest profit")
                {
                    One = "(SUM(PRICE.Cost) - SUM(PRICE.Manuf))";
                    Two = "WHERE Price.Manuf IS NOT NULL";
                    Three = "HAVING(SUM(PRICE.Cost) - SUM(PRICE.Manuf)) > 0"; //only shows things you actually profit on
                }
                else
                {
                    return;
                }

                string sql = string.Format(@"SELECT TOP 10 {0} AS ONE, MERCH.Name, MERCH.PID FROM MERCH
                                         INNER JOIN SUBSALE ON MERCH.PID = SUBSALE.PID
                                         INNER JOIN PRICE ON MERCH.Siz = PRICE.Siz
                                         {1}
                                         GROUP BY MERCH.PID, MERCH.Name
                                         {2}
                                         ORDER BY COUNT(SUBSALE.PID) DESC;", One, Two, Three);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                cmd.CommandText = sql;
                adapter.Fill(ds);


                foreach (DataRow row in ds.Tables["TABLE"].Rows)
                {
                    chart2.Series["Series"].Points.AddXY(Convert.ToString(row["Name"]), Convert.ToInt32(row["One"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Code:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            if (listBox7.SelectedIndex < 0)
            {
                DialogResult dialogResult = MessageBox.Show("You must select an item to update.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string[] values = listBox7.GetItemText(listBox7.SelectedItem).Split('　');
                int toadd = 0;
                if (System.Int32.TryParse(textBox11.Text, out toadd) == false)
                {
                    DialogResult dialogResult = MessageBox.Show("Invalid quantity.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string sql = string.Format(@"BEGIN TRANSACTION");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();

                sql = string.Format(@"SELECT PID, quantity, forsale
                                         FROM MERCH
                                         WHERE Name = '{0}' AND quantity = {1};", values[1], values[0]);
                cmd = new SqlCommand();
                cmd.Connection = db;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                cmd.CommandText = sql;
                adapter.Fill(ds);

                int PID = -1;
                int quantity = 0;

                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    quantity = Convert.ToInt32(row["quantity"]);
                    PID = Convert.ToInt32(row["PID"]);
                    if(Convert.ToInt32(row["forsale"]) == 0)
                    {
                        sql = string.Format(@"
                                    ROLLBACK TRANSACTION;
                                    ");

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;

                        cmd.ExecuteNonQuery();
                        db.Close();
                        DialogResult dialogResult = MessageBox.Show("This item was discontinued.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                quantity += toadd;

                sql = string.Format(@"
                                      UPDATE MERCH
                                      SET quantity = {0}
                                      WHERE PID = {1};
                                      ", quantity, PID);
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = string.Format(@"COMMIT TRANSACTION");
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                db.Close();
            }
            catch (Exception ex)
            {
                string sql = string.Format(@"
                ROLLBACK TRANSACTION;
                ");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                db.Close();
                MessageBox.Show("Transaction failed and successfully rolled back.\nError Code:\n" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                refreshRemoveMerch();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (listBox7.SelectedIndex < 0)
            {
                return;
            }
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            string[] values = listBox7.GetItemText(listBox7.SelectedItem).Split('　');
            string sql = string.Format(@"UPDATE MERCH
                                         SET forsale = {0}
                                         WHERE Name = '{1}' AND quantity = {2}", trackBar1.Value, values[1], values[0]);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            db.Open();
            cmd.ExecuteNonQuery();
            db.Close();
        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            string[] values = listBox7.GetItemText(listBox7.SelectedItem).Split('　');
            string sql = string.Format(@"SELECT forsale FROM MERCH
                                         WHERE Name = '{0}' AND quantity = {1}", values[1], values[0]);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db;
            cmd.CommandText = sql;
            db.Open();
            object result = cmd.ExecuteScalar();
            db.Close();

            trackBar1.Value = Convert.ToInt32(result);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Save database contents as CSV?\nThis may take a few moments.", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Cancel)
                return;
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string sql = string.Format(@"SELECT * FROM CON");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                cmd.CommandText = sql;
                adapter.Fill(ds);

                string path = filepath.Substring(0, filepath.Length - 18) + "tableCON.csv";
                string lines;

                lines = label15.Text.ToString().Split(' ')[1]+"\n";
                lines += "CID,Name\n";
                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    lines += Convert.ToString(row["CID"]) + "," + Convert.ToString(row["Name"]) + "\n";
                }
                File.WriteAllText(path, lines);


                sql = string.Format(@"SELECT * FROM MERCH");
                ds = new DataSet();
                cmd.CommandText = sql;
                adapter.Fill(ds);
                path = filepath.Substring(0, filepath.Length - 18) + "tableMERCH.csv";
                lines = label15.Text.ToString().Split(' ')[1] + "\n";
                lines += "PID,Name,Image,Siz,forsale,nsfw,quantity\n";
                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    lines += Convert.ToString(row["PID"]) + "," + Convert.ToString(row["Name"]) + "," + Convert.ToString(row["Image"]) + "," + Convert.ToString(row["Siz"]) + "," + Convert.ToString(row["forsale"]) + "," + Convert.ToString(row["nsfw"]) + "," + Convert.ToString(row["quantity"]) + "\n";
                }
                File.WriteAllText(path, lines);


                sql = string.Format(@"SELECT * FROM SALE");
                ds = new DataSet();
                cmd.CommandText = sql;
                adapter.Fill(ds);
                path = filepath.Substring(0, filepath.Length - 18) + "tableSALE.csv";
                lines = label15.Text.ToString().Split(' ')[1] + "\n";
                lines += "RID,Thetime,CID,Total\n";
                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    lines += Convert.ToString(row["RID"]) + "," + Convert.ToString(row["Thetime"]) + "," + Convert.ToString(row["CID"]) + "," + Convert.ToString(row["Total"]) + "\n";
                }
                File.WriteAllText(path, lines);


                sql = string.Format(@"SELECT * FROM SUBSALE");
                ds = new DataSet();
                cmd.CommandText = sql;
                adapter.Fill(ds);
                path = filepath.Substring(0, filepath.Length - 18) + "tableSUBSALE.csv";
                lines = label15.Text.ToString().Split(' ')[1] + "\n";
                lines += "RID,PID\n";
                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    lines += Convert.ToString(row["RID"]) + "," + Convert.ToString(row["PID"]) + "\n";
                }
                File.WriteAllText(path, lines);


                sql = string.Format(@"SELECT * FROM SALE");
                ds = new DataSet();
                cmd.CommandText = sql;
                adapter.Fill(ds);
                path = filepath.Substring(0, filepath.Length - 18) + "tableSALE.csv";
                lines = label15.Text.ToString().Split(' ')[1] + "\n";
                lines += "RID,Thetime,CID,Total\n";
                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    lines += Convert.ToString(row["RID"]) + "," + Convert.ToString(row["Thetime"]) + "," + Convert.ToString(row["CID"]) + "," + Convert.ToString(row["Total"]) + "\n";
                }
                File.WriteAllText(path, lines);


                sql = string.Format(@"SELECT * FROM PRICE");
                ds = new DataSet();
                cmd.CommandText = sql;
                adapter.Fill(ds);
                path = filepath.Substring(0, filepath.Length - 18) + "tablePRICE.csv";
                lines = label15.Text.ToString().Split(' ')[1] + "\n";
                lines += "Siz,Sizlong,Cost,manuf\n";
                foreach (DataRow row in ds.Tables["Table"].Rows)
                {
                    lines += Convert.ToString(row["Siz"]) + "," + Convert.ToString(row["Sizlong"]) + "," + Convert.ToString(row["Cost"]) + "," + Convert.ToString(row["Manuf"]) + "\n";
                }
                File.WriteAllText(path, lines);
                Thread.Sleep(1000);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save to CSV failed.\nError Code:\n" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Overwrite Database with CSV?\nThis will delete the current DB contents.", "Continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Cancel)
                return;
            SqlConnection db = new SqlConnection(string.Format("Data Source=(LocalDB)\\{0};AttachDbFilename={1}", "MSSQLLocalDB", filepath));
            try
            {
                string sql = string.Format(@"BEGIN TRANSACTION");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                db.Open();
                cmd.ExecuteNonQuery();

                sql = string.Format(@"DELETE FROM PRICE;");
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                sql = string.Format(@"DELETE FROM CON;");
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                string table = "CON";
                using (var file = new StreamReader(string.Format(@"{0}table{1}.csv", filepath.Substring(0, filepath.Length - 18), table)))
                {
                    string line = file.ReadLine();
                    string[] version = line.Split('.');
                    file.ReadLine();
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        string[] values = line.Split(',');
                        sql = string.Format(@"
                                  SET IDENTITY_INSERT {0} ON
                                  Insert  Into
                                  CON(CID,Name)
                                  Values({1},'{2}');
                                  SET IDENTITY_INSERT {0} OFF
                                  ", table, values[0], values[1]);

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }//while
                }//using

                table = "SALE";
                using (var file = new StreamReader(string.Format(@"{0}table{1}.csv", filepath.Substring(0, filepath.Length - 18), table)))
                {
                    string line = file.ReadLine();
                    string[] version = line.Split('.');
                    file.ReadLine();
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        string[] values = line.Split(',');
                        sql = string.Format(@"
                                  SET IDENTITY_INSERT {0} ON
                                  Insert  Into
                                  SALE(RID,Thetime,CID,Total)
                                  Values({1},{2},{3},{4});
                                  SET IDENTITY_INSERT {0} OFF
                                  ", table, values[0], values[1], values[2], values[3]);

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }//while
                }//using

                table = "PRICE";
                using (var file = new StreamReader(string.Format(@"{0}table{1}.csv", filepath.Substring(0, filepath.Length - 18), table)))
                {
                    string line = file.ReadLine();
                    string[] version = line.Split('.');
                    file.ReadLine();
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        string[] values = line.Split(',');
                        sql = string.Format(@"
                                  SET IDENTITY_INSERT {0} ON
                                  Insert  Into
                                  PRICE(Siz,Sizlong,Cost,Manuf)
                                  Values({1},'{2}',{3},{4});
                                  SET IDENTITY_INSERT {0} OFF
                                  ", table, values[0], values[1], values[2], values[3]);

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }//while
                }//using

                table = "MERCH";
                using (var file = new StreamReader(string.Format(@"{0}table{1}.csv", filepath.Substring(0, filepath.Length - 18), table)))
                {
                    string line = file.ReadLine();
                    string[] version = line.Split('.');
                    file.ReadLine();
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        string[] values = line.Split(',');
                        sql = string.Format(@"
                                  SET IDENTITY_INSERT {0} ON
                                  Insert  Into
                                  MERCH(PID,Name,Image,Siz,forsale,nsfw,quantity)
                                  Values({1},'{2}','{3}',{4},'{5}','{6}',{7});
                                  SET IDENTITY_INSERT {0} OFF
                                  ", table, values[0], values[1], values[2], values[3], values[4], values[5], values[6]);

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }//while
                }//using

                table = "SUBSALE";
                using (var file = new StreamReader(string.Format(@"{0}table{1}.csv", filepath.Substring(0, filepath.Length - 18), table)))
                {
                    string line = file.ReadLine();
                    string[] version = line.Split('.');
                    file.ReadLine();
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        string[] values = line.Split(',');
                        sql = string.Format(@"
                                  SET IDENTITY_INSERT {0} ON
                                  Insert  Into
                                  SALE(RID,Thetime,CID,Total)
                                  Values({1},{2});
                                  SET IDENTITY_INSERT {0} OFF
                                  ", table, values[0], values[1]);

                        cmd = new SqlCommand();
                        cmd.Connection = db;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }//while
                }//using

                sql = string.Format(@"COMMIT TRANSACTION");
                cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                db.Close();
            }
            catch (Exception ex)
            {
                string sql = string.Format(@"
                ROLLBACK TRANSACTION;
                ");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db;
                cmd.CommandText = sql;

                cmd.ExecuteNonQuery();
                db.Close();
                MessageBox.Show("Overwrite from CSV failed.\nTransaction successfully rolled back.\nError Code:\n" + ex.Message, "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox8.Items.Clear();
            double percent;
            if (System.Double.TryParse(textBox14.Text, out percent) == false)
            {
                MessageBox.Show("Please enter a number", "Incorrect Parameter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            listBox8.Items.Add("In the following ranges:");
            string toAdd = "";
            for(int i = 0; i < 6; i++)
            {
                if(i == 0)
                {
                    toAdd = centerJustify(String.Format("{0:C}", (((i + 0.5) / (percent / 100)) - 0.01)), 11) + " and below　  ";
                }
                else if (i == 5)
                {
                    toAdd = centerJustify(String.Format("{0:C}", ((i - 0.5) / (percent / 100))), 11) + " and above    ";
                }
                else
                {
                    toAdd = centerJustify(String.Format("{0:C}", ((i - 0.5) / (percent / 100))), 11) + " - " + centerJustify(String.Format("{0:C}", (((i + 0.5) / (percent / 100)) - 0.01)), 11);
                }

                listBox8.Items.Add(toAdd + " → $" + i);
            }

        }
    }
}
