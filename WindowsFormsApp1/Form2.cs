using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;
namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        StudentContectDB db = new StudentContectDB();
        private List<Faculty> FacultyList = new List<Faculty>();
        private List<Major> MajorList = new List<Major>();
        
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            StudentContectDB db = new StudentContectDB();
            FillFaculty(db.Faculties.ToList());
            BindGrid(db.Students.ToList());
            FillMajor(db.Majors.ToList());
        }
        private void BindGrid(List<Student> ListStudents)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in ListStudents)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells["StudentID"].Value = item.StudentID;
                dgvStudent.Rows[index].Cells["stdName"].Value = item.FullName;
                dgvStudent.Rows[index].Cells["Faculty"].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells["AverageScore"].Value = item.AverageScore;
            }
        }
        private void FillFaculty(List<Faculty> listFaculties)
        {
            this.cmbFacluty.DataSource = listFaculties;
            this.cmbFacluty.DisplayMember = "FacultyName";
            this.cmbFacluty.ValueMember = "FacultyID";
        }
        private void FillMajor(List<Major> MajorList)
        {
            this.cmbMajor.DataSource = MajorList;
            this.cmbMajor.DisplayMember = "MajorName";
            this.cmbMajor.ValueMember = "MajorID";
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}
