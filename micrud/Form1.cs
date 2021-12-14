using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace micrud
{
    public partial class Form1 : Form
    {
        private MySqlConnection conector = null;
        private string id;
        public Form1()
        {
            InitializeComponent();
            this.leer();
        }

        public void conectar()
        {
            MySqlConnectionStringBuilder mySqlConnectionStringBuilder = new
            MySqlConnectionStringBuilder();
            mySqlConnectionStringBuilder.Server = "localhost";
            mySqlConnectionStringBuilder.Port = 33065;
            mySqlConnectionStringBuilder.UserID = "root";
            mySqlConnectionStringBuilder.Password = "";
            mySqlConnectionStringBuilder.IntegratedSecurity = false;
            mySqlConnectionStringBuilder.Database = "mi_crud";
            conector = new MySqlConnection
            (mySqlConnectionStringBuilder.ConnectionString);
            conector.Open();

        }

        public void Cerrar()
        {
            this.conector.Close();
        }
        public void buscador()
        {
            leer($"where nombre like '%{this.textBox2.Text}%' ");
        }
        public void leer(string buscar = "")
        {
            this.conectar();
            MySqlCommand mySqlCommand = this.conector.CreateCommand();
            mySqlCommand.CommandText = $"select * from datospersonales {buscar} ";
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            this.dataGridView1.Rows.Clear();
            while (mySqlDataReader.Read())
            {
                this.dataGridView1.Rows.Add(
                mySqlDataReader["id"].ToString(),
                mySqlDataReader["Nombre"].ToString(),
                mySqlDataReader["Cedula"].ToString()
                );
            }
            this.Cerrar();
        }

        public void insertar()
        {
            this.conectar();
            MySqlCommand mySqlCommand = this.conector.CreateCommand();
            mySqlCommand.CommandText = "insert into datospersonales(Nombre,Cedula) values(@Nombre, @Cedula); ";
             mySqlCommand.Parameters.Add(new MySqlParameter
             ("@Nombre", this.textBox1.Text));
            mySqlCommand.Parameters.Add(new MySqlParameter("@Cedula",
            this.maskedTextBox1.Text));
            int registrosAfectados = mySqlCommand.ExecuteNonQuery();
            MessageBox.Show(registrosAfectados.ToString());
            this.Cerrar();
        }

        public void update()
        {
            this.conectar();
            MySqlCommand mySqlCommand = this.conector.CreateCommand();
            mySqlCommand.CommandText = "update datospersonales set Nombre = @Nombre,Cedula = @Cedula where id = @id; ";
            mySqlCommand.Parameters.Add(new MySqlParameter("@Nombre",
            this.textBox1.Text));
            mySqlCommand.Parameters.Add(new MySqlParameter("@Cedula",
            this.maskedTextBox1.Text));
            mySqlCommand.Parameters.Add(new MySqlParameter("@id", this.id));
            int registrosAfectados = mySqlCommand.ExecuteNonQuery();
            MessageBox.Show(registrosAfectados.ToString());
            this.Cerrar();
        }

        public void delete()
        {
            DialogResult respuestaUsuario = MessageBox.Show(
            $"Realmente desea eliminar este registro { this.textBox1.Text} con cedula { this.textBox2.Text}",
            "Confirmacion",
             MessageBoxButtons.YesNo,
             MessageBoxIcon.Question
                );
            if (respuestaUsuario == DialogResult.Yes)
            {
                this.conectar();
                MySqlCommand mySqlCommand = this.conector.CreateCommand();
                mySqlCommand.CommandText = "delete from datospersonales where id = @id; ";
                mySqlCommand.Parameters.Add(new MySqlParameter("@id", this.id));
                int registrosAfectados = mySqlCommand.ExecuteNonQuery();
                MessageBox.Show(registrosAfectados.ToString());
                this.Cerrar();
            }
        }

            public void cancel() {
                this.id = "0";
                this.textBox1.Text = "";
                this.maskedTextBox1.Text = "";
            }


        



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.insertar();
            this.leer();
            this.cancel();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.id = this.dataGridView1.CurrentRow.Cells[0].Value.ToString
            ();//columna 0 igual id
            this.textBox1.Text = this.dataGridView1.CurrentRow.Cells
            [1].Value.ToString();//columna 1 nombre
            this.maskedTextBox1.Text = this.dataGridView1.CurrentRow.Cells
            [2].Value.ToString();//columna 2 cedula
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            this.cancel();
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            this.delete();
            this.leer();
            this.cancel();
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            this.update();
            this.leer();
            this.cancel();
        }
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            this.buscador();
            /* if (e.KeyCode == Keys.Enter)
            {
            this.buscador();
            }*/
        }
    }
}
