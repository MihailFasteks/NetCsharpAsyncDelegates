namespace NetCsharpAsyncDelegates
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string fileToCopy = textBox1.Text;
            string whereToCopy = textBox2.Text;
            try
            {
                await ReadWriteAsync(fileToCopy, whereToCopy);
                MessageBox.Show("Копирование завершено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка копирования");
            }

        }
        static async Task ReadWriteAsync(string sourcePath, string destPath)
        {

            const int bufferSize = 4096;

            using (FileStream flSource = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true))
            using (FileStream flDest = new FileStream(destPath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true))
            {
                await flSource.CopyToAsync(flDest, bufferSize);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName; 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }
    }
}
