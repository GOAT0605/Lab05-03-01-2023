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
using System.IO;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Faculty> FacultyList = new List<Faculty>();
        private List<Major> MajorList = new List<Major>();
        private List<Student> StudentList = new List<Student>();
        StudentContectDB db = new StudentContectDB();
        private string pathImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StudentContectDB db = new StudentContectDB();
            FillFaculty(db.Faculties.ToList());
            BindGrid(db.Students.ToList());
        }
        private void BindGrid(List<Student> ListStudents)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in ListStudents)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells["StdID"].Value = item.StudentID;
                dgvStudent.Rows[index].Cells["StdName"].Value = item.FullName;
                dgvStudent.Rows[index].Cells["Faculty"].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells["AvgScore"].Value = item.AverageScore;
                dgvStudent.Rows[index].Cells["Major"].Value = item.Major != null ? item.Major.MajorName : " ";
                dgvStudent.Rows[index].Cells["AvartarColumn"].Value = item.avatar;
            }
        }
        private void FillFaculty(List<Faculty> listFaculties)
        {
            this.cmbFacluty.DataSource = listFaculties;
            this.cmbFacluty.DisplayMember = "FacultyName";
            this.cmbFacluty.ValueMember = "FacultyID";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Hide();


            Form2 form2 = new Form2();


            form2.Show();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();

            if (Application.OpenForms.Count != 0)
            {
                Application.Exit();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var ID = TxtStdID.Text;
            var ten = TxtName.Text;
            var DTB = TxtAverageScore.Text;
            var Khoa = (int)cmbFacluty.SelectedValue;
            StudentContectDB db = new StudentContectDB();
            
            Student student = new Student()
            {
                FullName = ten,
                StudentID = ID,
                AverageScore = float.Parse(DTB),
                FacultyID = Khoa,
            };

            db.Students.Add(student);
            db.SaveChanges();
            BindGrid(db.Students.ToList());
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentContectDB db = new StudentContectDB();
            string ImagePath ;
            int RowIndex = e.RowIndex;
            if (RowIndex >= 0)
            {
                TxtName.Text = dgvStudent.Rows[RowIndex].Cells["STDName"].Value.ToString();
                TxtStdID.Text = dgvStudent.Rows[RowIndex].Cells["StdID"].Value.ToString();
                cmbFacluty.SelectedItem = dgvStudent.Rows[RowIndex].Cells["Faculty"].Value;
                TxtAverageScore.Text = dgvStudent.Rows[RowIndex].Cells["AvgScore"].Value.ToString();
                ImagePath = dgvStudent.Rows[RowIndex].Cells["AvartarColumn"].Value.ToString();
                if (string.IsNullOrEmpty(ImagePath))
                {
                    pictureBox2.Image = null;
                }
                else
                {
                    string imageDirectory = "Images";
                    string imagePath = Path.Combine(imageDirectory, ImagePath);
                    string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imageFullPath = Path.Combine(parentDirectory, imagePath);

                    if (File.Exists(imageFullPath))
                    {
                        pictureBox2.Image = Image.FromFile(imageFullPath);
                    }
                    else
                    {

                        pictureBox2.Image = null;
                    }
                }
                cmbFacluty.Refresh();
    
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            StudentContectDB db = new StudentContectDB();
            var updateStudent = db.Students.SingleOrDefault(c => c.StudentID.Equals(TxtStdID.Text));
            if (updateStudent != null)

            {

                updateStudent.FullName = TxtName.Text.Trim();

                updateStudent.AverageScore = double.Parse(TxtAverageScore.Text); updateStudent.FacultyID = int.Parse(cmbFacluty.SelectedValue.ToString());
                if (!string.IsNullOrEmpty(pathImage) &&  File.Exists(pathImage))

                {

                    string imageName = TxtStdID.Text.Trim().ToString() + "," + Path.GetExtension(pathImage).TrimStart('.'); 
                    string parentDirectory= Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName; 
                    string imagePath = Path.Combine(parentDirectory, "Images");

                    if (!Directory.Exists(imagePath))

                    {

                        Directory.CreateDirectory(imagePath);

                    }

                    string imageDestinationPath = Path.Combine(imagePath, imageName); File.Copy(pathImage, imageDestinationPath, true); updateStudent.avatar = imageName;
                    db.SaveChanges();
                    BindGrid(db.Students.ToList());
                }
            }
            else
            {
                String NameImage = "";
                if (!string.IsNullOrEmpty(pathImage) && File.Exists(pathImage))

                {

                    string imageName = TxtStdID.Text.Trim().ToString() + "," + Path.GetExtension(pathImage).TrimStart('.');
                    string parentDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    string imagePath = Path.Combine(parentDirectory, "Images");

                    if (!Directory.Exists(imagePath))

                    {

                        Directory.CreateDirectory(imagePath);

                    }

                    string imageDestinationPath = Path.Combine(imagePath, imageName); File.Copy(pathImage, imageDestinationPath, true); NameImage = imageName;
                    db.SaveChanges();
                    BindGrid(db.Students.ToList());
                }
                var ID = TxtStdID.Text;
                var ten = TxtName.Text;
                var DTB = TxtAverageScore.Text;
                var Khoa = (int)cmbFacluty.SelectedValue;

                Student student = new Student()
                {
                    FullName = ten,
                    StudentID = ID,
                    AverageScore = float.Parse(DTB),
                    FacultyID = Khoa,
                    avatar = NameImage,
                };

                db.Students.Add(student);
                db.SaveChanges();
                BindGrid(db.Students.ToList());
            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            StudentContectDB db = new StudentContectDB();
            int rowIndex = dgvStudent.CurrentCell.RowIndex;
            if (rowIndex >= 0)
            {
                string studentID = dgvStudent.Rows[rowIndex].Cells["StdID"].Value.ToString();


                var student = db.Students.FirstOrDefault(s => s.StudentID == studentID);
                if (student != null)
                {
                    db.Students.Remove(student);
                    db.SaveChanges();
                }


                BindGrid(db.Students.ToList()); 
            }
        }

        private void BtnAvatar_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            try
            {
                OpenFileDialog fileOpen = new OpenFileDialog();
                fileOpen.Title = "Chọn hình ảnh Sinh Viên";
                fileOpen.Filter = "Hình ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files(*.*)|*.*";
                if (fileOpen.ShowDialog() == DialogResult.OK)
                {
                    imageLocation = fileOpen.FileName;
                    pictureBox2.Image = Image.FromFile(imageLocation);
                    pathImage = imageLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi không upload ảnh","Lỗi", MessageBoxButtons.OK , MessageBoxIcon.Error);
            }
            }

        private void CbMajor_CheckedChanged(object sender, EventArgs e)
        {
            var ListStudent = new List<Student>();
            if (this.CbMajor.Checked)
            {
                ListStudent = db.Students.Where(p => p.MajorID == null).ToList();
                BindGrid(ListStudent);
            }
            else
            {
                BindGrid(db.Students.ToList());
            }
        }
    }
       
     
    }

